using System.IO;

namespace HmxLabs.TestExt.Mocks.Net
{
    /// <summary>
    /// This class is designed to provide a dummy network stream to enable testing to socket/ TCP/ networking code more easily.
    /// Where such types of code would normally operate on a NetworkStream this class behaves to its caller as though it was a 
    /// network stream but actually is just backed by two independent memory streams for the read and write buffers. This then
    /// enables test code to inspect more easily what has been written to the stream or inject data that should be read
    /// without actually having to have a  network connection.
    /// 
    /// This class is designed to behave like a NetworkStream from the point of view of the network connection
    /// that it is passed to. All naming conventions internally are from the point of view of the network
    /// connection that is using this class
    /// 
    /// The additional methods named with "Request" and "Response" are designed to be used by the test class that is substituting
    /// a real network connection with this stream in order to read the responses and write the requests.
    /// </summary>
    public class MockNetworkStream : Stream
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public MockNetworkStream()
        {
            _recieveStream = new MemoryStream();
            _respondStream = new MemoryStream();
            _lock = new object();
        }

        /// <summary>
        /// Implemented as part of the dispose pattern, mostly for consistency as this obejct doesn't really
        /// have anything that needs disposing
        /// </summary>
        ~MockNetworkStream()
        {
            Dispose(false);
        }

        /// <summary>
        /// Override of Stram.Flush. Simply ensures that the contents of the input and output streams have been flushed
        /// </summary>
        public override void Flush()
        {
            lock (_lock)
            {
                _recieveStream.Flush();
                _respondStream.Flush();
            }
        }

        /// <summary>
        /// Seek to the specified point in the stream. As this is implemented from the client's point of view
        /// this means that it is the receive stream internally that we perform the seek operation on.
        /// </summary>
        /// <param name="offset_"></param>
        /// <param name="origin_"></param>
        /// <returns></returns>
        public override long Seek(long offset_, SeekOrigin origin_)
        {
            lock (_lock)
            {
                return _recieveStream.Seek(offset_, origin_);
            }
        }

        /// <summary>
        /// Set the length of the stream
        /// </summary>
        /// <param name="value_"></param>
        public override void SetLength(long value_)
        {
            lock (_lock)
            {
                _recieveStream.SetLength(value_);
            }
        }

        /// <summary>
        /// Read from the stream. As this is from the client perspective, this means reading from the receive stream.
        /// 
        /// See <code>Stream.Read</code> for more information
        /// </summary>
        /// <param name="buffer_"></param>
        /// <param name="offset_"></param>
        /// <param name="count_"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer_, int offset_, int count_)
        {
            lock (_lock)
            {
                return _recieveStream.Read(buffer_, offset_, count_);
            }
        }

        /// <summary>
        /// Additional method that is not part of <code>Stream</code> that allows the test
        /// code to read the data that the system under test wrote to this stream object
        /// </summary>
        /// <param name="buffer_"></param>
        /// <param name="offset_"></param>
        /// <param name="count_"></param>
        /// <returns></returns>
        public int ReadResponse(byte[] buffer_, int offset_, int count_)
        {
            lock (_lock)
            {
                return _respondStream.Read(buffer_, offset_, count_);
            }
        }

        /// <summary>
        /// Override of <code>Stream.Write</code>. As this is from the client perspective it therefore
        /// write the output to the respond stream internally.
        /// </summary>
        /// <param name="buffer_"></param>
        /// <param name="offset_"></param>
        /// <param name="count_"></param>
        public override void Write(byte[] buffer_, int offset_, int count_)
        {
            lock (_lock)
            {
                var origPos = _respondStream.Position;
                _respondStream.Seek(0, SeekOrigin.End);
                _respondStream.Write(buffer_, offset_, count_);
                _respondStream.Position = origPos;
            }
        }

        /// <summary>
        /// Additional method that is not on <code>Stream</code> that allows the test code to inject data
        /// into the stream for consumption by the system under test
        /// </summary>
        /// <param name="buffer_"></param>
        /// <param name="offset_"></param>
        /// <param name="count_"></param>
        public void WriteRequest(byte[] buffer_, int offset_, int count_)
        {
            lock (_lock)
            {
                var origPos = _recieveStream.Position;
                _recieveStream.Seek(0, SeekOrigin.End);
                _recieveStream.Write(buffer_, offset_, count_);
                _recieveStream.Position = origPos;
            }
        }

        /// <summary>
        /// Override of <code>Stream.CanRead</code>
        /// </summary>
        public override bool CanRead
        {
            get
            {
                lock (_lock)
                {
                    return _recieveStream.CanRead;
                }
            }
        }

        /// <summary>
        /// Override of <code>Stream.CanSeek</code>
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                lock (_lock)
                {
                    return _recieveStream.CanSeek;
                }
            }
        }

        /// <summary>
        /// Override of <code>Stream.CanWrite</code>
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                lock (_lock)
                {
                    return _respondStream.CanWrite;
                }
            }
        }

        /// <summary>
        /// Override of <code>Stream.Length</code>
        /// </summary>
        public override long Length
        {
            get
            {
                lock (_lock)
                {
                    return _recieveStream.Length;
                }
            }
        }

        /// <summary>
        /// Override of <code>Stream.Position</code>
        /// </summary>
        public override long Position
        {
            get
            {
                lock (_lock)
                {
                    return _recieveStream.Position;
                }
            }

            set
            {
                lock (_lock)
                {
                    _recieveStream.Position = value;
                }
            }
        }

        /// <summary>
        /// Implementation of the dispose method. Simply disposes the underyling memory streams
        /// </summary>
        /// <param name="disposing_"></param>
        protected override void Dispose(bool disposing_)
        {
            base.Dispose(disposing_);

            if (!disposing_)
                return; 

            _recieveStream.Dispose();
            _respondStream.Dispose();
        }

        private readonly MemoryStream _recieveStream;
        private readonly MemoryStream _respondStream;
        private readonly object _lock;
    }
}
