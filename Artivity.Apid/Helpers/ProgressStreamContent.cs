using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;

namespace Artivity.Apid.Helpers
{
    public delegate void ProgressDelegate(long bytes, long totalBytes, long totalBytesExpected);

    public class ProgressStreamContent : StreamContent
    {
        #region Members

        long _totalBytes;

        long _totalBytesExpected = -1;

        ProgressDelegate _progress;

        public ProgressDelegate Progress
        {
            get { return _progress; }
            set { _progress = value != null ? value : delegate { }; }
        }

        #endregion

        #region Constructors

        public ProgressStreamContent(Stream stream, CancellationToken token)
            : this(new ProgressStream(stream, token))
        {
        }

        public ProgressStreamContent(Stream stream, int bufferSize)
            : this(new ProgressStream(stream, CancellationToken.None), bufferSize)
        {
        }

        public ProgressStreamContent(string content, int bufferSize)
            : this(new ProgressStream(content, CancellationToken.None), bufferSize)
        {
        }

        ProgressStreamContent(ProgressStream stream)
            : base(stream)
        {
            Initialize(stream);
        }

        ProgressStreamContent(ProgressStream stream, int bufferSize)
            : base(stream, bufferSize)
        {
            Initialize(stream);
        }

        #endregion

        #region Methods

        private void Initialize(ProgressStream stream)
        {
            stream.ReadCallback = ReadBytes;

            Progress = delegate { };
        }

        private void Reset()
        {
            _totalBytes = 0L;
        }

        private void ReadBytes(long bytes)
        {
            if (_totalBytesExpected == -1)
            {
                _totalBytesExpected = Headers.ContentLength ?? -1;
            }

            long computedLength;

            if (_totalBytesExpected == -1 && TryComputeLength(out computedLength))
            {
                _totalBytesExpected = computedLength == 0 ? -1 : computedLength;
            }

            // If less than zero still then change to -1
            _totalBytesExpected = Math.Max(-1, _totalBytesExpected);
            _totalBytes += bytes;

            Progress(bytes, _totalBytes, _totalBytesExpected);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Reset();

            return base.SerializeToStreamAsync(stream, context);
        }

        protected override bool TryComputeLength(out long length)
        {
            bool result = base.TryComputeLength(out length);

            _totalBytesExpected = length;

            return result;
        }

        #endregion
    }
}