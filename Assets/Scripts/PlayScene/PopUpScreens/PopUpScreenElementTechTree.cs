using UnityEngine;

public class PopUpScreenElementTechTree : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private PopUpViewSociety[] _techTreeView = null;

    private byte _currentTechTree = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��ũƮ�� ȭ�� Ȱ��ȭ
    /// </summary>
    public void ActiveThis(byte index)
    {
        // ���� ��ũƮ�� ����
        _currentTechTree = index;

        // ���� ��ũƮ�� ����
        _techTreeView[_currentTechTree].Execute();

        // ��ü â ����.
        gameObject.SetActive(true);
    }


    public void BtnAdopt()
    {
        // ������
        _techTreeView[_currentTechTree].BtnAdopt();
    }


    public void BtnBack()
    {
        // ������
        _techTreeView[_currentTechTree].ChangeState();

        // ��ü â �ݴ´�.
        gameObject.SetActive(false);
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BtnBack();
        }
    }
}
