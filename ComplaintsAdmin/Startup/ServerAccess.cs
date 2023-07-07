using System.Configuration;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;

namespace ComplaintsAdmin.Startup
{/// <summary>
/// Checks DB accessibility 
/// </summary>
    internal class ServerAccess
    {
        internal static void TestServerAccess()
        {
            string adress = ConfigurationManager.AppSettings["address"];
            if (TestConnection(adress) == false)
            {
                MessageBox.Show($"Отсутствует связь с {adress}");
                Environment.Exit(0);
            }
        }

        private static bool TestConnection(string ip)
        {
            Ping x = new Ping();
            PingReply reply = x.Send(IPAddress.Parse(ip));

            if (reply.Status != IPStatus.Success)
                return false;

            return true;
        }
    }
}
