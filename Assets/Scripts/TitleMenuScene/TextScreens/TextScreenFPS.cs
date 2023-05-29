using UnityEngine;

public class TextScreenFPS : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
            "------------------------------------------------------------\n",
            $"{Language.Instance["초당 프레임 수"]}\n",
            $"{Language.Instance["현재 초당 프레임 수"]} {GameManager.Instance.TargetFrameRate.ToString()}\n",
            "------------------------------------------------------------"
            );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
            $">>30",
            $">>45",
            $">>60"
            );
    }


    public override void ButtonAct(byte index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.TargetFrameRate = 30;
                Application.targetFrameRate = 30;
                break;

            case 1:
                GameManager.Instance.TargetFrameRate = 45;
                Application.targetFrameRate = 45;
                break;

            case 2:
                GameManager.Instance.TargetFrameRate = 60;
                Application.targetFrameRate = 60;
                break;

            default:
                return;
        }

        NextScreen = TitleMenuManager.TextScreens.Settings;
        ChangeState();
        GameManager.Instance.SaveSettings();
    }

    public override void OnEscapeBtn()
    {
        NextScreen = TitleMenuManager.TextScreens.Settings;
        ChangeState();
    }
}
