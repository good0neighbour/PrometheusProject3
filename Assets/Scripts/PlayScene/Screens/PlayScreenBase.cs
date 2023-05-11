using UnityEngine;

/// <summary>
/// 유니티 UI에 연결하기 위해 MoniBehaviour를 상속받는다.
/// </summary>
public class PlayScreenBase : MonoBehaviour, IState
{
    /* ==================== Variables ==================== */

    [Header("부모 클래스의 참조변수")]
    [SerializeField] private Transform _menuButton = null;



    /* ==================== Public Methods ==================== */

    public void ChangeState()
    {
        // 이 화면 비활성화
        gameObject.SetActive(false);

        // 다음 화면 실행
        GeneralMenuButtons.Instance.GetScreen().Execute();
    }


    public void Execute()
    {
        // 이 화면 활성화
        gameObject.SetActive(true);

        // 현재 메뉴 화면 변경
        GeneralMenuButtons.Instance.SetCurrentScreen(this, _menuButton);
    }



    /* ==================== Private Methods ==================== */
}
