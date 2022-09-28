using System;
using BoatRaceServer.Tools;

namespace BoatRaceServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConfigData configData = DataManager.Instance.GetConfigData();
            Console.WriteLine(configData.ip);
            Console.WriteLine(configData.port);
            Console.WriteLine(configData.connect_db);
            Console.WriteLine(configData.debug_log);
            Console.WriteLine(configData.debug_warning);
            Console.WriteLine(configData.debug_error);
            
        }
        
    }
}