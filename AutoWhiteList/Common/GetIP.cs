using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWhiteList.Common
{
    class GetIP
    {
        public static string getIpInternet()
        {
            try
            {
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    string ip = client.DownloadString("http://ipinfo.io/ip");
                    ip = ip.Replace("\r", "").Replace("\n", "");
                    return ip;
                }
            }
            catch
            {
                return "127.0.0.1";
            }
        }
    }
}
