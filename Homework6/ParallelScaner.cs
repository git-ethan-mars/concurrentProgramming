using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using TPL;

public class ParallelScanner : IPScanner
{
    public Task Scan(IPAddress[] ipAddr, int[] ports)
    {
        var task = Task.Factory.StartNew(() =>
        {
            var checkingIpsTasks = ipAddr.Select(ip => Task.Factory.StartNew(() =>
                {
                    if (PingAddr(ip) == IPStatus.Success)
                    {
                        var checkingPortsTasks = ports.Select(port =>
                                Task.Factory.StartNew(() => CheckPort(ip, port),
                                    TaskCreationOptions.AttachedToParent))
                            .ToArray();
                    }
                }
                , TaskCreationOptions.AttachedToParent)).ToArray();
        });
        return task;
    }

    private IPStatus PingAddr(IPAddress ipAddr, int timeout = 3000)
    {
        using var ping = new Ping();

        Console.WriteLine($"Pinging {ipAddr}");
        var status = ping.SendPingAsync(ipAddr, timeout).Result.Status;
        Console.WriteLine($"Pinged {ipAddr}: {status}");

        return status;
    }

    private static void CheckPort(IPAddress ipAddr, int port, int timeout = 3000)
    {
        using var tcpClient = new TcpClient();

        Console.WriteLine($"Checking {ipAddr}:{port}");
        var portStatus = tcpClient.ConnectAsync(ipAddr, port, timeout).Result;
        Console.WriteLine($"Checked {ipAddr}:{port} - {portStatus}");
    }
}