using UnityEngine;

/// <summary>
/// 유니티 UI에 연결하기 위해 MonoBehaviour를 상속
/// </summary>
public class Screen : MonoBehaviour, IState
{
    /* ==================== Variables ==================== */

    [SerializeField] private Screen[] _screens = null;
    private byte _screenMoveTo = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 메뉴 화면 이동 버튼
    /// </summary>
    public virtual void BtnMoveScreen(int index)
    {
        _screenMoveTo = (byte)index;
        ChangeState();
    }


    public virtual void Execute()
    {
        // 메뉴 화면 활성화
        gameObject.SetActive(true);
    }


    public void ChangeState()
    {
        // 현재 메뉴화면 비활성화
        gameObject.SetActive(false);

        // 다음 상태로 진행
        _screens[_screenMoveTo].Execute();
    }



    /* ==================== Private Methods ==================== */
}
