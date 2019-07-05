using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using SGF.Base;

public class NetWorkManagement : UnitySingleton<NetWorkManagement>
{
    private string IP = "127.0.0.1";

    private int port=8001;
    private const int recvLen = 8192;
    private Socket clientSocket = null;
    public bool isConnect = false;
    private Thread recvThread = null;
    private byte[] recvBuffer = new byte[recvLen];
    private int pkgSize =0;
    private byte[] rawBuffer=null;

    public delegate void NetMsgHandler (CmdMsg msg);
    private Queue<CmdMsg> eventQueue=new Queue<CmdMsg>();
    private Dictionary<int,NetMsgHandler> dic= new Dictionary<int,NetMsgHandler>();

    void OnApplicationQuit()
    {
        Close();
    }

    public void Init()
    {
        if (isConnect == false)
        {
            ConnectedToServer();
        }
        
    }

    public void AddServiceListener(int stype, NetMsgHandler handler)
    {
        if (dic.ContainsKey(stype))
        {
            dic[stype] += handler;
        }
        else
        {
            dic.Add(stype, handler);
        }
    }

    public void RemoveServiceListener(int stype, NetMsgHandler handler)
    {
        if (dic.ContainsKey(stype))
        {
            dic[stype] -= handler;
            if (dic[stype] ==null)
            {
                dic.Remove(stype);
            }
        }

    }

    void Update()
    {
        while (eventQueue.Count>0)
        {
            lock (eventQueue)
            {
                CmdMsg msg = eventQueue.Dequeue();
                if (dic.ContainsKey(msg.sType))
                {
                    dic[msg.sType](msg);
                }
            }
        }
    }


    private void OnRecvClientCmd(byte[] buf,int len)
    {
        CmdMsg msg;
        ProtoMan.UnpackMsgCmd(buf, 2, len, out msg);


        if (msg!=null)
        {
            lock (eventQueue)
            {
                eventQueue.Enqueue(msg);
            }
        }
    }


    private void RecvData(int len, byte[] buf)
    {

        if (TcpPacker.ReadHeader(buf, len, out pkgSize))
        {
            if (pkgSize>len)
            {
                rawBuffer = new byte[pkgSize];
                Array.Copy(buf, 0,rawBuffer,0, len);
                clientSocket.Receive(rawBuffer,len,pkgSize-len, SocketFlags.None);
                OnRecvClientCmd(rawBuffer, pkgSize);


                pkgSize = 0;
                recvBuffer = null;
                rawBuffer = null;
                return;
            }

            OnRecvClientCmd(buf, pkgSize);

            len -= pkgSize;
            int recved = 0;
            while (len>0)
            {
                recved += pkgSize;
                pkgSize = DataView.ReadUintLe(buf, pkgSize, 2);
                rawBuffer = new byte[pkgSize];
                Array.Copy(buf, recved, rawBuffer, 0, pkgSize);
                OnRecvClientCmd(rawBuffer, pkgSize);
                len -= pkgSize;

            }
        }


    }

    public void ConnectedToServer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(IP);
            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);
            IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback((IAsyncResult ar)=> {
                try
                {
                    Socket client = (Socket)ar.AsyncState;
                    client.EndConnect(ar);
                    isConnect = true;
                    ModuleManager.Instance.Invoke("ConnectSuccess", "Success to connect server");

                    //string port2 = ((IPEndPoint)clientSocket.LocalEndPoint).Port.ToString();
                    recvThread = new Thread(new ThreadStart(() => {
                        if (isConnect == false)
                        {
                            return;
                        }
                        while (true)
                        {
                            if (!clientSocket.Connected)
                            {
                                break;
                            }
                            try
                            {
                                int recvLen = clientSocket.Receive(recvBuffer);
                                if (recvLen > 0)
                                {
                                    
                                    RecvData(recvLen, recvBuffer);
                                }
                            }
                            catch
                            {
                                clientSocket.Disconnect(true);
                                clientSocket.Shutdown(SocketShutdown.Both);
                                clientSocket.Close();
                                isConnect = false;
                                ModuleManager.Instance.Invoke("Log", "Failed to connect server");
                                break;
                            }
                        }
                    }));
                    recvThread.IsBackground = true;
                    recvThread.Start();
                }
                catch 
                {
                    isConnect = false;
                    ModuleManager.Instance.Invoke("Log", "Failed to connect server");
                }
            }), clientSocket);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
            }
        }
        catch
        {
            isConnect = false;
            ModuleManager.Instance.Invoke("Log", "Failed to connect server");
        }
    }

    void Close()
    {

        if (!isConnect)
        {
            return;
        }
        if (recvThread!=null)
        {
            recvThread.Abort();
        }

        if (clientSocket!= null && clientSocket.Connected)
        {
            ModuleManager.Instance.Invoke("Log", "DisConnect to connect server");
            clientSocket.Close();
        }
        
    }

    public void SendProtobufCmd(int stype, int ctype, ProtoBuf.IExtensible body)
    {
        byte[] cmdData = ProtoMan.PackProtobufCmd(stype, ctype, body);
        if (cmdData==null)
        {
            return;
        }
        byte[] tcpPkg = TcpPacker.Pack(cmdData);

        clientSocket.BeginSend(tcpPkg, 0, tcpPkg.Length, SocketFlags.None, new AsyncCallback((IAsyncResult ir) =>
            {
                try
                {
                    Socket client =(Socket)ir.AsyncState;
                }
                catch
                {
                    ModuleManager.Instance.Invoke("Log", "Failed to Send Cmd");
                }
            }),clientSocket);

    }

    void SendJsonCmd(int stype, int ctype, string jsonBody)
    {
        byte[] cmdData = ProtoMan.PackJsonCmd(stype, ctype, jsonBody);
        if (cmdData == null)
        {
            return;
        }
        byte[] tcpPkg = TcpPacker.Pack(cmdData);
         
        clientSocket.BeginSend(tcpPkg, 0, tcpPkg.Length, SocketFlags.None, new AsyncCallback((IAsyncResult ir) =>
            {
                try
                {
                    Socket client = (Socket)ir.AsyncState;
                }
                catch 
                {

                }
            }), clientSocket);
    }
}
