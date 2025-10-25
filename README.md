# SevenVpn.cs
Mobile-API for [7 VPN](https://play.google.com/store/apps/details?id=com.sevenvpn) which is an reliable and easy-to-use VPN application that designed to ensure your security and privacy online anywhere and anytime

## Example
```cs
using System;
using SevenVpnApi;

namespace Application
{
    internal class Program
    {
        static async Task Main()
        {
            var api = new SevenVpn();
            string servers = await api.GetServers();
            Console.WriteLine(servers);
        }
    }
}
```
