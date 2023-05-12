using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PopUpScreenMenu : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _mainScreen = null;
    [SerializeField] private GameObject _languagePopUpScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnLanguage()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� ���� â ����
        _languagePopUpScreen.SetActive(true);
    }


    public void BtnBack()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ����
        _mainScreen.SetActive(true);
        gameObject.SetActive(false);
        GeneralMenuButtons.Instance.EnableThis(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }


    public void BtnLanguageSelect(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� ���� â �ݱ�
        _languagePopUpScreen.SetActive(false);

        // ��� �ҷ�����
        Language.Instance.LoadLangeage((LanguageType)index);
    }



    /* ==================== Private Methods ==================== */
}
