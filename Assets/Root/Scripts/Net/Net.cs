using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Yoziya
{
    public class Net : MonoBehaviour
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread receiveThread;

        void Start()
        {
            // 连接到游戏服务器
            client = new TcpClient("192.168.1.5", 8080);
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            // 开启一个新的线程接收服务器消息
            receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
            SendMessageToServer("000");
        }

        void OnDestroy()
        {
            // 断开连接
            if (client != null)
            {
                client.Close();
                client = null;
            }

            // 停止接收线程
            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }
        }

        private void ReceiveMessages()
        {
            while (client != null && client.Connected)
            {
                try
                {
                    // 读取服务器消息
                    string message = reader.ReadLine();
                    if (message != null)
                    {
                        Debug.Log("Received message: " + message);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error receiving message: " + e.Message);
                }
            }
        }

        // 示例：发送消息到服务器
        public void SendMessageToServer(string message)
        {
            if (client != null && client.Connected)
            {
                try
                {
                    writer.WriteLine(message);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error sending message: " + e.Message);
                }
            }
        }
    }
}
