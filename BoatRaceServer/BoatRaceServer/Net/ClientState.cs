using System.Net.Sockets;
using System.Text;

namespace BoatRaceServer.Net
{
    public class ClientState
    {
        public Socket socket;
        public byte[] readBuff = new byte[1024];

        public ClientState() { }
        public ClientState(Socket _socket) { socket = _socket; }

        public string GetDesc()
        {
            return socket.RemoteEndPoint.ToString();
        }

        public void Send(string sendStr)
        {
            byte[] sendBytes = Encoding.Default.GetBytes(sendStr);
            Send(sendBytes);
        }

        public void Send(byte[] sendBytes)
        {
            socket.Send(sendBytes);
        }
    }
}