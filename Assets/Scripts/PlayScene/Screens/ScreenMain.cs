using UnityEngine;

public class ScreenMain : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("����")]
    [SerializeField] private GameObject _popUpMenuScreen = null;
    [SerializeField] private GameObject _popMessageLogScreen = null;

    private bool _isMessageLogOpened = false;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// �޴� ȭ�� ����
    /// </summary>
    public void BtnPopUpMenu()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ����
        _popUpMenuScreen.SetActive(true);
        gameObject.SetActive(false);
        GeneralMenuButtons.Instance.EnableThis(false);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnMessageLog(bool openPopUpScreen)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ����
        if (openPopUpScreen)
        {
            _popMessageLogScreen.SetActive(true);
            gameObject.SetActive(false);
            GeneralMenuButtons.Instance.EnableThis(false);
            PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
        }
        else
        {
            _popMessageLogScreen.SetActive(false);
            gameObject.SetActive(true);
            GeneralMenuButtons.Instance.EnableThis(true);
            PlayManager.Instance.GameResume = Constants.GAME_RESUME;
        }

        _isMessageLogOpened = openPopUpScreen;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // ����Ű ����
        // �޼��� �α� ������ ��
        if (_isMessageLogOpened)
        {
            // ���� ����
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnMessageLog(false);
            }
        }
        // �߾� ȭ���� ��
        else
        {
#if PLATFORM_STANDALONE_WIN
            // Ű���� ����
            if (Input.GetKeyUp(KeyCode.A))
            {
                GeneralMenuButtons.Instance.BtnLeftRight(true);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                GeneralMenuButtons.Instance.BtnLeftRight(false);
            }
#endif
            // ���� ����
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnPopUpMenu();
            }
        }
    }
}
