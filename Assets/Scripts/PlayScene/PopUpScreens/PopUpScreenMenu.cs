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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 언어 선택 창 열기
        _languagePopUpScreen.SetActive(true);
    }


    public void BtnSoundVolume()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 활성화
        _soundVolumePopUpScreen.SetActive(true);
    }


    public void BtnFPS()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 활성화
        _fPSPopUpScreen.SetActive(true);
    }


    public void BtnWithdrawPopUp(bool enable)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 활성화
        _withDrawPopUpScreen.SetActive(enable);
    }


    public void BtnBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 변경
        _mainScreen.SetActive(true);
        gameObject.SetActive(false);
        GeneralMenuButtons.Instance.EnableThis(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }


    public void BtnSettings(bool enable)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 활성화
        _settingsPopUpScreen.SetActive(enable);
    }


    public void BtnLanguageSelect(int index)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 언어 선택 창 닫기
        _languagePopUpScreen.SetActive(false);

        // 언어 불러오기
        Language.Instance.LoadLangeage((LanguageType)index);

        // 설정 저장
        GameManager.Instance.SaveSettings();
    }


    public void BtnSoundVolumeSelect(int index)
    {
        // 소리 재생
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
        _currentSoundVolume.text = $"{Language.Instance["현재 음량"]} {(GameManager.Instance.SoundVolume * 100.0f).ToString("0")}%";
        _soundVolumePopUpScreen.SetActive(false);
        GameManager.Instance.SaveSettings();
    }


    public void BtnFPSSelect(int index)
    {
        // 소리 재생
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

        // 표시 업데이트
        _currentFPS.text = $"{Language.Instance["현재 초당 프레임 수"]} {GameManager.Instance.TargetFrameRate.ToString()}";

        // 화면 비활성화
        _fPSPopUpScreen.SetActive(false);

        // 설정 저장
        GameManager.Instance.SaveSettings();
    }


    public void BtnQuit()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 배경 음악 종료
        AudioManager.Instance.FadeOutThemeMusic();

        // 저장
        PlayManager.Instance.SaveGame();
        GameManager.Instance.SaveSettings();

        // 종료
        Application.Quit();
    }


    public void BtnWithdraw()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 배경 음악 종료
        AudioManager.Instance.FadeOutThemeMusic();

        // 저장
        GameManager.Instance.IsThereSavedGame = false;
        GameManager.Instance.SaveSettings();

        // 주 화면으로 이동
        Language.OnLanguageChange = null;
        PlayManager.OnYearChange = null;
        PlayManager.OnMonthChange = null;
        PlayManager.OnPlayUpdate = null;
        SceneManager.LoadScene(0);
    }


    public void BtnTitleMenu()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 배경 음악 종료
        AudioManager.Instance.FadeOutThemeMusic();

        // 저장
        PlayManager.Instance.SaveGame();
        GameManager.Instance.IsThereSavedGame = true;
        GameManager.Instance.SaveSettings();

        // 주 화면으로 이동
        Language.OnLanguageChange = null;
        PlayManager.OnYearChange = null;
        PlayManager.OnMonthChange = null;
        PlayManager.OnPlayUpdate = null;
        SceneManager.LoadScene(0);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 표시 업데이트
        _currentSoundVolume.text = $"{Language.Instance["현재 음량"]} {(GameManager.Instance.SoundVolume * 100.0f).ToString("0")}%";
        _currentFPS.text = $"{Language.Instance["현재 초당 프레임 수"]} {GameManager.Instance.TargetFrameRate.ToString()}";
    }


    private void Update()
    {
        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Touch);

            // 화면 비활성화
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
