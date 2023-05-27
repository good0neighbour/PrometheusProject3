using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ButtonCoolTimeBase : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("�θ� Ŭ����")]
    [Header("����")]
    [SerializeField] private float _coolTimeSpeedmult = 0.1f;

    [Header("����")]
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private Image _availableImage = null;

    private float _availableImageAmount = 0.0f;

    public bool IsCoolTimeRunning
    {
        get;
        private set;
    }

    public bool IsAvailable
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnAdopt()
    {
        // ��� �Ұ�
        if (IsCoolTimeRunning || !IsAvailable)
        {
            return;
        }

        // ��� ����
        Cost();

        // ���� ����
        OnAdopt();

        // ���� ���
        _availableImageAmount = 0.0f;
        IsCoolTimeRunning = true;
        _availableImage.fillAmount = _availableImageAmount;
        _titleText.color = Constants.TEXT_BUTTON_DISABLE;
    }



    /* ==================== Protected Methods ==================== */

    /// <summary>
    /// ���� �� ����
    /// </summary>
    protected abstract void OnAdopt();


    /// <summary>
    /// ��� ����
    /// </summary>
    protected abstract void Cost();


    /// <summary>
    /// ��� Ȯ��
    /// </summary>
    protected abstract bool CostAvailable();


    /// <summary>
    /// ��� ���� �� ����
    /// </summary>
    protected virtual void OnCoolTimeEnd()
    {
        _availableImage.fillAmount = 1.0f;
        IsCoolTimeRunning = false;
    }



    /* ==================== Private Methods ==================== */

    private void OnPlayUpdate()
    {
        // �ִϸ��̼� ��� ��
        if (IsCoolTimeRunning)
        {
            switch (PlayManager.Instance.GameSpeed)
            {
                case 0:
                    // �Ͻ����� ���¿����� �������� �ʴ´�.
                    return;
                default:
                    // ���� ��� ��
                    _availableImageAmount += _coolTimeSpeedmult * PlayManager.Instance.GameSpeed * Time.deltaTime;

                    //��� �Ϸ�
                    if (1.0f <= _availableImageAmount)
                    {
                        // ��� ����
                        OnCoolTimeEnd();
                    }
                    else
                    {
                        _availableImage.fillAmount = _availableImageAmount;
                    }
                    return;
            }
        }
        // Ȱ��ȭ�� ��
        else if (gameObject.activeSelf)
        {
            // ��� Ȯ��
            if (CostAvailable())
            {
                IsAvailable = true;
                _titleText.color = Constants.WHITE;
            }
            else
            {
                IsAvailable = false;
                _titleText.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
    }


    private void Awake()
    {
        // �븮�� ���
        PlayManager.OnPlayUpdate += OnPlayUpdate;
    }
}
