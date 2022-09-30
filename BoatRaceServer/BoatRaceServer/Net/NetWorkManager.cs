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

        private Dictionary<string, Action<byte[]>> _funcDict = new Dictionary<string, Action<byte[]>>();
        // RegisterFuncName()
        // UnRegisterFuncName()
        public void RegisterFuncName(string msgName, Action<byte[]> func)
        {
            _funcDict.Add(msgName, func);
        }

        public void InvokeFunc(string name, byte[] array)
        {
            _funcDict[name]?.Invoke(array);
        }
        
        void Login()
        {
            
        }
    }
}