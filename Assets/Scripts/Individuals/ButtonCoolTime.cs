using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCoolTime : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private float _coolTimeSpeedmult = 0.1f;
    [SerializeField] private byte _adoptIndexNumber = 0;
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private Image _availableImage = null;
    [SerializeField] private CoolTimeBtnScreenBase _screen = null;

    private float _availableImageAmount = 0.0f;
    private bool _isCoolTimeRunning = false;



    /* ==================== Public Methods ==================== */

    public void BtnAdopt()
    {
        // ��� �Ұ�
        if (_isCoolTimeRunning)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        _screen.OnAdopt(_adoptIndexNumber);

        // ���� ���
        _availableImageAmount = 0.0f;
        _isCoolTimeRunning = true;
        _availableImage.fillAmount = _availableImageAmount;
        _titleText.color = Constants.TEXT_BUTTON_DISABLE;
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
            _availableImage.fillAmount = 1.0f;
            _isCoolTimeRunning = false;
            _titleText.color = Constants.WHITE;
        }
        else
        {
            _availableImage.fillAmount = _availableImageAmount;
        }
    }


    private void Awake()
    {
        // �븮�� ���
        PlayManager.OnPlayUpdate += CoolTimeRunning;
    }
}
