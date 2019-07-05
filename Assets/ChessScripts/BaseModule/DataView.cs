using System.Collections;
using System.Collections.Generic;
using System;

public class DataView
{
    public static void WriteUshortLe(byte[] buf,int offset,ushort value)
    {
        byte[] byteValue = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(byteValue);
        }
        Array.Copy(byteValue, 0, buf, offset, byteValue.Length);

    }

    public static void WriteUintLe(byte[] buf, int offset, uint value)
    {
        byte[] byteValue = BitConverter.GetBytes(value);
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(byteValue);
        }
        Array.Copy(byteValue, 0, buf, offset,byteValue.Length);
    }

    public static void WriteBytes(byte[] buf, int offset, byte[] value)
    {
        Array.Copy(value, 0, buf, offset, value.Length);
    }

    public static int ReadUintLe(byte[] buf,int offset,int length)
    {
        byte[] temp=new byte[length];
        Array.Copy(buf, offset, temp, 0, length);
        int num = BitConverter.ToUInt16(temp,0);
        return num;
    }

}
