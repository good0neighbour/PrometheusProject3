using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonCoolTime : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("설정")]
    [SerializeField] private byte _fundCost = 0;
    [SerializeField] private byte _cultureCost = 0;
    [SerializeField] private float _coolTimeSpeedmult = 0.1f;
    [SerializeField] private byte _adoptIndexNumber = 0;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private Image _availableImage = null;
    [SerializeField] private CoolTimeBtnScreenBase _screen = null;

    private float _availableImageAmount = 0.0f;
    private bool _isCoolTimeRunning = false;
    private bool _isAvailable = false;



    /* ==================== Public Methods ==================== */

    public void BtnAdopt()
    {
        // 사용 불가
        if (_isCoolTimeRunning || !_isAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 비용 지출
        PlayManager.Instance[VariableUint.Culture] -= _cultureCost;

        // 승인 동작
        _screen.OnAdopt(_adoptIndexNumber);

        // 재사용 대기
        _availableImageAmount = 0.0f;
        _isCoolTimeRunning = true;
        _availableImage.fillAmount = _availableImageAmount;
        _titleText.color = Constants.TEXT_BUTTON_DISABLE;

        // 비용 확인
        OnMonthChange();
    }



    /* ==================== Private Methods ==================== */

    private void CoolTimeRunning()
    {
        // 사용 불가
        if (!_isCoolTimeRunning)
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
            _isCoolTimeRunning = false;

            // 비용 확인 후 활성화
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
        // 애니메이션 진행 중이거나 활성화 상태가 아니면 반환한다.
        if (_isCoolTimeRunning || !gameObject.activeSelf)
        {
            return;
        }

        // 비용 확인
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
