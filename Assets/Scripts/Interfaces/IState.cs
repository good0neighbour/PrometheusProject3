/// <summary>
/// 상태패턴을 위한 인터페이스
/// </summary>
public interface IState
{
    /// <summary>
    /// 현 상태에서 가장 먼저 실행할 것
    /// </summary>
    public void Execute();

    /// <summary>
    /// 상태를 바꾼다.
    /// </summary>
    public void ChangeState();
}
