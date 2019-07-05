using ProtoBuf;
using System;
using System.IO;
using System.Text;

public class CmdMsg
{
    public int sType;
    public int cType;
    public byte[] body;
}


public class ProtoMan
{
    private const int headerSize = 8;



    private static byte[] ProtobufSeroalizer(ProtoBuf.IExtensible body)
    {
        using (MemoryStream m = new MemoryStream())
        {
            byte[] buffer = null;
            Serializer.Serialize(m, body);
            m.Position = 0;
            int length = (int)m.Length;
            buffer = new byte[length];
            m.Read(buffer, 0, length);
            return buffer;
        }
    }

    public static byte[] PackProtobufCmd(int stype, int ctype, IExtensible body)
    {
        int cmdLen = headerSize;
        byte[] cmdBody = null;
        if (body != null)
        {
            cmdBody = ProtobufSeroalizer(body);
            cmdLen += cmdBody.Length;
        }
        byte[] cmd = new byte[cmdLen];
        DataView.WriteUshortLe(cmd, 0, (ushort)stype);
        DataView.WriteUshortLe(cmd, 2, (ushort)ctype);
        if (cmdBody != null)
        {
            DataView.WriteBytes(cmd, headerSize, cmdBody);
        }
        return cmd;
    }

    public static byte[] PackJsonCmd(int stype, int ctype, string body)
    {
        int cmdLen = headerSize;
        byte[] cmdBody = null;
        if (body != null)
        {
            cmdBody = Encoding.UTF8.GetBytes(body);
            cmdLen += cmdBody.Length;
        }
        byte[] cmd = new byte[cmdLen];
        DataView.WriteUshortLe(cmd, 0, (ushort)stype);
        DataView.WriteUshortLe(cmd, 0, (ushort)ctype);
        if (cmdBody != null)
        {
            DataView.WriteBytes(cmd, headerSize, cmdBody);
        }



        return cmd;
    }



    public static bool UnpackMsgCmd(byte[] data, int start, int len, out CmdMsg msg)
    {

        msg = new CmdMsg();
        msg.sType = DataView.ReadUintLe(data, 2, 2);
        msg.cType = DataView.ReadUintLe(data, 4, 2);

        int bodyLen = len - headerSize-2;
        msg.body = new byte[bodyLen];
        Array.Copy(data, start+headerSize, msg.body, 0, bodyLen);

        return true;

    }

    public static T ProtobufDeserialize<T>(byte[] _data)
    {
        using (MemoryStream m = new MemoryStream(_data))
        {
            return Serializer.Deserialize<T>(m);
        }
    }



}
