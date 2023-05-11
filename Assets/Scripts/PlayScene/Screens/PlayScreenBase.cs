using UnityEngine;

/// <summary>
/// ����Ƽ UI�� �����ϱ� ���� MoniBehaviour�� ��ӹ޴´�.
/// </summary>
public class PlayScreenBase : MonoBehaviour, IState
{
    /* ==================== Variables ==================== */

    [Header("�θ� Ŭ������ ��������")]
    [SerializeField] private Transform _menuButton = null;



    /* ==================== Public Methods ==================== */

    public void ChangeState()
    {
        // �� ȭ�� ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ���� ȭ�� ����
        GeneralMenuButtons.Instance.GetScreen().Execute();
    }


    public void Execute()
    {
        // �� ȭ�� Ȱ��ȭ
        gameObject.SetActive(true);

        // ���� �޴� ȭ�� ����
        GeneralMenuButtons.Instance.SetCurrentScreen(this, _menuButton);
    }



    /* ==================== Private Methods ==================== */
}
