namespace BoatRaceServer.Tools
{
    public struct ConfigData
    {
        public ConfigData(string ip, int port, bool connectDb, bool debugLog, 
            bool debugWarning, bool debugError)
        {
            this.ip = ip;
            this.port = port;
            this.connect_db = connectDb;
            this.debug_log = debugLog;
            this.debug_warning = debugWarning;
            this.debug_error = debugError;
        }
        
        public string ip;
        public int port;
        public bool connect_db;
        public bool debug_log;
        public bool debug_warning;
        public bool debug_error;
    }
}