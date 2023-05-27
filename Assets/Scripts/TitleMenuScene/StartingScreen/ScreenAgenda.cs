using UnityEngine;

public class ScreenAgenda : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _previousScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 전환
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
    }


    public void BtnStart()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 게임 시작
        gameObject.SetActive(false);
        GameManager.Instance.IsNewGame = true;
        TitleMenuManager.Instance.GameStart();
    }



    /* ==================== Private Methods ==================== */
}
