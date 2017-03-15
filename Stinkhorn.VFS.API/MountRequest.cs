using Stinkhorn.API;
using static System.Environment;

namespace Stinkhorn.VFS.API
{
    [RequestDesc(Category = nameof(VFS))]
    public class MountRequest : IRequest
    {
        public FsType Type { get; set; } = FsType.Auto;
        public SpecialFolder Source { get; set; } = SpecialFolder.MyComputer;
        public string Target { get; set; } = "/";
    }

    [ResponseDesc]
    public class MountResponse : IResponse, IFolder
    {
        public IFile[] Files { get; set; }
        public IFolder[] Folders { get; set; }
    }

    public enum FsType
    {
        Auto
    }

    public interface IEntry
    {

    }

    public interface IFile : IEntry
    {

    }

    public interface IFolder : IEntry
    {
        IFile[] Files { get; }
        IFolder[] Folders { get; }
    }

    public abstract class VfsEntry : IEntry
    {

    }

    public class VfsFile : VfsEntry, IFile
    {

    }

    public class VfsFolder : VfsEntry, IFile
    {

    }
}