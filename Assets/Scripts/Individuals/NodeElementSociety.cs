using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NodeElementSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("기본")]
    [SerializeField] private string _elementName = null;
    [SerializeField] private string _description = null;
    [SerializeField] private float _progreesionPerOnce = 0.1f;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private Image _progressionImage = null;

    private float _animationAmount = 0.0f;
    private float _animationGoalValue = 0.0f;
    private byte _elementNum = 0;
    private bool _isAvailable = false;
    private bool _animationProceed = false;



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        PopUpViewSociety.Instance.NodeSelect(-1, _elementNum, _description, _isAvailable);
    }


    /// <summary>
    /// 승인 시 동작
    /// </summary>
    public void OnAdopt(float[] progresions)
    {
        // 진행도 저장
        progresions[_elementNum] += _progreesionPerOnce;

        // 애니메이션 실행
        _animationGoalValue = progresions[_elementNum];
        _animationAmount = _animationGoalValue - _progressionImage.fillAmount;
        _animationProceed = true;

        Debug.Log("ElementAdopt");
    }


    public void SetElement(byte elementNum)
    {
        _elementNum = elementNum;
        _titleText.text = _elementName;
    }


    public void SetAvailable(bool available)
    {
        _isAvailable = available;
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
