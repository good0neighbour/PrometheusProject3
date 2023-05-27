public abstract class TextScreenBase : IState
{
    protected TitleMenuManager.TextScreens NextScreen;

    public void ChangeState()
    {
        TitleMenuManager.Instance.MoveScreen(NextScreen);
    }

    public abstract void Execute();

    /// <summary>
    /// 버튼 이름 설정
    /// </summary>
    public abstract void SetButtons();

    /// <summary>
    /// 버튼 동작
    /// </summary>
    public abstract void ButtonAct(byte index);
}
