public class TextScreenSettings : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
            "------------------------------------------------------------\n",
            $"{Language.Instance["설정"]}\n",
            $"{Language.Instance["현재 초당 프레임 수"]} {GameManager.Instance.TargetFrameRate.ToString()}\n",
            $"{Language.Instance["현재 음량"]} {(GameManager.Instance.SoundVolume * 100.0f).ToString("0")}%\n",
            "------------------------------------------------------------"
            );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
            $">>{Language.Instance["뒤로"]}",
            $">>{Language.Instance["초당 프레임 수"]}",
            $">>{Language.Instance["음량"]}",
            $">>{Language.Instance["언어"]}"
            );
    }


    public override void ButtonAct(byte index)
    {
        switch (index)
        {
            case 0:
                NextScreen = TitleMenuManager.TextScreens.Main;
                ChangeState();
                return;

            case 1:
                NextScreen = TitleMenuManager.TextScreens.FPS;
                ChangeState();
                return;

            case 2:
                NextScreen = TitleMenuManager.TextScreens.SoundVolume;
                ChangeState();
                return;

            case 3:
                AudioManager.Instance.PlayAuido(AudioType.Touch);
                TitleMenuManager.Instance.LanguageScreenEnable();
                return;

            default:
                return;
        }
    }

    public override void OnEscapeBtn()
    {
        NextScreen = TitleMenuManager.TextScreens.Main;
        ChangeState();
    }
}
