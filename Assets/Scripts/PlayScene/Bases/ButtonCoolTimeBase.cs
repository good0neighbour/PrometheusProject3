using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ButtonCoolTimeBase : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("부모 클래스")]
    [Header("설정")]
    [SerializeField] private float _coolTimeSpeedmult = 0.1f;

    [Header("참조")]
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
        // 사용 불가
        if (IsCoolTimeRunning || !IsAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 비용 지출
        Cost();

        // 승인 동작
        OnAdopt();

        // 재사용 대기
        _availableImageAmount = 0.0f;
        IsCoolTimeRunning = true;
        _availableImage.fillAmount = _availableImageAmount;
        _titleText.color = Constants.TEXT_BUTTON_DISABLE;

        // 비용 확인 후  버튼 활성화
        OnMonthChange();
    }



    /* ==================== Protected Methods ==================== */

    /// <summary>
    /// 승인 시 동작
    /// </summary>
    protected abstract void OnAdopt();


    /// <summary>
    /// 비용 지출
    /// </summary>
    protected abstract void Cost();


    /// <summary>
    /// 비용 확인
    /// </summary>
    protected abstract bool CostAvailable();



    /* ==================== Private Methods ==================== */

    private void CoolTimeRunning()
    {
        // 사용 불가
        if (!IsCoolTimeRunning)
        {
            return;
        }

        // 재사용 대기 중
        _availableImageAmount += _coolTimeSpeedmult * PlayManager.Instance.GameSpeed * Time.deltaTime;

        //대기 완료
        if (1.0f <= _availableImageAmount)
        {
            // 애니메이션 종료
            _availableImage.fillAmount = 1.0f;
            IsCoolTimeRunning = false;

            // 비용 확인 후  버튼 활성화
            OnMonthChange();
        }
        else
        {
            _availableImage.fillAmount = _availableImageAmount;
        }
    }


    private void OnMonthChange()
    {
        // 애니메이션 진행 중이거나 활성화 상태가 아니면 반환한다.
        if (IsCoolTimeRunning || !gameObject.activeSelf)
        {
            return;
        }

        // 비용 확인
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


    private void Awake()
    {
        // 대리자 등록
        PlayManager.OnMonthCahnge += OnMonthChange;
        PlayManager.OnPlayUpdate += CoolTimeRunning;
    }


    private void OnEnable()
    {
        // 비용 확인
        OnMonthChange();
    }
}
