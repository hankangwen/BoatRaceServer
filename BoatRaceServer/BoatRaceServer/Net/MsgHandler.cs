using BoatRace;

namespace BoatRaceServer.Net
{
    public class MsgHandler : Singleton<MsgHandler>
    {
        MsgHandler()
        {
            NetWorkManager netWorkManager = NetWorkManager.Instance;
            netWorkManager.RegisterFuncName("MsgLogin", MsgLogin);
        }

        void MsgLogin(byte[] array)
        {
            
        }
    }
}