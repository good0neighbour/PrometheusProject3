using UnityEngine;

public class PopUpScreenElementTechTree : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private PopUpViewSociety[] _techTreeView = null;

    private byte _currentTechTree = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 테크트리 화면 활성화
    /// </summary>
    public void ActiveThis(byte index)
    {
        // 현재 테크트리 변경
        _currentTechTree = index;

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

    private void Update()
    {
        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BtnBack();
        }
    }
}
