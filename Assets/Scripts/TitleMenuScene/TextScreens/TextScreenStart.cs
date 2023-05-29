public class TextScreenStart : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
        "------------------------------------------------------------\n",
        $"{Language.Instance["시작"]}\n",
        "------------------------------------------------------------"
        );
    }


    public override void SetButtons()
    {
        if (GameManager.Instance.IsThereSavedGame)
        {
            TitleMenuManager.Instance.SetButtons(
            $">>{Language.Instance["뒤로"]}",
            $">>{Language.Instance["새로 시작"]}",
            $">>{Language.Instance["불러오기"]}"
            );
        }
        else
        {
            TitleMenuManager.Instance.SetButtons(
            $">>{Language.Instance["뒤로"]}",
            $">>{Language.Instance["새로 시작"]}"
            );
        }
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
                AudioManager.Instance.PlayAuido(AudioType.Touch);
                if (GameManager.Instance.IsThereSavedGame)
                {
                    NextScreen = TitleMenuManager.TextScreens.NewStartConfirm;
                    ChangeState();
                }
                else
                {
                    TitleMenuManager.Instance.PlanetScreenEnable();
                }
                return;

            case 2:
                if (GameManager.Instance.IsThereSavedGame)
                {
                    AudioManager.Instance.PlayAuido(AudioType.Select);
                    AudioManager.Instance.FadeOutThemeMusic();
                    GameManager.Instance.IsNewGame = false;
                    TitleMenuManager.Instance.GameStart();
                }
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
