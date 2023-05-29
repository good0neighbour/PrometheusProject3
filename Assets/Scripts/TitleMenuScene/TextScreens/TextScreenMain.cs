using UnityEngine;

public class TextScreenMain : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
            "------------------------------------------------------------\n",
            $"{Language.Instance["주 메뉴"]}\n",
            "------------------------------------------------------------"
            );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
            $">>{Language.Instance["종료"]}",
            $">>{Language.Instance["설정"]}",
            $">>{Language.Instance["시작"]}"
            );
    }


    public override void ButtonAct(byte index)
    {
        switch (index)
        {
            case 0:
                AudioManager.Instance.PlayAuido(AudioType.Select);
                GameManager.Instance.SaveSettings();
                Application.Quit();
                return;

            case 1:
                NextScreen = TitleMenuManager.TextScreens.Settings;
                ChangeState();
                return;

            case 2:
                NextScreen = TitleMenuManager.TextScreens.Start;
                ChangeState();
                return;

            default:
                return;
        }
    }

    public override void OnEscapeBtn()
    {
        AudioManager.Instance.PlayAuido(AudioType.Select);
        GameManager.Instance.SaveSettings();
        Application.Quit();
    }
}
