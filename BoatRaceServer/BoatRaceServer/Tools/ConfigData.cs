namespace BoatRaceServer.Tools
{
    public class ConfigData
    {
        public string ip = "127.0.0.1";        // ip
        public int port = 8888;                // port
        public bool connect_db = true;         // connect db
        public bool debug_log = true;          // enable log
        public bool debug_warning = true;      // enable warning
        public bool debug_error = true;        // enable error
        public bool develop_mode = true;       // develop or release
        public string develop_db = "123";      // develop db
        public string release_db = "123";      // release db

        public int[] GetIntArray(int a, string b)
        {
            int[] result = {a, int.Parse(b)};
            return result;
        }
    }
}