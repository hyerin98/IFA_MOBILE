public class HomePageUI : IPageScript
{
    protected override void OnPopupOK(string message, string argument)
    {
        if (!message.Equals("CHANGE_MODE")) return;
        switch (argument)
        {
            case "HOME":
                MainManager.instance.ChangeContentMode(MainManager.CONTENT_MODE.HOME);
                break;
            case "REMOTE":
                MainManager.instance.ChangeContentMode(MainManager.CONTENT_MODE.REMOTE);
                break;
        }
    }
}