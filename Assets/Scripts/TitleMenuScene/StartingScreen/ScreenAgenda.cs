using UnityEngine;
using TMPro;

public class ScreenAgenda : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text[] _agendaTexts = null;
    [SerializeField] private GameObject[] _agendaBtns = null;
    [SerializeField] private GameObject _previousScreen = null;

    private byte[] _agendas = null;
    private byte _currentSlot = 0;



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
        AudioManager.Instance.PlayAuido(AudioType.TakeOff);
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 배경 음악 종료
        AudioManager.Instance.FadeOutThemeMusic();

        // 기본
        GameManager.Instance.StartFund = 0;
        GameManager.Instance.StartResearch = 0;
        GameManager.Instance.StartResources = 0;

        // 의제 적용
        for (byte i = 0; i < _agendas.Length; ++i)
        {
            switch (_agendas[i])
            {
                case 1:
                    GameManager.Instance.StartFund += Constants.FUND_AGENDA;
                    break;
                case 2:
                    ++GameManager.Instance.StartResearch;
                    break;
                case 3:
                    ++GameManager.Instance.StartResources;
                    break;
                default:
                    break;
            }
        }

        // 게임 시작
        gameObject.SetActive(false);
        GameManager.Instance.IsNewGame = true;
        TitleMenuManager.Instance.GameStart();
    }


    public void BtnSlot(int index)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 슬롯
        _currentSlot = (byte)index;

        // 의제 버튼 활성화
        for (byte i = 0; i < _agendaBtns.Length; ++i)
        {
            _agendaBtns[i].SetActive(true);
        }
    }


    public void BtnAgenda(int index)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 의제 취소
        if (_agendas[_currentSlot] == index)
        {
            _agendas[_currentSlot] = 0;
            _agendaTexts[_currentSlot].text = Language.Instance["의제 없음"];
        }
        else
        {
            // 의제 저장
            _agendas[_currentSlot] = (byte)index;

            // 의제 표시
            switch (index)
            {
                case 1:
                    _agendaTexts[_currentSlot].text = Language.Instance["기후 변화"];
                    break;

                case 2:
                    _agendaTexts[_currentSlot].text = Language.Instance["환경 파괴"];
                    break;

                case 3:
                    _agendaTexts[_currentSlot].text = Language.Instance["자원 고갈"];
                    break;

                default:
                    Debug.LogError("잘못된 의제 번호");
                    return;
            }
        }

        // 의제 버튼 비활성화
        for (byte i = 0; i < _agendaBtns.Length; ++i)
        {
            _agendaBtns[i].SetActive(false);
        }
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        for (byte i = 0; i < _agendas.Length; ++i)
        {
            switch (_agendas[i])
            {
                case 0:
                    _agendaTexts[i].text = Language.Instance["의제 없음"];
                    break;

                case 1:
                    _agendaTexts[i].text = Language.Instance["기후 변화"];
                    break;

                case 2:
                    _agendaTexts[i].text = Language.Instance["환경 파괴"];
                    break;

                case 3:
                    _agendaTexts[i].text = Language.Instance["자원 고갈"];
                    break;
            }
        }
    }


    private void Awake()
    {
        _agendas = new byte[_agendaTexts.Length];
        OnLanguageChange();
        Language.OnLanguageChange += OnLanguageChange;
    }
}
