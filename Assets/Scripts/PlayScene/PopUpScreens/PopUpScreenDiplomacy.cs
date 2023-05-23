using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScreenDiplomacy : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private CoolTimeBtnDiplomacySemiBase[] _buttons = null;
    [SerializeField] private string[] _buttonDescriptions = null;
    [SerializeField] private TMP_Text _dexcriptionText = null;
    [SerializeField] private TMP_Text _adoptBtnText = null;
    [SerializeField] private TMP_Text _statusText = null;
    [SerializeField] private TMP_Text _backBtnText = null;
    [SerializeField] private Image _progressionImage = null;
    [SerializeField] private GameObject _previousScreen = null;

    private byte _currentBtn = 0;
    private float _supportRate = 0.0f;
    private float _timer = 0.0f;
    private bool _isAdoptAvailable = false;
    private bool _isBackBtnAvailable = true;
    private bool _adoptAnimationProceed = false;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // ��� �Ұ�
        if (!_isBackBtnAvailable)
        {
            return;
        }

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);

        // ó�� ���·�
        _adoptBtnText.text = Language.Instance["����"];
        SetAdoptAvailable(false);
        _dexcriptionText.text = null;
        _statusText.text = null;
    }


    public void BtnAdopt()
    {
        // ��� �Ұ�
        if (!_isAdoptAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��ư ��� �Ұ�
        _isAdoptAvailable = false;

        // �ڷΰ��� ����
        _isBackBtnAvailable = false;
        _backBtnText.color = Constants.TEXT_BUTTON_DISABLE;

        // �ִϸ��̼� ����
        AdoptAnimation(PlayManager.Instance[VariableFloat.SocietySupportRate]);
    }


    public void BtnTouch(int index)
    {
        _currentBtn = (byte)index;
        _dexcriptionText.text = _buttonDescriptions[_currentBtn];

        if (_buttons[_currentBtn].IsCoolTimeRunning)
        {
            _adoptBtnText.text = Language.Instance["��� ��"];
            SetAdoptAvailable(false);
        }
        else
        {
            _adoptBtnText.text = Language.Instance["����"];
            SetAdoptAvailable(_buttons[_currentBtn].IsAvailable);
        }
    }



    /* ==================== Private Methods ==================== */

    private void AdoptAnimation(float supportRate)
    {
        _adoptAnimationProceed = true;
        _supportRate = supportRate;
    }


    private void SetAdoptAvailable(bool available)
    {
        _isAdoptAvailable = available;
        if (_isAdoptAvailable)
        {
            _adoptBtnText.color = Constants.WHITE;
            _isAdoptAvailable = true;
        }
        else
        {
            _adoptBtnText.color = Constants.TEXT_BUTTON_DISABLE;
            _isAdoptAvailable = false;
        }
    }


    private void Update()
    {
        if (_adoptAnimationProceed)
        {
            _timer += Time.deltaTime;
            _progressionImage.fillAmount = _timer;
            if (1.0f <= _timer)
            {
                if (_supportRate >= Random.Range(0.0f, Constants.MAX_SUPPORT_RATE_ADOPTION))
                {
                    _buttons[_currentBtn].BtnAdopt();
                }
                else
                {
                    _buttons[_currentBtn].OnFail();
                }

                // ��� Ȯ�� �� ���� ��ư Ȱ��ȭ
                SetAdoptAvailable(_buttons[_currentBtn].IsAvailable);

                // �ڷΰ��� ����
                _isBackBtnAvailable = true;
                _backBtnText.color = Constants.WHITE;

                _adoptAnimationProceed = false;
                _progressionImage.fillAmount = 0.0f;
                _timer = 0.0f;
            }
        }
    }
}
