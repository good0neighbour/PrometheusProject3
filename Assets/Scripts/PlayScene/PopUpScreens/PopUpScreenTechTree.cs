using UnityEngine;

public class PopUpScreenTechTree : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TechTreeBase[] _techTreeView = new TechTreeBase[(int)TechTreeType.TechTreeEnd];

    private byte _currentTechTree = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 테크트리 화면 활성화
    /// </summary>
    public void ActiveThis(TechTreeType techTreeType)
    {
        // 현재 테크트리 변경
        _currentTechTree = (byte)techTreeType;

        // 현재 테크트리 실행
        _techTreeView[_currentTechTree].Execute();

        // 전체 창 연다.
        gameObject.SetActive(true);
    }


    public void BtnAdopt()
    {
        // 다형성
        _techTreeView[_currentTechTree].BtnAdopt();
    }


    public void BtnBack()
    {
        // 다형성
        _techTreeView[_currentTechTree].ChangeState();

        // 전체 창 닫는다.
        gameObject.SetActive(false);
    }



    /* ==================== Private Methods ==================== */
}
