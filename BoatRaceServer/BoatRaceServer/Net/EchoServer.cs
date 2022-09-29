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
            // IPAddress ipAdr = IPAddress.Parse(_ip);
            // IPEndPoint ipEp = new IPEndPoint(ipAdr, this._port);
            // listenfd.Bind(ipEp);
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

            
            // // Test:转发给所有客户端
            // byte[] sendBytes = new byte[count];
            // Array.Copy(state.readBuff, 0, sendBytes, 0, count);
            //
            //
            // var obj = ProtobufUtil.Instance.BytesToObject<Hero>(sendBytes);
            // Console.WriteLine(obj.info.name);
            //
            // string str = Encoding.Default.GetString(sendBytes);
            // Console.WriteLine("Receive, client is {0}, msg = {1}", state.GetDesc(), str);
            // foreach (var client in clients.Values)
            // {
            //     client.Send(sendBytes);
            // }

            return true;
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