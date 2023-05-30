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
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
    }


    public void BtnStart()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.TakeOff);
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ���� ����
        AudioManager.Instance.FadeOutThemeMusic();

        // �⺻
        GameManager.Instance.StartFund = 0;
        GameManager.Instance.StartResearch = 0;
        GameManager.Instance.StartResources = 0;

        // ���� ����
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

        // ���� ����
        gameObject.SetActive(false);
        GameManager.Instance.IsNewGame = true;
        TitleMenuManager.Instance.GameStart();
    }


    public void BtnSlot(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ����
        _currentSlot = (byte)index;

        // ���� ��ư Ȱ��ȭ
        for (byte i = 0; i < _agendaBtns.Length; ++i)
        {
            _agendaBtns[i].SetActive(true);
        }
    }


    public void BtnAgenda(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ���
        if (_agendas[_currentSlot] == index)
        {
            _agendas[_currentSlot] = 0;
            _agendaTexts[_currentSlot].text = Language.Instance["���� ����"];
        }
        else
        {
            // ���� ����
            _agendas[_currentSlot] = (byte)index;

            // ���� ǥ��
            switch (index)
            {
                case 1:
                    _agendaTexts[_currentSlot].text = Language.Instance["���� ��ȭ"];
                    break;

                case 2:
                    _agendaTexts[_currentSlot].text = Language.Instance["ȯ�� �ı�"];
                    break;

                case 3:
                    _agendaTexts[_currentSlot].text = Language.Instance["�ڿ� ��"];
                    break;

                default:
                    Debug.LogError("�߸��� ���� ��ȣ");
                    return;
            }
        }

        // ���� ��ư ��Ȱ��ȭ
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
                    _agendaTexts[i].text = Language.Instance["���� ����"];
                    break;

                case 1:
                    _agendaTexts[i].text = Language.Instance["���� ��ȭ"];
                    break;

                case 2:
                    _agendaTexts[i].text = Language.Instance["ȯ�� �ı�"];
                    break;

                case 3:
                    _agendaTexts[i].text = Language.Instance["�ڿ� ��"];
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
