using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Artivity.Apid.Helpers
{
    internal class ProgressStream : Stream
    {
        #region Members

        private CancellationToken _token;

        public Action<long> ReadCallback { get; set; }

        public Action<long> WriteCallback { get; set; }

        public Stream ParentStream { get; private set; }

        public override long Position
        {
            get { return ParentStream.Position; }
            set { ParentStream.Position = value; }
        }

        public override bool CanRead { get { return ParentStream.CanRead; } }

        public override bool CanSeek { get { return ParentStream.CanSeek; } }

        public override bool CanWrite { get { return ParentStream.CanWrite; } }

        public override bool CanTimeout { get { return ParentStream.CanTimeout; } }

        public override long Length { get { return ParentStream.Length; } }

        #endregion

        #region Constructors

        public ProgressStream(Stream stream, CancellationToken token)
        {
            _token = token;

            ParentStream = stream;

            ReadCallback = delegate { };
            WriteCallback = delegate { };
        }

        public ProgressStream(string content, CancellationToken token)
        {
            _token = token;

            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();

            stream.Position = 0;

            ParentStream = stream;

            ReadCallback = delegate { };
            WriteCallback = delegate { };
        }

        #endregion

        #region Methods

        public override void Flush()
        {
            ParentStream.Flush();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return ParentStream.FlushAsync(cancellationToken);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            _token.ThrowIfCancellationRequested();

            int readCount = ParentStream.Read(buffer, offset, count);

            ReadCallback(readCount);

            return readCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            _token.ThrowIfCancellationRequested();

            return ParentStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _token.ThrowIfCancellationRequested();

            ParentStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _token.ThrowIfCancellationRequested();

            ParentStream.Write(buffer, offset, count);

            WriteCallback(count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            _token.ThrowIfCancellationRequested();

            var linked = CancellationTokenSource.CreateLinkedTokenSource(_token, cancellationToken);
            var readCount = await ParentStream.ReadAsync(buffer, offset, count, linked.Token);

            ReadCallback(readCount);

            return readCount;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            _token.ThrowIfCancellationRequested();

            var linked = CancellationTokenSource.CreateLinkedTokenSource(_token, cancellationToken);
            var task = ParentStream.WriteAsync(buffer, offset, count, linked.Token);

            WriteCallback(count);

            return task;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ParentStream.Dispose();
            }
        }

        #endregion
    }
}
