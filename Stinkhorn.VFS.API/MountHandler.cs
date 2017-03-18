using Mono.Addins;
using Stinkhorn.API;
using System;
using System.Linq;
using System.Collections.Generic;
using Proc = System.Diagnostics.Process;

namespace Stinkhorn.VFS.API
{
    [Extension]
    public class MountHandler : IResponseHandler<MountResponse, object>
        , IDisposable
    {
        FileServer server;
        IFolder root;
        IDictionary<string, IFolder> refs;

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
                Proc.Start("explorer", url);
            }
            Mount(src, msg);
            InsertResponse(src, msg);
            return ResponseStatus.Handled;
        }

        public static string BuildRefPath(object id, string src)
            => $"{id}@{src}";

        void InsertResponse<T>(object id, T msg)
            where T : ISourceable, IFilesParent, IFoldersParent
        {
            var entries = msg.Files.OfType<IEntry>().Concat(msg.Folders);
            var refPath = BuildRefPath(id, msg.Source);
            var folder = refs[refPath];
            Array.ForEach(entries.ToArray(), e => folder[e.Name] = e);
        }

        void Mount<T>(object id, T msg) where T : ISourceable, ITargetable
        {
            var current = GetFolder(msg.Target, createAllowed: true);
            current.Ref = BuildRefPath(id, msg.Source);
            refs[current.Ref] = current;
        }

        internal IFolder GetFolder(string dest, bool createAllowed)
        {
            const char separator = '/';
            var current = root;
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
            refs?.Clear();
            refs = null;
            root = null;
            server?.Dispose();
            server = null;
        }
    }
}