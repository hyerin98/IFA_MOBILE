using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DG.Tweening;
using IMFINE.Debugs;
using IMFINE.Utils;
using IMFINE.Utils.ConfigManager;
using IMFINE.Net.TCPManager;
using System;
using TMPro;
using UnityEngine.UI;

public class ControllerManager : MonoSingleton<ControllerManager>
{
    private string _currentSelectType = "idle";

    public string currentSelectType
    {
        get { return _currentSelectType; }
    }

    private string _preSelectType = "idle";
    public string preSelectType
    {
        get { return _preSelectType; }
    }

    private bool _isReceived;
    public bool isReceived
    {
        get { return _isReceived; }
    }

    private string sendInfo;

    private void Awake()
    {
        TCPManager.instance.MessageReceived += ReceiveMessage;
    }

    private void ReceiveMessage(string receiverId, string message, string argument, string senderId)
    {
        string[] arguments = argument.Split(',');

        switch (message.ToUpper())
        {
            case "FINISH_VIDEO":
                OnFinishVideo();
                break;
        }
    }

    public void OnFinishVideo()
    {
        _isReceived = true;
        _currentSelectType = "idle";
        _preSelectType = "idle";
    }

    public void SetReceivedState(bool isOn)
    {
        _isReceived = isOn;
    }

    public void SendVideoPlayMessages()
    {
        if (sendInfo == "idle")
        {
            TCPManager.instance.Send("Others", "START_IDLE");
        }
        
        _currentSelectType = sendInfo;
        _preSelectType = sendInfo;
    }

   
}
