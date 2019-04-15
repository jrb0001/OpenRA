using System;
using System.Net;
using System.Net.Sockets;

namespace OpenRA
{
	public class NetworkUtils
	{
		public static IPAddress GetAnyBindAddress()
		{
			if (Socket.OSSupportsIPv6)
				return IPAddress.IPv6Any;
			else
				return IPAddress.Any;
		}

		public static IPAddress GetLoopbackAddress()
		{
			if (Socket.OSSupportsIPv6)
				return IPAddress.IPv6Loopback;
			else
				return IPAddress.Loopback;
		}

		public static IPEndPoint ParseEndpoint(string value)
		{
			try
			{
				Uri uri;
				if (Uri.TryCreate(value, UriKind.Absolute, out uri))
					return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port);
				else if (Uri.TryCreate(String.Concat("tcp://", value), UriKind.Absolute, out uri))
					return new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port);
				else
					return null;
			}
			catch (FormatException)
			{
				return null;
			}
		}
	}
}
