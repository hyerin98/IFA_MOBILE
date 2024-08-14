using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class IPageScript : MonoBehaviour
{
    [SerializeField] MainManager.CONTENT_MODE myPage;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Hide();
        MainManager.instance.ContentModeChanged += ChangeContentMode;
        MainManager.instance.PopupOK += OnPopupOK;
    }

    protected virtual void OnPopupOK(string message, string argument)
    {
    }

    private void ChangeContentMode(MainManager.CONTENT_MODE page)
    {
        if (myPage == page) Show();
        else Hide();
    }

    private bool isShow;

    private void Show()
    {
        if (isShow) return;
        Init();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        isShow = true;
    }

    protected virtual void Init()
    {
    }

    private void Hide()
    {
        if (!isShow) return;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = true;
        isShow = false;
    }
}