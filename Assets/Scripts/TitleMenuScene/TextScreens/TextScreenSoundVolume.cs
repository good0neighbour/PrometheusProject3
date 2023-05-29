public class TextScreenSoundVolume : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
            "------------------------------------------------------------\n",
            $"{Language.Instance["음량"]}\n",
            $"{Language.Instance["현재 음량"]} {(GameManager.Instance.SoundVolume * 100.0f).ToString("0")}%\n",
            "------------------------------------------------------------"
            );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
            ">>0%",
            ">>25%",
            ">>50%",
            ">>75%",
            ">>100%"
            );
    }


    public override void ButtonAct(byte index)
    {
        switch (index)
        {
            case 0:
                GameManager.Instance.SoundVolume = 0.0f;
                break;

            case 1:
                GameManager.Instance.SoundVolume = 0.25f;
                break;

            case 2:
                GameManager.Instance.SoundVolume = 0.5f;
                break;

            case 3:
                GameManager.Instance.SoundVolume = 0.75f;
                break;

            case 4:
                GameManager.Instance.SoundVolume = 1.0f;
                break;

            default:
                return;
        }

        AudioManager.Instance.OnSoundVolumeChanged();
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
