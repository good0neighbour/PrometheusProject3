using UnityEngine;

public class TextScreenMain : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
            "------------------------------------------------------------\n",
            "주메뉴\n",
            "------------------------------------------------------------"
            );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
            $">>{Language.Instance["종료"]}",
            $">>{Language.Instance["시작"]}"
            );
    }


    public override void ButtonAct(byte index)
    {
        switch (index)
        {
            case 0:
                Application.Quit();
                return;
            case 1:
                NextScreen = TitleMenuManager.TextScreens.Start;
                ChangeState();
                return;
            default:
                return;
        }
    }
}
