using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeElementSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("기본")]
    [SerializeField] private string _elementName = null;
    [SerializeField] private float _progreesionPerOnce = 0.1f;

    [Header("기본")]
    [SerializeField] private ushort _cultureCost = 1;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private Image _progressionImage = null;

    private float _animationAmount = 0.0f;
    private float _animationGoalValue = 0.0f;
    private ushort _descriptionNum = 0;
    private byte _elementNum = 0;
    private bool _isAvailable = false;
    private bool _animationProceed = false;

    public bool IsAvailable
    {
        get
        {
            return _isAvailable;
        }
        set
        {
            _isAvailable = value;
            if (IsAvailable)
            {
                _titleText.color = Constants.WHITE;
            }
            else
            {
                _titleText.color = Constants.TEXT_BUTTON_DISABLE;
            }
        }
    }



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        PopUpViewSociety.Instance.NodeSelect(-1, _elementNum, Language.Instance[_elementName], Language.Instance[_descriptionNum], IsAvailable);
    }


    /// <summary>
    /// 승인 버튼 클릭 시
    /// </summary>
    public void BtnAdopt()
    {
        PlayManager.Instance[VariableUint.Culture] -= _cultureCost;
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Culture);
    }


    /// <summary>
    /// 승인 성공 시 동작
    /// </summary>
    public void OnAdopt(float[] progresions)
    {
        // 진행도 저장
        progresions[_elementNum] += _progreesionPerOnce;

        // 애니메이션 실행
        _animationGoalValue = progresions[_elementNum];
        _animationAmount = _animationGoalValue - _progressionImage.fillAmount;
        _animationProceed = true;
    }


    /// <summary>
    /// 노드 초기화
    /// </summary>
    public void SetElement(byte elementNum)
    {
        _elementNum = elementNum;
        _titleText.text = _elementName;
        _titleText.color = Constants.TEXT_BUTTON_DISABLE;
        _descriptionNum = (ushort)(Language.Instance.GetLanguageIndex(_elementName) + 1);
    }


    /// <summary>
    /// 비용 확인
    /// </summary>
    public bool CostAvailable()
    {
        return _cultureCost <= PlayManager.Instance[VariableUint.Culture];
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        if (_animationProceed)
        {
            _progressionImage.fillAmount += _animationAmount * Time.deltaTime;

            if (_animationGoalValue <= _progressionImage.fillAmount)
            {
                _progressionImage.fillAmount = _animationGoalValue;
                _animationProceed = false;
            }
        }
    }
}
