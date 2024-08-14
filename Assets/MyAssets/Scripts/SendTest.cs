using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IMFINE.Net.TCPManager;
using IMFINE.Utils;

public class SendTest : MonoSingleton<SendTest>
{
    [SerializeField] private string videoReceiverId = "Video";

    private readonly Dictionary<string, string> playMessageMapping = new Dictionary<string, string>
    {
        { "PLAY_1", "ALL" },
        { "PLAY_2", "ALL" },
        { "PLAY_3", "ALL" },
        {"AutoPlay", "ALL"}
    };

    private void SendCommand(string receiverId, string message)
    {
        TCPManager.instance.Send(receiverId, message);
        LogMessage($"Sent {message} Command to {receiverId}");
    }

    private void LogMessage(string log)
    {
        Debug.Log($"> MobileController / {log}");
    }

    public void PlayVideo(string playMessage)
    {
        if (playMessageMapping.TryGetValue(playMessage, out string receiverId))
        {
            // 0~10 사이의 숫자를 랜덤으로 생성
            int randomValue = Random.Range(0, 11); 
            string messageWithRandomValue = $"{playMessage}_{randomValue}";

            SendCommand(receiverId, messageWithRandomValue);
        }
        else
        {
            LogMessage("Unknown play message.");
        }
    }

    public void OnAutoPlayClicked()
    {
        PlayVideo("AutoPlay");
    }

    public void OnPlay1ButtonClicked()
    {
        PlayVideo("PLAY_1");
    }

    public void OnPlay2ButtonClicked()
    {
        PlayVideo("PLAY_2");
    }

    public void OnPlay3ButtonClicked()
    {
        PlayVideo("PLAY_3");
    }

    public void PauseVideo()
    {
        SendCommand(videoReceiverId, "PAUSE");
    }

    public void StopVideo()
    {
        SendCommand(videoReceiverId, "STOP");
    }
}
