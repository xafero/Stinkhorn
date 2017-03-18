using System;
using System.Diagnostics;
using System.IO;

namespace Stinkhorn.VFS.API
{
    class VirtualStream : Stream
    {
        readonly IFile file;
        readonly long start;
        readonly ReadFileChunk chunk;

        internal long Counter { get; private set; } = 0L;

        public VirtualStream(IFile file, long start, ReadFileChunk chunk)
        {
            this.file = file;
            this.start = start;
            this.chunk = chunk;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = chunk(buffer, offset, count, start);
            Counter += bytesRead;
            return bytesRead;
        }

        public override bool CanRead => Counter <= (file.Size - start);

        public override long Length
        {
            get
            {
                Debugger.Break();
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                Debugger.Break();
                throw new NotImplementedException();
            }

            set
            {
                Debugger.Break();
                throw new NotImplementedException();
            }
        }

        public override bool CanSeek
        {
            get
            {
                Debugger.Break();
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite
        {
            get
            {
                Debugger.Break();
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Debugger.Break();
            throw new NotImplementedException();
        }
    }
}