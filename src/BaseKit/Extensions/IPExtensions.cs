using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace BaseKit.Extensions
{
    /// <summary>متدهای extension برای کار با <see cref="IPAddress"/>.</summary>
    public static class IPExtensions
    {
        /// <summary>پینگ کردن آدرس IP با timeout سه ثانیه؛ در صورت شکست (از جمله PingException) false برمی‌گرداند.</summary>
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
