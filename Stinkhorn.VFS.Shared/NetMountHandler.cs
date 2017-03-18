using Stinkhorn.API;
using Stinkhorn.VFS.API;
using System.IO;
using System.Linq;
using static System.Environment;
using static Stinkhorn.VFS.Shared.NetExtensions;
using System;

namespace Stinkhorn.VFS.Shared
{
    [ReqHandlerFilter]
    public class NetMountHandler : IRequestHandler<MountRequest>,
        IRequestHandler<ListRequest>, IRequestHandler<FileRequest>
    {
        const SpecialFolderOption opt = SpecialFolderOption.Create;

        public IResponse Process(FileRequest input)
        {
            var path = FindRealPath(input.Source, input.Path, opt);
            using (var file = File.OpenRead(path))
            {
                var offset = input.Offset;
                if (offset > 0)
                    file.Seek(offset, SeekOrigin.Begin);
                var size = input.Buffer;
                if (size <= 1024)
                    size = 4 * 1024;
                var buffer = new byte[size];
                var bits = file.Read(buffer, 0, buffer.Length);
                byte[] result = null;
                if (bits < buffer.Length)
                {
                    result = new byte[bits];
                    Array.Copy(buffer, result, bits);
                }
                var resp = new FileResponse
                {
                    Relative = input.Path.TrimStart('/'),
                    Source = input.Source,
                    Bytes = result ?? buffer,
                    Offset = offset
                };
                resp.Length = resp.Bytes.LongLength;
                return resp;
            }
        }

        public IResponse Process(ListRequest input)
        {
            var path = FindRealPath(input.Source, input.Path, opt);
            string[] dirs;
            string[] files;
            FetchEntries(path, out dirs, out files);
            return new ListResponse
            {
                Relative = input.Path.TrimStart('/'),
                Source = input.Source,
                Folders = dirs.Select(d => new VfsFolder(d)).ToArray(),
                Files = files.Select(f => new VfsFile(f)).ToArray()
            };
        }

        public IResponse Process(MountRequest input)
        {
            var specialFld = input.Source;
            string[] dirs;
            string[] files;
            if (specialFld == SpecialFolder.MyComputer)
            {
                var allDrives = DriveInfo.GetDrives();
                var drives = allDrives.Where(d => d.DriveType == DriveType.Fixed
                        && d.IsReady).ToDictionary(k => k.RootDirectory.FullName,
                        v => v.VolumeLabel);
                dirs = drives.Values.ToArray();
                files = new string[0];
            }
            else
            {
                var folder = GetFolderPath(specialFld, opt);
                FetchEntries(folder, out dirs, out files);
            }
            return new MountResponse
            {
                Folders = dirs.Select(d => new VfsFolder(d)).ToArray(),
                Files = files.Select(f => new VfsFile(f)).ToArray(),
                Source = input.Source + string.Empty,
                Target = input.Target
            };
        }

        void FetchEntries(string folder, out string[] dirs, out string[] files)
        {
            try
            {
                dirs = Directory.GetDirectories(folder);
                files = Directory.GetFiles(folder);
            }
            catch (UnauthorizedAccessException uae)
            {
                dirs = new string[0];
                files = new[] { uae.ToFileName() };
            }
        }
    }
}