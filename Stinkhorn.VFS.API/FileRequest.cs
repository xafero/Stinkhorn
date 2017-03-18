using Stinkhorn.API;

namespace Stinkhorn.VFS.API
{
    [RequestDesc(Category = nameof(VFS))]
    public class FileRequest : IRequest
    {
        public string Source { get; set; }
        public string Path { get; set; }

        public long Offset { get; set; }
        public int Buffer { get; set; }
    }

    [ResponseDesc]
    public class FileResponse : IResponse
    {
        public long Offset { get; set; }
        public long Length { get; set; }
        public byte[] Bytes { get; set; }

        public string Source { get; set; }
        public string Relative { get; set; }
    }
}