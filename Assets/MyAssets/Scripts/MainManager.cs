using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IMFINE.Utils;
using IMFINE.Utils.ConfigManager;
using IMFINE.Net.TCPManager;
using TMPro;

public class MainManager : MonoSingleton<MainManager>
{
    public enum CONTENT_MODE
    {
        HOME,
        REMOTE,
        Reset,
        Skip,
        AutoPlay,
        MoveToTransitionStart
    }

    public enum CONTENT_PAGE
    {
        IDLE,
        VIDEO1,
        VIDEO2,
        VIDEO3
    }
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private GameObject popupPanel;
    private CONTENT_MODE currentContentMode;
    private CONTENT_PAGE currentPage;
    public CONTENT_MODE CurrentContentMode {get => currentContentMode;}

    public delegate void changingStatusDelegate(bool isChanging, string mCmd);

    public delegate void contentModeDelegate(CONTENT_MODE mode);

    public delegate void contentPageDelegate(CONTENT_PAGE page);
    public delegate void popupDelegate(string message, string argument);
     public event popupDelegate PopupOK;

    public event contentModeDelegate ContentModeChanged;
    public event contentPageDelegate ContentPageChanged;

    public event changingStatusDelegate ChangingStatusChanged;

    public Button leftBtn;
    public Button rightBtn; 
    //public GameObject homePanel;

    [SerializeField] private GameObject leftPanel;
    public GameObject rightPanel;

    public bool isLeft;

    private void Awake()
    {
        TCPManager.instance.MessageReceived += OnMessageReceived;
    }

    public void PressBtn()
    {
        if(isLeft)
        {
            // leftPanel.SetActive(true);
            // homePanel.SetActive(false);
            leftBtn.interactable = false;
            rightBtn.interactable = false;

        }
        else
        {
            // rightPanel.SetActive(true);
            // homePanel.SetActive(false);
            leftBtn.interactable = false;
            rightBtn.interactable = false;
        }
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) ChangePage(CONTENT_PAGE.IDLE);
    }
    private string sendMessage, sendArgument;
    public void ShowPopup(string text, string message, string argument = "")
    {
        popupText.text = text;
        sendMessage = message;
        sendArgument = argument;
        popupPanel.SetActive(true);
    }
    public void OnClickPopupOKButton()
    {
        Send(sendMessage, sendArgument);
        PopupOK?.Invoke(sendMessage, sendArgument);
        popupPanel.SetActive(false);
        //homePanel.SetActive(true);
        leftPanel.SetActive(false);
        rightPanel.SetActive(false);
    }

    public void OnClickPopupCancelButton()
    {
        popupPanel.SetActive(false);
    }

    public void Send(string message, string argument = "")
    {
        TCPManager.instance.Send("ALL", message, argument);
    }

    private void OnMessageReceived(string receiverid, string message, string argument, string senderid)
    {
        switch (message.ToUpper())
        {
            case "CHANGE_PAGE":
                ChangePage((CONTENT_PAGE)Enum.Parse(typeof(CONTENT_PAGE), argument.ToUpper()));
                break;
            case "START_CHANGING_STATUS":
                ChangingStatusChanged?.Invoke(true, argument);
                break;
            case "FINISH_CHANGING_STATUS":
                ChangingStatusChanged?.Invoke(false, argument);
                break;
        }
    }

    public void ChangeContentMode(CONTENT_MODE mode)
    {
        if(currentContentMode == mode) return;
        currentContentMode = mode;
        ContentModeChanged?.Invoke(currentContentMode);
    }

    private void ChangePage(CONTENT_PAGE page)
    {
        if(currentPage == page) return;
        currentPage = page;
        ContentPageChanged?.Invoke(currentPage);
    }
}
