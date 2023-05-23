using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCoolTime : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("����")]
    [SerializeField] private byte _fundCost = 0;
    [SerializeField] private byte _cultureCost = 0;
    [SerializeField] private float _coolTimeSpeedmult = 0.1f;
    [SerializeField] private byte _adoptIndexNumber = 0;

    [Header("����")]
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private Image _availableImage = null;
    [SerializeField] private CoolTimeBtnScreenBase _screen = null;

    private float _availableImageAmount = 0.0f;
    private bool _isCoolTimeRunning = false;
    private bool _isAvailable = false;



    /* ==================== Public Methods ==================== */

    public void BtnAdopt()
    {
        // ��� �Ұ�
        if (_isCoolTimeRunning || !_isAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ����
        PlayManager.Instance[VariableUint.Culture] -= _cultureCost;

        // ���� ����
        _screen.OnAdopt(_adoptIndexNumber);

        // ���� ���
        _availableImageAmount = 0.0f;
        _isCoolTimeRunning = true;
        _availableImage.fillAmount = _availableImageAmount;
        _titleText.color = Constants.TEXT_BUTTON_DISABLE;

        // ��� Ȯ��
        OnMonthChange();
    }



    /* ==================== Private Methods ==================== */

    private void CoolTimeRunning()
    {
        // ��� �Ұ�
        if (!_isCoolTimeRunning)
        {
            return;
        }

        // ���� ��� ��
        _availableImageAmount += _coolTimeSpeedmult * PlayManager.Instance.GameSpeed * Time.deltaTime;

        //��� �Ϸ�
        if (1.0f <= _availableImageAmount)
        {
            // �ִϸ��̼� ����
            _availableImage.fillAmount = 1.0f;
            _isCoolTimeRunning = false;

            // ��� Ȯ�� �� Ȱ��ȭ
            OnMonthChange();
        }
        else
        {
            _availableImage.fillAmount = _availableImageAmount;
        }
    }


    private bool CostAvaiable()
    {
        if (_cultureCost > PlayManager.Instance[VariableUint.Culture])
        {
            return false;
        }
        if (_fundCost > PlayManager.Instance[VariableLong.Funds])
        {
            return false;
        }

        return true;
    }


    private void OnMonthChange()
    {
        // �ִϸ��̼� ���� ���̰ų� Ȱ��ȭ ���°� �ƴϸ� ��ȯ�Ѵ�.
        if (_isCoolTimeRunning || !gameObject.activeSelf)
        {
            return;
        }

        // ��� Ȯ��
        if (CostAvaiable())
        {
            _isAvailable = true;
            _titleText.color = Constants.WHITE;
        }
        else
        {
            _isAvailable = false;
            _titleText.color = Constants.TEXT_BUTTON_DISABLE;
        }
    }


    private void Awake()
    {
        // �븮�� ���
        PlayManager.OnMonthCahnge += OnMonthChange;
        PlayManager.OnPlayUpdate += CoolTimeRunning;
    }


    private void OnEnable()
    {
        // ��� Ȯ��
        OnMonthChange();
    }
}
