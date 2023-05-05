using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("노이즈")]
    [SerializeField] private float _noiseSpeed = 50.0f;
    [SerializeField] private float _noiseEffectSpeedmult = 2.0f;
    [SerializeField] private float _noiseBrightness = 0.6f;
    [SerializeField] private float _noiseAlpha = 0.6f;

    [Header("좌우 버튼")]
    [SerializeField] private float _leftRightBtnMinBrightness = 0.5f;

    [Header("참조")]
    [SerializeField] private Image _background = null;
    [SerializeField] private TMP_Text[] _leftRightBtns = null;

    private RectTransform _backgroundTransform = null;
    private float _leftRightTimer = 0.0f;
    private float _leftRightAdd = 0.0f;
    private float _leftRightMultiply = 0.0f;

    public static AnimationManager Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 노이즈 효과 시작
    /// </summary>
    public void NoiseEffect()
    {
        _background.color = new Color(1.0f, 1.0f, 1.0f, _noiseAlpha);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 배경 rectTransform 참조
        _backgroundTransform = _background.rectTransform;

        // 좌우 버튼 애니메이션 상수 산출
        _leftRightMultiply = (1.0f - _leftRightBtnMinBrightness) * 0.5f;
        _leftRightAdd = _leftRightBtnMinBrightness + _leftRightMultiply;
    }


    private void Update()
    {
        // 계산용 지역변수
        float float0;

        #region 노이즈 애니메이션
        _backgroundTransform.localPosition += new Vector3(0.0f, _noiseSpeed, 0.0f);
        if (Constants.HALF_CANVAS_HEIGHT < _backgroundTransform.localPosition.y)
        {
            _backgroundTransform.localPosition -= new Vector3(0.0f, Constants.CANVAS_HEIGHT, 0.0f);
        }
        #endregion

        #region 노이즈 어두워지기
        float0 = _background.color.r + (_noiseBrightness - _background.color.r) * Time.deltaTime * _noiseEffectSpeedmult;
        _background.color = new Color(float0, float0, float0, _noiseAlpha);
        #endregion

        #region 좌우 버튼 애니메이션
        // 알파 조정
        float0 = Mathf.Sin(_leftRightTimer) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[0].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[1].color = new Color(float0, float0, float0, 1.0f);

        float0 = Mathf.Sin(_leftRightTimer + Constants.HALF_PI) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[2].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[3].color = new Color(float0, float0, float0, 1.0f);

        float0 = Mathf.Sin(_leftRightTimer + Constants.PI) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[4].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[5].color = new Color(float0, float0, float0, 1.0f);

        // 타이머
        _leftRightTimer += Time.deltaTime;
        if (_leftRightTimer > Constants.DOUBLE_PI)
        {
            _leftRightTimer -= Constants.DOUBLE_PI;
        }
        #endregion
    }
}
