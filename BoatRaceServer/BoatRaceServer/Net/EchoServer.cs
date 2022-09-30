using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BoatRace;
using BoatRaceServer.Tools;

namespace BoatRaceServer.Net
{
    public class EchoServer
    {
        private string _ip;
        private int _port;
        public EchoServer(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }

        Socket listenfd = null;
        ObjectPool<ClientState> clientStatePool = new ObjectPool<ClientState>();
        Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

        public void Start()
        {
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // listenfd.Bind(new IPEndPoint(IPAddress.Parse(_ip), _port));
            listenfd.Bind(new IPEndPoint(IPAddress.Any, _port));
            listenfd.Listen(0);
            Debug.Log($"Server started, ip = {this._ip}, port = {this._port}");

            #region 多路复用
            //多路复用：同时处理多路信号
            List<Socket> checkRead = new List<Socket>() { };
            while (true) {
                checkRead.Clear();
                checkRead.Add(listenfd);
                // Foreach all client which is connected.
                foreach (var client in clients.Values)
                {
                    checkRead.Add(client.socket);
                }
                // Start select.
                Socket.Select(checkRead, null, null, 1000);
                foreach (var item in checkRead)
                {
                    if (item == listenfd)
                        ReadListenfd(item);
                    else
                        ReadClientfd(item);
                }
            }
            #endregion
        }

        #region ReadListenfd, ReadClientfd
        private void ReadListenfd(Socket listenfd)
        {
            Debug.Log("Accept Client");
            Socket clientfd = listenfd.Accept();
            ClientState state = clientStatePool.Get();
            state.socket = clientfd;
            clients.Add(clientfd, state);
        }

        private bool ReadClientfd(Socket clientfd)
        {
            ClientState state = clients[clientfd];
            // Receive byte.
            int count;
            try
            {
                count = clientfd.Receive(state.readBuff);
            }
            catch (SocketException e)
            {
                CloseClient(clientfd);
                Debug.LogError($"Client Connect Error : {e.Message}");
                return false;
            }

            // Client force close.
            if(count == 0) {
                CloseClient(clientfd);
                Debug.Log("Client force close socket.");
                return false;
            }

            // 接收端解析消息
            
            
            //byte[] bytes = state.readBuff;
            //int readIdx = 0;
            //Int16 bodyLength = (Int16)((bytes[readIdx + 1] << 8) | bytes[readIdx]);
            //readIdx += 2;
            //int nameCount = 0;
            //string protoName = MsgBase.DecodeName(bytes, readIdx, out nameCount);
            //if (protoName == "") {
            //    Console.WriteLine("Receive 协议名 failed.");
            //    return false;
            //}

            //readIdx += nameCount;
            //// 解析协议体
            //int bodyCount = bodyLength - nameCount;
            //MsgBase msgBase = MsgBase.Decode(protoName, bytes, readIdx, bodyCount);
            //readIdx += bodyCount;
            //Console.WriteLine(msgBase.protoName);

            //string funName = msgBase.protoName;

            /*
            // Deal with data.
            string receiveStr = Encoding.Default.GetString(state.readBuff, 0, count);
            Console.WriteLine("Receive : " + receiveStr);

            // Use Reflection.
            //string str = MsgBase.DecodeName()
            string[] split = receiveStr.Split('|');
            string msgName = split[0];
            string msgArgs = split[1];
            string funName = "Msg" + msgName;
            MethodInfo method = typeof(MsgHandler).GetMethod(funName);
            object[] o = { state, msgArgs };
            method.Invoke(null, o);
            */

            //string sendStr = clientfd.RemoteEndPoint.ToString() + ":" + receiveStr;
            //string sendStr = receiveStr;
            //byte[] sendBytes = Encoding.Default.GetBytes(sendStr);
            //// Sync data to all client.
            ///
            //foreach (var item in clients.Values)
            //{
            //    item.socket.Send(sendBytes);
            //}
            
            
            // byte[] sendBytes = new byte[count];
            // Array.Copy(state.readBuff, 0, sendBytes, 0, count);
            // var obj = ProtobufUtil.Instance.BytesToObject<Hero>(sendBytes);
            // Console.WriteLine(obj.info.name);
            
            return true;
        }

        // eg:Fire2AllClient(sendBytes, clients[clientfd]);
        public void Fire2AllClient(byte[] sendBytes, ClientState ignoreClient = null)
        {
            foreach (var client in clients.Values)
            {
                if(ignoreClient!= null && client == ignoreClient)
                    continue;
                client.Send(sendBytes);
            }
        }

        public void CloseClient(Socket clientfd)
        {
            var state = clients[clientfd];
            clientfd.Close();
            clients.Remove(clientfd);
            clientStatePool.Release(state);
        }
        #endregion
    }
}