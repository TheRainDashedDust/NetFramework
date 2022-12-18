using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;

public interface PacketFactory
{
    MessageID GetPacketID();
}
public abstract class PacketFactoryManager
{
    public abstract bool Init();

    public Ipacket GetPacketHandler(MessageID nMID)
    {
        if (m_HandlerDic.ContainsKey(nMID))
        {
            return m_HandlerDic[nMID] as Ipacket;
        }
        return null;
    }

    public void RemovePacket(Ipacket pPacket)
    {
        if (pPacket == null) return;
    }


    protected void AddFactory(PacketFactory pFactory)
    {
        if (!m_Factories.ContainsKey(pFactory.GetPacketID()))
        {
            m_Factories.Add(pFactory.GetPacketID(), pFactory);
        }
    }

    protected void AddPacketHander(MessageID nMID, Ipacket packetHander)
    {
        if (!m_HandlerDic.ContainsKey(nMID))
        {
            m_HandlerDic.Add(nMID, packetHander);
        }
    }

    protected Hashtable m_Factories = new Hashtable();
    protected Dictionary<MessageID, Ipacket> m_HandlerDic = new Dictionary<MessageID, Ipacket>();
}

