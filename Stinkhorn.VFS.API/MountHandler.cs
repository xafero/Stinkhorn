using Mono.Addins;
using Stinkhorn.API;
using System;
using System.Linq;
using System.Collections.Generic;
using Proc = System.Diagnostics.Process;
using System.Collections.Concurrent;
using System.Threading;

namespace Stinkhorn.VFS.API
{
    [Extension]
    public class MountHandler : IResponseHandler<MountResponse, object>,
        IResponseHandler<ListResponse, object>, IDisposable, IPublisher,
        IResponseHandler<FileResponse, object>
    {
        FileServer server;
        IFolder root;
        IDictionary<string, IFolder> refs;
        IDictionary<string, string> tgts;
        ConcurrentDictionary<string, ReadFileChunk> chunks;

        public ResponseStatus Process(object src, MountResponse msg)
        {
            if (server == null)
            {
                var user = Guid.NewGuid().ToString("N").Substring(0, 5);
                var pass = Guid.NewGuid().ToString("N").Substring(0, 7);
                var host = "127.0.0.1";
                var port = 21;
                var url = $"ftp://{user}:{pass}@{host}:{port}";
                server = new FileServer(this, host, port, user, pass);
                root = new VfsFolder("");
                refs = new Dictionary<string, IFolder>();
                tgts = new Dictionary<string, string>();
                chunks = new ConcurrentDictionary<string, ReadFileChunk>();
                Proc.Start("explorer", url);
            }
            Mount(src, msg);
            InsertResponse(src, msg);
            return ResponseStatus.Handled;
        }

        public ResponseStatus Process(object src, ListResponse msg)
        {
            InsertResponse(src, msg, msg.Relative);
            return ResponseStatus.Handled;
        }

        public Action<Guid, IMessage> Pub { private get; set; }

        internal void Refresh(Guid id, string src, string path)
        {
            Pub(id, new ListRequest { Source = src, Path = path });
        }

        internal int ReadFile(Guid id, string src, string path,
            byte[] buffer, long offset, long length, long start)
        {
            Pub(id, new FileRequest
            {
                Source = src,
                Path = path,
                Buffer = (int)length,
                Offset = start
            });
            var chunkRef = BuildChunkRef(id, src, path, start);
            ReadFileChunk handle = null;
            var numberOfTries = 0;
            while (handle == null && (numberOfTries++) <= 5)
                if (!chunks.TryGetValue(chunkRef, out handle))
                    Thread.Sleep(100);
            var bytesRead = handle(buffer, offset, length, start);
            chunks.TryRemove(chunkRef, out handle);
            return bytesRead;
        }

        public ResponseStatus Process(object src, FileResponse msg)
        {
            var chunkRef = BuildChunkRef(src, msg.Source, msg.Relative, msg.Offset);
            ReadFileChunk chunk = (b, o, l, s) =>
            {
                Array.Copy(msg.Bytes, 0, b, o, Math.Min(l, msg.Length));
                return (int)msg.Length;
            };
            chunks[chunkRef] = chunk;
            return ResponseStatus.Handled;
        }

        static string BuildChunkRef(object id, string src,
            string relative, long offset)
            => $"{(id + "").Replace("-", "")}@{src}|{relative.TrimStart('/')}|{offset}";

        public static string BuildRefPath(object id, string src)
            => $"{id}@{src}";

        void InsertResponse<T>(object id, T msg, string subPath = null)
            where T : ISourceable, IFilesParent, IFoldersParent
        {
            var entries = msg.Files.OfType<IEntry>().Concat(msg.Folders);
            var refPath = BuildRefPath(id, msg.Source);
            var folder = refs[refPath];
            if (!string.IsNullOrWhiteSpace(subPath))
                folder = GetFolder(subPath, true, start: folder);
            Array.ForEach(entries.ToArray(), e => folder[e.Name] = e);
        }

        void Mount<T>(object id, T msg) where T : ISourceable, ITargetable
        {
            var current = GetFolder(msg.Target, createAllowed: true);
            current.Ref = BuildRefPath(id, msg.Source);
            refs[current.Ref] = current;
            tgts[current.Ref] = msg.Target;
        }

        internal string GetMountPath(VfsEntry entry, out string lastRef)
        {
            lastRef = entry?.LastRef;
            if (string.IsNullOrWhiteSpace(lastRef))
                return null;
            return tgts[lastRef];
        }

        internal IFolder GetFolder(string dest, bool createAllowed,
            IFolder start = null)
        {
            const char separator = '/';
            var current = start ?? root;
            foreach (var part in dest.Split(separator))
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;
                var item = current[part] as IFolder;
                if (item != null)
                {
                    current = item;
                    continue;
                }
                if (!createAllowed)
                    return null;
                var parent = current as VfsFolder;
                parent[part] = current = new VfsFolder(part, parent);
            }
            return current;
        }

        public void Dispose()
        {
            chunks?.Clear();
            chunks = null;
            tgts?.Clear();
            tgts = null;
            refs?.Clear();
            refs = null;
            root = null;
            server?.Dispose();
            server = null;
        }
    }

    internal delegate int ReadFileChunk(byte[] buffer,
        long offset, long length, long start);
}