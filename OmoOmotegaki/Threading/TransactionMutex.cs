using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace OmoOmotegaki.Threading
{
    /// <summary>
    /// Mutexを共有するプロセス間で通信を行う。
    /// 新規プロセスをサーバー、それ以降に起動したプロセスをクライアントとする。
    /// </summary>
    internal sealed class TransactionMutex : IDisposable
    {
        public TransactionMutex(bool initiallyOwned, string name, string ipcPortName, string ipcUri)
        {
            Mutex = new Mutex(initiallyOwned, name);

            if (Mutex.WaitOne(0, false))
            {
                // 新規起動
                Server = new IpcTransServer(ipcPortName, ipcUri);
            }
            else
            {
                // 多重起動
                Client = new IpcTransClient(ipcPortName, ipcUri);
            }
        }

        public IpcTransServer Server { get; private set; }

        public IpcTransClient Client { get; private set; }

        public Mutex Mutex { get; private set; }

        public bool IsServer => !(Server is null);

        public void Dispose()
        {
            if (Mutex != null)
            {
                if (IsServer) Mutex.ReleaseMutex();

                Mutex.Close();
                Mutex = null;


                if (Server != null)
                {
                    Server.Dispose();
                    Server = null;
                }

                if (Client != null)
                {
                    Client.Dispose();
                    Client = null;
                }
            }
        }

        internal sealed class RemoteTransaction : MarshalByRefObject
        {
            public event TransactionEventHandler Transacted;

            public void Transact(TransData data)
            {
                Transacted?.Invoke(new TransactionEventArgs(data));
            }
        }


        /*
         * サーバー
         */

        internal sealed class IpcTransServer : IDisposable
        {
            private readonly IChannel _channel;

            public IpcTransServer(string ipcPortName, string ipcUri)
            {
                _channel = new IpcServerChannel(ipcPortName);

                ChannelServices.RegisterChannel(_channel, true);

                Transaction = new RemoteTransaction();

                Transaction.Transacted += Internal_Trans_OnTransact;

                RemotingServices.Marshal(Transaction, ipcUri, typeof(RemoteTransaction));
            }

            public event TransactionEventHandler Transacted;

            public RemoteTransaction Transaction { get; }

            private void Internal_Trans_OnTransact(TransactionEventArgs e)
            {
                Transacted?.Invoke(e);
            }

            public void Dispose()
            {
                ChannelServices.UnregisterChannel(_channel);
            }
        }


        /*
         * クライアント
         */

        internal sealed class IpcTransClient : IDisposable
        {
            private readonly IChannel _channel;
            private readonly RemoteTransaction _trans;

            public IpcTransClient(string ipcPortName, string ipcUri)
            {
                _channel = new IpcClientChannel();

                ChannelServices.RegisterChannel(_channel, true);

                _trans = (RemoteTransaction)Activator.GetObject(
                                                    typeof(RemoteTransaction),
                                                    $"ipc://{ipcPortName}/{ipcUri}");
            }

            public void Transact(TransData data)
            {
                _trans.Transact(data);
            }

            public void Dispose()
            {
                ChannelServices.UnregisterChannel(_channel);
            }
        }
    }

    [Serializable]
    public struct TransData : ISerializable
    {
        public int Action { get; set; }

        public TransData(int action) : this()
        {
            Action = action;
        }

        public TransData(SerializationInfo info, StreamingContext text) : this()
        {
            Action = info.GetInt32("Action");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Action", Action);
        }
    }


    public sealed class TransactionEventArgs : EventArgs
    {
        public TransData Data { get; set; }

        public TransactionEventArgs(TransData data)
        {
            Data = data;
        }
    }

    public delegate void TransactionEventHandler(TransactionEventArgs e);
}
