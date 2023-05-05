using UnityEngine;

/// <summary>
/// ����Ƽ UI�� �����ϱ� ���� MonoBehaviour�� ���
/// </summary>
public class Screen : MonoBehaviour, IState
{
    /* ==================== Variables ==================== */

    [SerializeField] private Screen[] _screens = null;
    private byte _screenMoveTo = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// �޴� ȭ�� �̵� ��ư
    /// </summary>
    public virtual void BtnMoveScreen(int index)
    {
        _screenMoveTo = (byte)index;
        ChangeState();
    }


    public virtual void Execute()
    {
        // �޴� ȭ�� Ȱ��ȭ
        gameObject.SetActive(true);
    }


    public void ChangeState()
    {
        // ���� �޴�ȭ�� ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ���� ���·� ����
        _screens[_screenMoveTo].Execute();
    }



    /* ==================== Private Methods ==================== */
}
