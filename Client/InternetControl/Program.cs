using System;
using System.Threading;
using NetFwTypeLib;

namespace InternetControl
{
    class Program
    {
        
        static void Main(string[] args)
        {
            using (FirewallPortBlock firewallPortBlocker = new FirewallPortBlock())
            {


                //firewallPortBlocker.TcpHttpHttpsBlock();
                //firewallPortBlocker.UdpHttpBlock();
                firewallPortBlocker.RuleRemove();
            }
        
    }
    }
}
