using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.IO;
using System;
using TMPro;
using IMFINE.Net.TCPManager;
using IMFINE.Utils;
using IMFINE.Utils.ConfigManager;
using System.Threading.Tasks;
using System.Net;

public class Client : MonoBehaviour
{
    string clientName = "Wall";
    bool socketReady;
    bool isInitialized;

    private TcpListener server;
    private TcpClient client;
    private NetworkStream stream;

    StreamWriter writer;
    StreamReader reader;

    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button ExitButton;

    public bool isPressed = false;
    public bool isExit = false;
    public bool isExited = false;

   

    void NotPressedButton()
    {
        isPressed = false;
        Button1.interactable = false;
        Button2.interactable = false;
        Button3.interactable = false;
    }


    void CanPressedButton()
    {
        isPressed = true;
        Button1.interactable = true;
        Button2.interactable = true;
        Button3.interactable = true;
    }

    public void ConnectToServer()
    {
        if (socketReady) return;

        try
        {
            stream = client.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;

            Button1.gameObject.SetActive(true);
            Button2.gameObject.SetActive(true);
            Button3.gameObject.SetActive(true);

        }
        catch (Exception e)
        {
            TraceBox.Log($"소켓에러 : {e.Message}");
        }
    }

    private bool CheckSend()
    {
        if(!isInitialized || stream == null)
        {
            Debug.Log("> " + GetType().Name + " Send Failed: Server is not initialized or client is not connected.");
        }
        return true;
    }

    async void Update()
    {
        if(TCPManager.instance.IsStartConnect)
        {
            Debug.Log("연결됐당");
            if (socketReady && stream.DataAvailable)
        {
            string data = reader.ReadLine();
            if (data != null)
            {
                Task.Run(() => OnIncomingDataAsync(data));
                ConnectToServer();
            }
                
        }

        if(isPressed && isExit) // 만약 버튼이 활성화되어있고 exit버튼도 같이 눌렀다면
        {
            isExited = true; // 어플리케이션 나가지기 true
        }

        if(!isPressed && isExit) // 만약 버튼이 비활성화되어있고 exit버튼도 같이 눌렀다면
        {
            isExited = false; // 어플리케이션 나가지기 false. -> 버튼 활성화로 되어야함
        }

        if(isExited)
        {
            Application.Quit();
            OnApplicationQuit(); // 6.28 추가
        }
        }
        
        
    }

    public async Task OnIncomingDataAsync(string sendInfo)
    {
        if (sendInfo == "%NAME")
        {
            SendAsync($"&NAME|{clientName}");
            return;
        }

        if (sendInfo == "ENABLE_BUTTONS")
        {
            CanPressedButton();
        }
        if (sendInfo == "DISABLE_BUTTONS")
        {
            NotPressedButton();
        }
        if (sendInfo.StartsWith("VIDEO_FINISHED"))
        {
            CanPressedButton();
        }

        TraceBox.Log(sendInfo);
    }

    async Task SendAsync(string data)
    {
        if (!socketReady) return;

        try
        {
            await writer.WriteLineAsync(data);
            await writer.FlushAsync();
        }
        catch (Exception ex)
        {
            // 예외 처리
            Debug.LogError("Error while sending data: " + ex.Message);
        }
    }


    void SendVideoCommand(string command)
    {
        SendAsync(command);
    }

    private async void AcceptClientAsync()
    {
        while(isInitialized)
        {
            try
            {
                client = await server.AcceptTcpClientAsync();
                stream = client.GetStream();
                Debug.Log("> " + GetType().Name + " / Client connected" + client.Client.RemoteEndPoint);
                
            }
            catch(Exception ex)
            {
                Debug.Log("> " + GetType().Name + " / Error: " + ex.Message);
            }
        }
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => ConfigManager.instance.isPrepared);
        //server = new TcpListener(IPAddress.Any, ConfigManager.instance.data.port);
        server.Start();

        isInitialized = true;
        AcceptClientAsync();

        while(true)
        {
            try
            {
                Button1.onClick.AddListener(() =>
                {
                    Debug.Log("1번");
                    TCPManager.instance.Send("Others","HEllo");
                    SendVideoCommand("VIDEO2");
                });
                Button2.onClick.AddListener(() =>
                {
                    Debug.Log("2번");
                    SendVideoCommand("VIDEO3");
                });
                Button3.onClick.AddListener(() =>
                {
                    Debug.Log("3번");
                    SendVideoCommand("VIDEO4");
                });
                ExitButton.onClick.AddListener(() =>
                {
                    isExit = true;
                    SendVideoCommand("EXIT1");
                });
                yield break;
            }
            catch
            {
                yield break;
            }
        }
    }

    public void playButton1()
    {
        
    }

    void OnDestroy()
    {
        isInitialized = false;
        server?.Stop();
        client?.Close();
    }

    private void OnApplicationQuit()
    {
        OnDestroy();
    }
}
