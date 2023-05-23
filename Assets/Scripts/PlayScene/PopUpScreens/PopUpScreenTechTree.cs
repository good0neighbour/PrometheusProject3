using UnityEngine;

public class PopUpScreenTechTree : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TechTreeViewBase[] _techTreeView = new TechTreeViewBase[(int)TechTreeType.TechTreeEnd];

    private byte _currentTechTree = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��ũƮ�� ȭ�� Ȱ��ȭ
    /// </summary>
    public void ActiveThis(TechTreeType techTreeType)
    {
        // ���� ��ũƮ�� ����
        _currentTechTree = (byte)techTreeType;

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
