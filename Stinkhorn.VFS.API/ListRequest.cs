using Stinkhorn.API;

namespace Stinkhorn.VFS.API
{
    [RequestDesc(Category = nameof(VFS))]
    public class ListRequest : IRequest
    {
        public string Source { get; set; }
        public string Path { get; set; }
    }

    [ResponseDesc]
    public class ListResponse : IResponse,
        IFilesParent, IFoldersParent
    {
        public IFile[] Files { get; set; }
        public IFolder[] Folders { get; set; }
    }
}