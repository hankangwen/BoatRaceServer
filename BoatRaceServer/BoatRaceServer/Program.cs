using System;
using System.Collections;
using System.Collections.Generic;
using BoatRaceServer.Net;
using BoatRaceServer.Tools;

namespace BoatRaceServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConfigData configData = GetConfigData();
            InitDebugger(configData);
            
            EchoServer server = NetWorkManager.Instance.CreateServer(configData.ip, configData.port);
            server.Start();
        }
        
        static ConfigData GetConfigData()
        {
            DataManager dataMgr = DataManager.Instance;
            ReflectionManager reflectionMgr = ReflectionManager.Instance;
            List<string> config = dataMgr.ReadTextAsset("config");
            ConfigData configData = new ConfigData();
            foreach (var line in config)
            {
                string[] result = line.Split(':');
                string fieldName = dataMgr.ToLower(result[0]);
                string value = result[1];
                if (!reflectionMgr.SetPublicValue(fieldName, value, configData))
                    Debug.LogError($"Cannot find {fieldName} in class {typeof(ConfigData)}");
            }
            return configData;
        }
        
        static void InitDebugger(ConfigData configData)
        {
            // 设置Debug输出哪些等级的信息
            Debug.enableLog = configData.debug_log;
            Debug.enableWarning = configData.debug_warning;
            Debug.enableError = configData.debug_error;
        }
    }
}