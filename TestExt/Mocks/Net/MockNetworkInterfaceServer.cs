using System.Collections.Generic;
using HmxLabs.Core.Net;

namespace HmxLabs.TestExt.Mocks.Net
{
    /// <summary>
    /// A mock implementation of <code>INetworkInterfaceServer</code> to allow easy testing of code
    /// that uses this interface
    /// </summary>
    public class MockNetworkInterfaceServer : INetworkInterfaceServer
    {
        /// <summary>
        /// See <code>INetworkInterfaceServer.ClientConnected</code>
        /// </summary>
        public event ClientChangeAction ClientConnected;

        /// <summary>
        /// See <code>INetworkInterfaceServer.ClientDisconnected</code>
        /// </summary>
        public event ClientChangeAction ClientDisconnected;

        /// <summary>
        /// See <code>INetworkInterfaceServer.ClientConnectionError</code>
        /// </summary>
        public event NetworkInterfaceErrorAction ClientConnectionError;

        /// <summary>
        /// See <code>INetworkInterfaceServer.ClientReceiveError</code>
        /// </summary>
        public event NetworkInterfaceErrorAction ClientReceiveError;

        /// <summary>
        /// See <code>INetworkInterfaceServer.ServerError</code>
        /// </summary>
        public event NetworkInterfaceServerErrorAction ServerError;

        /// <summary>
        /// See <code>INetworkInterfaceServer.MessageReceived</code>
        /// </summary>
        public event MessageReceivedAction MessageReceived;

        /// <summary>
        /// See <code>INetworkInterfaceServer.KeepAliveReceived</code>
        /// </summary>
        public event NetworkInterfaceAction KeepAliveReceived;

        /// <summary>
        /// This is a mock object and this method is a no op.
        /// </summary>
        public void Dispose()
        {
            // No Op. nothing to actually dispose
        }

        /// <summary>
        /// See <code>INetworkInterfaceServer.Start</code>
        /// </summary>
        public void Start()
        {

        }
        
        /// <summary>
        /// See <code>INetworkInterfaceServer.Stop</code>
        /// </summary>
        public void Stop()
        {

        }

        /// <summary>
        /// Read only property that returns true if the ClientConnected event has any subscribers
        /// </summary>
        public bool HasSubscribersToClientConnected => null != ClientConnected;

        /// <summary>
        /// Read only property that returns true if the ClientDisconnected event has any subscribers
        /// </summary>
        public bool HasSubscribersToClientDisconnected => null != ClientDisconnected;

        /// <summary>
        /// Read only property that returns true if the ClientConnectionError event has any subscribers
        /// </summary>
        public bool HasSubscribersToClientConnectionError => null != ClientConnectionError;

        /// <summary>
        /// Read only property that returns true if the ClientReceiveError event has any subscribers
        /// </summary>
        public bool HasSubscribersToClientReceiveErorr => null != ClientReceiveError;

        /// <summary>
        /// Read only property that returns true if the ServerError event has any subscribers
        /// </summary>
        public bool HasSubscribersToServerError => null != ServerError;

        /// <summary>
        /// Read only property that returns true if the MessageReceived event has any subscribers
        /// </summary>
        public bool HasSubscibersToMessageReceived => null != MessageReceived;

        /// <summary>
        /// Read only property that returns true if the KeepAliveReceived event has any subscribers
        /// </summary>
        public bool HasSubscribersToKeepAliveReceived { get { return null != KeepAliveReceived; } }

        /// <summary>
        /// Causes the ClientConnected event to be fired
        /// </summary>
        /// <param name="client_">The client connection object to pass as the argument when the event fires</param>
        public void InvokeClientConnected(INetworkInterface client_)
        {
            if (null == ClientConnected)
                return;

            ClientConnected(this, client_);
        }

        /// <summary>
        /// Read only property providing a List of all the network interfaces clients this server has connected
        /// </summary>
        public List<INetworkInterface> ClientList => _clients;

        /// <summary>
        /// Read only property providing an enumeration of the clients connected to this server
        /// </summary>
        public IEnumerable<INetworkInterface> Clients => _clients;

        private readonly List<INetworkInterface> _clients = new List<INetworkInterface>();
    }
}
