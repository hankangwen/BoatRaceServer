using System;
using BoatRaceServer.Net;
using BoatRaceServer.Tools;

namespace BoatRaceServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] ip_port = InitConfig();
            EchoServer server = NetWorkManager.Instance.CreateServer(ip_port[0], int.Parse(ip_port[1]));
            server.Start();
            
        }

        static string[] InitConfig()
        {
            ConfigData configData = DataManager.Instance.GetConfigData();
            Debug.enableLog = configData.debug_log;
            Debug.enableWarning = configData.debug_warning;
            Debug.enableError = configData.debug_error;
            string[] ip_port = {configData.ip, configData.port.ToString()};
            return ip_port;
        }
    }
}