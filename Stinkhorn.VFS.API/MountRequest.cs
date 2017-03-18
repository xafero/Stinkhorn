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
    public class MountResponse : IResponse,
        ISourceable, ITargetable, IFilesParent, IFoldersParent
    {
        public IFile[] Files { get; set; }
        public IFolder[] Folders { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public interface IFoldersParent
    {
        IFolder[] Folders { get; }
    }

    public interface IFilesParent
    {
        IFile[] Files { get; }
    }

    public interface ITargetable
    {
        string Target { get; }
    }

    public interface ISourceable
    {
        string Source { get; }
    }

    public enum FsType
    {
        Auto
    }

    public interface IEntry
    {
        string Name { get; set; }
        string Ref { get; set; }
    }

    public interface IFile : IEntry
    {
        long Size { get; }
    }

    public interface IFolder : IEntry
    {
        IEntry this[string name] { get; set; }
    }

    public abstract class VfsEntry : IEntry
    {
        public string Name { get; set; }

        protected string ToSimpleName(string name) => name.Split('\\').Last();

        public string Ref { get; set; }

        internal VfsEntry Parent { get; set; }

        internal string LastRef
        {
            get
            {
                var current = this;
                var lastRef = Ref;
                while (string.IsNullOrWhiteSpace(lastRef)
                    && current?.Parent != null)
                {
                    current = current.Parent;
                    lastRef = current.Ref;
                }
                return lastRef;
            }
        }
    }

    public class VfsFile : VfsEntry, IFile
    {
        public long Size { get; set; }

        public VfsFile() { }

        public VfsFile(string name, VfsEntry parent = null) : this()
        {
            Parent = parent;
            Name = ToSimpleName(name);
        }
    }

    public class VfsFolder : VfsEntry, IFolder
    {
        public IList<IFolder> Folders { get; set; }
        public IList<IFile> Files { get; set; }

        public VfsFolder()
        {
            Folders = new List<IFolder>();
            Files = new List<IFile>();
        }

        public VfsFolder(string name, VfsEntry parent = null) : this()
        {
            Parent = parent;
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
                var entry = value as VfsEntry;
                if (entry != null)
                    entry.Parent = this;
            }
        }
    }
}