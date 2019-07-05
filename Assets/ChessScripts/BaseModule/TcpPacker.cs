using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TcpPacker
{
    private const int handerSize=2;

    public static byte[] Pack(byte[] cmdData)
    {
        int len = cmdData.Length;
        if (len>65535-2)
        {
            return null;
        }

        int cmdLen = len + handerSize;
        byte[] cmd = new byte[cmdLen];
        DataView.WriteUshortLe(cmd, 0, (ushort)cmdLen);
        DataView.WriteBytes(cmd, handerSize, (cmdData));

        return cmd;
    }


    public static bool ReadHeader(byte[] data, int dataLen, out int pkgSize)
    {

        if (data.Length<2)
        {
            pkgSize = -1;
            return false;
        }
        pkgSize = DataView.ReadUintLe(data, 0, 2);
        return true;
    }

    //public static byte[] UnPack(byte[] cmdData)
    //{
    //    int len = cmdData.Length;
    //    if (len > 65535 - 2)
    //    {
    //        return null;
    //    }

    //    int cmdLen = len + handerSize;
    //    byte[] cmd = new byte[cmdLen];
    //    DataView.WriteUshortLe(cmd, 0, (ushort)cmdLen);
    //    DataView.WriteBytes(cmd, handerSize, (cmdData));

    //    return cmd;
    //}
}
