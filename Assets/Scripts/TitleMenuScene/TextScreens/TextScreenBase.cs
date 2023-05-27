public abstract class TextScreenBase : IState
{
    protected TitleMenuManager.TextScreens NextScreen;

    public void ChangeState()
    {
        TitleMenuManager.Instance.MoveScreen(NextScreen);
    }

    public abstract void Execute();

    /// <summary>
    /// ��ư �̸� ����
    /// </summary>
    public abstract void SetButtons();

    /// <summary>
    /// ��ư ����
    /// </summary>
    public abstract void ButtonAct(byte index);
}
