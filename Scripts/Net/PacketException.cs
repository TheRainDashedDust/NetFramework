using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PacketException: IOException
{
    internal PacketException(string message)
            : base(message)
    {
    }

    internal static PacketException PacketReadError(string msg)
    {
        return new PacketException("PacketException ReadError:" + msg);
    }

    internal static PacketException PacketExecuteError(string msg)
    {
        return new PacketException("PacketException ExecuteError:" + msg);
    }

    internal static PacketException PacketCreateError(string msg)
    {
        return new PacketException("PacketException CreateError:" + msg);
    }
}

