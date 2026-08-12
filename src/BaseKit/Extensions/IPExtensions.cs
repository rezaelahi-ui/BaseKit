using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BaseKit.Extensions
{
    public static class IPExtensions
    {
        public static async Task<bool> Ping(this IPAddress ip)
        {
            using var ping = new Ping();
            try
            {
                var reply = await ping.SendPingAsync(ip.ToString(), 3000);
                return reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}
