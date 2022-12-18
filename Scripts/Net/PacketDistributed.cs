using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;


public abstract class PacketDistributed
{
    public MessageID GetPacketID() { return packetID; }
    protected MessageID packetID;
    public static PacketDistributed CreatePacket(MessageID packetID)
    {
        PacketDistributed packet = null;
        switch (packetID)
        {

        }
        if (null != packet)
        {
            packet.packetID = packetID;
        }
        return packet;
    }
    public void SendPacket()
    {
        NetWorkLogic.GetMe().SendPacket(this);
    }

    public PacketDistributed ParseFrom(byte[] data, int nLen)
    {
        CodedInputStream input = CodedInputStream.CreateInstance(data, 0, nLen);
        PacketDistributed inst = MergeFrom(input, this);
        input.CheckLastTagWas(0);
        return inst;
    }

    public abstract int SerializedSize();
    public abstract void WriteTo(CodedOutputStream data);
    public abstract PacketDistributed MergeFrom(CodedInputStream input, PacketDistributed _Inst);
    public abstract bool IsInitialized();

    
}

