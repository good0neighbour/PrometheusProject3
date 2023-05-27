using UnityEngine;

public class ScreenAgenda : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _previousScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
    }


    public void BtnStart()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ���� ����
        gameObject.SetActive(false);
        GameManager.Instance.IsNewGame = true;
        TitleMenuManager.Instance.GameStart();
    }



    /* ==================== Private Methods ==================== */
}
