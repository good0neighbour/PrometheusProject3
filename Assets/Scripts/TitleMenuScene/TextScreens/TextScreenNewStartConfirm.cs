public class TextScreenNewStartConfirm : TextScreenBase
{
    public override void Execute()
    {
        TitleMenuManager.Instance.SetTextScreen(
        "------------------------------------------------------------\n",
        $"{Language.Instance["기존 임무를 철회하고 새로운 임무를 시작하겠습니까?"]}\n",
        "------------------------------------------------------------"
        );
    }


    public override void SetButtons()
    {
        TitleMenuManager.Instance.SetButtons(
        $">>{Language.Instance["뒤로"]}",
        $">>{Language.Instance["새로 시작"]}"
        );
    }


    public override void ButtonAct(byte index)
    {
        switch (index)
        {
            case 0:
                NextScreen = TitleMenuManager.TextScreens.Start;
                ChangeState();
                return;

            case 1:
                AudioManager.Instance.PlayAuido(AudioType.Touch);
                TitleMenuManager.Instance.PlanetScreenEnable();
                return;

            default:
                return;
        }
    }

    public override void OnEscapeBtn()
    {
        NextScreen = TitleMenuManager.TextScreens.Start;
        ChangeState();
    }
}
