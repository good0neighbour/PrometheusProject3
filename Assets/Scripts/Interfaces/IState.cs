/// <summary>
/// ���������� ���� �������̽�
/// </summary>
public interface IState
{
    /// <summary>
    /// �� ���¿��� ���� ���� ������ ��
    /// </summary>
    public void Execute();

    /// <summary>
    /// ���¸� �ٲ۴�.
    /// </summary>
    public void ChangeState();
}
