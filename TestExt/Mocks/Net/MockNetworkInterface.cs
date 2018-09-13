using System;
using System.Threading;
using HmxLabs.Core.Net;
using HmxLabs.Core.Threading;

namespace HmxLabs.TestExt.Mocks.Net
{
    /// <summary>
    /// A mock implementation of <code>INetworkInterface</code> to allow easier testing of code that
    /// relies on <code>INetworkInterface</code>.
    /// 
    /// The send implementation is a simple Sleep(100) while the receive methods can by invoked by
    /// test code to provide any desired message
    /// </summary>
    public class MockNetworkInterface : INetworkInterface
    {
        /// <summary>
        /// See <code>INetworkInterface.MessageReceived</code>
        /// </summary>
        public event MessageReceivedAction MessageReceived;

        /// <summary>
        /// See <code>INetworkInterface.KeepAliveReceived</code>
        /// </summary>
        public event NetworkInterfaceAction KeepAliveReceived;

        /// <summary>
        /// See <code>INetworkInterface.ConnectionError</code>
        /// </summary>
        public event NetworkInterfaceErrorAction ConnectionError;

        /// <summary>
        /// See <code>INetworkInterface.ReceiveError</code>
        /// </summary>
        public event NetworkInterfaceErrorAction ReceiveError;

        /// <summary>
        /// See <code>INetworkInterface.Connected</code>
        /// </summary>
        public event ConnectionStatusAction Connected;

        /// <summary>
        /// See <code>INetworkInterface.Disconnected</code>
        /// </summary>
        public event ConnectionStatusAction Disconnected;

        /// <summary>
        /// Utility method for use by test code to verify if this mock has subscribers to the message received event.
        /// 
        /// This may be useful to validate that the system under test correctly subscribes to this event for example
        /// </summary>
        public bool HasMessageReceivedSubscribers => null != MessageReceived;

        /// <summary>
        /// Utility method for test code to verify if this mock has any subscribers to the keep alive received event
        /// 
        /// This may be useful to validate that the system under test correctly subscribes to this event for example
        /// </summary>
        public bool HasKeepAliveReceivedSubscribers { get { return null != KeepAliveReceived; } }

        /// <summary>
        /// Utility methods for test code to verify if this mock has any subscribers to the ConnectionError event.
        /// This may be useful to validate that the system under test correctly subscribes to this event for example
        /// </summary>
        public bool HasConnectionErrorSubscribers { get { return null != ConnectionError; } }

        /// <summary>
        /// Utility method for test code to verify if this mock has any subscribers to the ReceiveError event.
        /// This may be useful to validate that the system under test correctly subscribes to this event for example
        /// </summary>
        public bool HasReceiveErrorSubscribers { get { return null != ReceiveError; } }

        /// <summary>
        /// Utility method for test code to verify if this mock has any subscribers to the Connected event.
        /// This may be useful to validate that the system under test correctly subscribes to this event for example
        /// </summary>
        public bool HasConnectedSubscribers { get { return null != Connected; } }

        /// <summary>
        /// Utilty method for test code to verify if this ock has any subscribers to the Disconnected event.
        /// This may be useful to validate that the system under test correctly subscribes to this event for example
        /// </summary>
        public bool HasDisconnectedSubscribers { get { return null != Disconnected; } }

        /// <summary>
        /// No Op. Does not need to be called but requires a no op implementation to conform to INetworkInterface
        /// </summary>
        public void Dispose()
        {
            // No op. Nothing to dispose.
        }

        /// <summary>
        /// No Op implementation of INetWorkInterface.Connect
        /// </summary>
        public void Connect()
        {
            Thread.Sleep(10);
        }

        /// <summary>
        /// Async connection, does a Sleep(100) async
        /// </summary>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        /// <returns></returns>
        public IAsyncResult BeginConnect(AsyncCallback callback_, object state_)
        {
            var asyncAction = new AsyncActionSimple(() => Thread.Sleep(100)); // No op
            return asyncAction.BeginInvoke(callback_, state_);
        }

        /// <summary>
        /// End the async connect operation
        /// </summary>
        /// <param name="asyncResult_"></param>
        public void EndConnect(IAsyncResult asyncResult_)
        {
            var asyncAction = asyncResult_.AsyncState as AsyncActionSimple;
            if (null == asyncAction)
                throw new ArgumentException("Inavlid IAsyncResultProvided");

            asyncAction.EndInvoke(asyncResult_);
        }

        /// <summary>
        /// Close the connection. This is a no op.
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// Performs only a sleep(100). Nothing is done with the message data provided
        /// </summary>
        /// <param name="message_"></param>
        public void Send(byte[] message_)
        {
            Thread.Sleep(100);
        }

        /// <summary>
        /// Async connect. Behavious is a Sleep(100) asyn, again, nothing is done with the provided message data
        /// </summary>
        /// <param name="message_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        /// <returns></returns>
        public IAsyncResult BeginSend(byte[] message_, AsyncCallback callback_, object state_)
        {
            var asyncAction = new AsyncActionSimple(() => Thread.Sleep(100)); // No op
            return asyncAction.BeginInvoke(callback_, state_);
        }

        /// <summary>
        /// End the async send
        /// </summary>
        /// <param name="asyncResult_"></param>
        public void EndSend(IAsyncResult asyncResult_)
        {
            var asyncAction = asyncResult_.AsyncState as AsyncActionSimple;
            if (null == asyncAction)
                throw new ArgumentException("Inavlid IAsyncResultProvided");

            asyncAction.EndInvoke(asyncResult_);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Calling this causes the <code>MessageReceived</code> event in this mock interface to be
        /// fired, allowing any connected clients subscribing to this event to be tested.
        /// </summary>
        /// <param name="message_"></param>
        public void InvokeMessageReceived(byte[] message_)
        {
            if (null == MessageReceived)
                return;

            MessageReceived(this, message_);
        }

        /// <summary>
        /// Causes the <code>KeepAliveReceived</code> event to be fired, thus allowing any connected clients
        /// subscribing to this event to be tested
        /// </summary>
        public void InvokeKeepAliveReceived()
        {
            if (null == KeepAliveReceived)
                return;

            KeepAliveReceived(this);
        }

        /// <summary>
        /// Causes the <code>ConnectionError</code> event to be fired
        /// </summary>
        /// <param name="exception_"></param>
        public void InvokeConnectionError(Exception exception_)
        {
            if (null == ConnectionError)
                return;

            ConnectionError(this, exception_);
        }

        /// <summary>
        /// Causes the <code>ReceieveError</code> event to be fired
        /// </summary>
        /// <param name="exception_"></param>
        public void InvokeReceiveError(Exception exception_)
        {
            if (null == ReceiveError)
                return;

            ReceiveError(this, exception_);
        }

        /// <summary>
        /// Causes the <code>Connected</code> event on this interface to be fired thus allowing
        /// any connected clients subscribing to it to be tested.
        /// </summary>
        /// <param name="connected_">Whether the connected flag should be set to true or false</param>
        public void InvokeConnected(bool connected_)
        {
            if (null == Connected)
                return;

            IsConnected = true;
            Connected(this, true);
        }

        /// <summary>
        /// Causes the <code>Disconnected</code> event on this interface to be fired thus allowing
        /// any connected clients subscribing to it to be tested.
        /// </summary>
        /// <param name="connected_"></param>
        public void InvokeDisconnected(bool connected_)
        {
            if (null == Disconnected)
                return;

            IsConnected = false;
            Disconnected(this, false);
        }
    }
}
