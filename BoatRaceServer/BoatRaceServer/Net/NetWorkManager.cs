using System;
using System.Collections.Generic;
using BoatRace;
using BoatRaceServer.Tools;

namespace BoatRaceServer.Net
{
    public class NetWorkManager : Singleton<NetWorkManager>
    {
        NetWorkManager() { }

        private EchoServer _echoServer;

        public EchoServer CreateServer(string ip, int port)
        {
            _echoServer = new EchoServer(ip, port);
            return _echoServer;
        }

        public EchoServer GetServer()
        {
            if (_echoServer == null)
            {
                Debug.LogError("Server is null, please call method CreateServer()");
            }
            return _echoServer;
        }


    }
}