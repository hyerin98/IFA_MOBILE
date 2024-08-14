using System;
using UnityEngine;
using UnityEngine.UI;

public class ContentUI : IPageScript
{
    [SerializeField] private Button IdleButton;
    [SerializeField] private Button relocationButton;
    [SerializeField] private Button stopButton;

    private void Start()
    {
        MainManager.instance.ContentPageChanged += OncContentPageChanged;
    }


    private void OncContentPageChanged(MainManager.CONTENT_PAGE page)
    {
        SetUIState(page);
    }

    protected override void Init()
    {
        SetUIState();
    }

    private void SetUIState(MainManager.CONTENT_PAGE page = MainManager.CONTENT_PAGE.IDLE)
    {
        IdleButton.interactable = page.Equals(MainManager.CONTENT_PAGE.IDLE);

        relocationButton.interactable = page.Equals(MainManager.CONTENT_PAGE.VIDEO1) ||
                                        page.Equals(MainManager.CONTENT_PAGE.VIDEO2) ||
                                        page.Equals(MainManager.CONTENT_PAGE.VIDEO3);

        stopButton.interactable = !page.Equals(MainManager.CONTENT_PAGE.IDLE);
    }

    protected override void OnPopupOK(string message, string argument)
    {
    }

    public void OnClickHomeButton()
    {
        MainManager.instance.ShowPopup("Do you want to move Home?", "CHANGE_MODE", "HOME");
    }
}