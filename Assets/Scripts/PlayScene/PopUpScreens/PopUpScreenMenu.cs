using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PopUpScreenMenu : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _mainScreen = null;
    [SerializeField] private GameObject _settingsPopUpScreen = null;
    [SerializeField] private GameObject _languagePopUpScreen = null;
    [SerializeField] private GameObject _soundVolumePopUpScreen = null;
    [SerializeField] private GameObject _fPSPopUpScreen = null;
    [SerializeField] private GameObject _withDrawPopUpScreen = null;
    [SerializeField] private TMP_Text _currentSoundVolume = null;
    [SerializeField] private TMP_Text _currentFPS = null;



    /* ==================== Public Methods ==================== */

    public void BtnLanguage()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� ���� â ����
        _languagePopUpScreen.SetActive(true);
    }


    public void BtnSoundVolume()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� Ȱ��ȭ
        _soundVolumePopUpScreen.SetActive(true);
    }


    public void BtnFPS()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� Ȱ��ȭ
        _fPSPopUpScreen.SetActive(true);
    }


    public void BtnWithdrawPopUp(bool enable)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� Ȱ��ȭ
        _withDrawPopUpScreen.SetActive(enable);
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


    public void BtnSettings(bool enable)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� Ȱ��ȭ
        _settingsPopUpScreen.SetActive(enable);
    }


    public void BtnLanguageSelect(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� ���� â �ݱ�
        _languagePopUpScreen.SetActive(false);

        // ��� �ҷ�����
        Language.Instance.LoadLangeage((LanguageType)index);

        // ���� ����
        GameManager.Instance.SaveSettings();
    }


    public void BtnSoundVolumeSelect(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        switch (index)
        {
            case 0:
                GameManager.Instance.SoundVolume = 0.0f;
                break;

            case 1:
                GameManager.Instance.SoundVolume = 0.25f;
                break;

            case 2:
                GameManager.Instance.SoundVolume = 0.5f;
                break;

            case 3:
                GameManager.Instance.SoundVolume = 0.75f;
                break;

            case 4:
                GameManager.Instance.SoundVolume = 1.0f;
                break;

            default:
                return;
        }

        AudioManager.Instance.OnSoundVolumeChanged();
        _currentSoundVolume.text = $"{Language.Instance["���� ����"]} {(GameManager.Instance.SoundVolume * 100.0f).ToString("0")}%";
        _soundVolumePopUpScreen.SetActive(false);
        GameManager.Instance.SaveSettings();
    }


    public void BtnFPSSelect(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        switch (index)
        {
            case 0:
                GameManager.Instance.TargetFrameRate = 30;
                Application.targetFrameRate = 30;
                break;

            case 1:
                GameManager.Instance.TargetFrameRate = 45;
                Application.targetFrameRate = 45;
                break;

            case 2:
                GameManager.Instance.TargetFrameRate = 60;
                Application.targetFrameRate = 60;
                break;

            default:
                return;
        }

        // ǥ�� ������Ʈ
        _currentFPS.text = $"{Language.Instance["���� �ʴ� ������ ��"]} {GameManager.Instance.TargetFrameRate.ToString()}";

        // ȭ�� ��Ȱ��ȭ
        _fPSPopUpScreen.SetActive(false);

        // ���� ����
        GameManager.Instance.SaveSettings();
    }


    public void BtnQuit()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ���� ����
        AudioManager.Instance.FadeOutThemeMusic();

        // ����
        PlayManager.Instance.SaveGame();
        GameManager.Instance.SaveSettings();

        // ����
        Application.Quit();
    }


    public void BtnWithdraw()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ���� ����
        AudioManager.Instance.FadeOutThemeMusic();

        // ����
        GameManager.Instance.IsThereSavedGame = false;
        GameManager.Instance.SaveSettings();

        // �� ȭ������ �̵�
        Language.OnLanguageChange = null;
        PlayManager.OnYearChange = null;
        PlayManager.OnMonthChange = null;
        PlayManager.OnPlayUpdate = null;
        SceneManager.LoadScene(0);
    }


    public void BtnTitleMenu()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ���� ����
        AudioManager.Instance.FadeOutThemeMusic();

        // ����
        PlayManager.Instance.SaveGame();
        GameManager.Instance.IsThereSavedGame = true;
        GameManager.Instance.SaveSettings();

        // �� ȭ������ �̵�
        Language.OnLanguageChange = null;
        PlayManager.OnYearChange = null;
        PlayManager.OnMonthChange = null;
        PlayManager.OnPlayUpdate = null;
        SceneManager.LoadScene(0);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ǥ�� ������Ʈ
        _currentSoundVolume.text = $"{Language.Instance["���� ����"]} {(GameManager.Instance.SoundVolume * 100.0f).ToString("0")}%";
        _currentFPS.text = $"{Language.Instance["���� �ʴ� ������ ��"]} {GameManager.Instance.TargetFrameRate.ToString()}";
    }


    private void Update()
    {
        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Touch);

            // ȭ�� ��Ȱ��ȭ
            if (_languagePopUpScreen.activeSelf)
            {
                _languagePopUpScreen.SetActive(false);
            }
            else if (_soundVolumePopUpScreen.activeSelf)
            {
                _soundVolumePopUpScreen.SetActive(false);
            }
            else if (_fPSPopUpScreen.activeSelf)
            {
                _fPSPopUpScreen.SetActive(false);
            }
            else if (_settingsPopUpScreen.activeSelf)
            {
                BtnSettings(false);
            }
            else if (_withDrawPopUpScreen.activeSelf)
            {
                BtnWithdrawPopUp(false);
            }
            else
            {
                BtnBack();
            }
        }
    }
}
