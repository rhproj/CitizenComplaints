using System.Net;
using System.Net.NetworkInformation;

namespace Complaints_WPF.Services
{/// <summary>
/// Checks DB accessibility 
/// </summary>
    public static class ServerAccess
    {
        public static string _address = "10.40.133.12";

        public static bool TestConnection(string ip)
        {
            Ping x = new Ping();
            PingReply reply = x.Send(IPAddress.Parse(ip));

            if (reply.Status != IPStatus.Success)
                return false;

            return true;
        }
    }
}
