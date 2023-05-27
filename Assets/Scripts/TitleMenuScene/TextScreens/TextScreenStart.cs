public class TextScreenStart : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
            "------------------------------------------------------------\n",
            "시작 화면\n",
            "------------------------------------------------------------"
            );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
            $">>{Language.Instance["뒤로"]}",
            $">>{Language.Instance["새로 시작"]}",
            $">>{Language.Instance["불러오기"]}"
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
                AudioManager.Instance.PlayAuido(AudioType.Touch);
                TitleMenuManager.Instance.PlanetScreenEnable();
                return;

            case 2:
                AudioManager.Instance.PlayAuido(AudioType.Select);
                GameManager.Instance.IsNewGame = false;
                TitleMenuManager.Instance.GameStart();
                return;

            default:
                return;
        }
    }
}
