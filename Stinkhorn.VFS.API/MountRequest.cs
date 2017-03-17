using Stinkhorn.API;
using System.Linq;
using System.Collections.Generic;
using static System.Environment;
using System;

namespace Stinkhorn.VFS.API
{
    [RequestDesc(Category = nameof(VFS))]
    public class MountRequest : IRequest
    {
        public FsType Type { get; set; } = FsType.Auto;
        public SpecialFolder Source { get; set; } = SpecialFolder.MyComputer;
        public string Target { get; set; } = "/drives";
    }

    [ResponseDesc]
    public class MountResponse : IResponse
    {
        public IFile[] Files { get; set; }
        public IFolder[] Folders { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public enum FsType
    {
        Auto
    }

    public interface IEntry
    {
        string Name { get; set; }
    }

    public interface IFile : IEntry
    {

    }

    public interface IFolder : IEntry
    {
        IEntry this[string name] { get; set; }

        string Ref { get; set; }
    }

    public abstract class VfsEntry : IEntry
    {
        public string Name { get; set; }

        protected string ToSimpleName(string name) => name.Split('\\').Last();
    }

    public class VfsFile : VfsEntry, IFile
    {
        public VfsFile() { }

        public VfsFile(string name) : this()
        {
            Name = ToSimpleName(name);
        }
    }

    public class VfsFolder : VfsEntry, IFolder
    {
        public IList<IFolder> Folders { get; set; }
        public IList<IFile> Files { get; set; }
        public string Ref { get; set; }

        public VfsFolder()
        {
            Folders = new List<IFolder>();
            Files = new List<IFile>();
        }

        public VfsFolder(string name) : this()
        {
            Name = ToSimpleName(name);
        }

        public IEntry this[string name]
        {
            get
            {
                return Folders.OfType<IEntry>().Concat(Files)
                    .FirstOrDefault(e => e.Name.Equals(name,
                    StringComparison.InvariantCultureIgnoreCase));
            }
            set
            {
                var folder = value as IFolder;
                if (folder != null)
                    Folders.Add(folder);
                var file = value as IFile;
                if (file != null)
                    Files.Add(file);
            }
        }
    }
}