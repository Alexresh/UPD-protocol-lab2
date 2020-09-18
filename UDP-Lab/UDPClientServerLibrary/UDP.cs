using System;
using System.Net.Sockets;

public class UDPClientServerLibrary
{
    public static UdpClient CreateUDPServerConnection(int port)
    {
        UdpClient udpServer = new UdpClient(port);
        try
        {
            udpServer.Connect("127.0.0.1", port);
            return udpServer;
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
