using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 행성 이미지 위치 목록
/// </summary>
public enum PlanetImagePosition
{
    Left,
    Middle,
    Right
}

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

    [Header("좌우 목록 선택")]
    [SerializeField] private float _selectionMaxBrightness = 1.0f;
    [SerializeField] private float _selectionMinBrightness = 0.5f;
    [SerializeField] private Color _selectionColor = Color.white;

    [Header("메세지 상자 이동 속도")]
    [SerializeField] private float _messageBoxSpeedmult = 2.0f;

    [Header("3D 애니메이션")]
    [SerializeField] private float _spinSpeedmult = 1.0f;

    [Header("참조")]
    [SerializeField] private Image _background = null;
    [SerializeField] private Image _messageBoxBackground = null;
    [SerializeField] private TMP_Text[] _leftRightBtns = new TMP_Text[6];
    [SerializeField] private Image _selection = null;
    [SerializeField] private Transform _planet = null;
    [SerializeField] private Transform _sun = null;
    [SerializeField] private RectTransform _planetImage = null;
    [SerializeField] private RectTransform _spaceImage = null;
    [SerializeField] private RectTransform _messageBoxTransform = null;

    private RectTransform _backgroundTransform = null;
    private RectTransform _messageBoxBackgroundTransform = null;
    private float _leftRightTimer = 0.0f;
    private float _leftRightAdd = 0.0f;
    private float _leftRightMultiply = 0.0f;
    private float _selectionAdd = 0.0f;
    private float _selectionMultiply = 0.0f;
    private float _planetImageTargetPos = 0.0f;
    private float _spaceImageTargetPos = 0.0f;
    private float _messageBoxTargetPos = 0.0f;

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
        _messageBoxBackground.color = new Color(1.0f, 1.0f, 1.0f, _noiseAlpha);
    }


    /// <summary>
    /// 행성 이미지, 우주 배경, 메세지 상자의 목표 위치 설정
    /// </summary>
    public void SetPlanetImagePosition(PlanetImagePosition position)
    {
        switch (position)
        {
            case PlanetImagePosition.Left:
                _planetImageTargetPos = Constants.HALF_CANVAS_WIDTH;
                _spaceImageTargetPos = Constants.SPACE_IMAGE_TARGET_POSITION;
                _messageBoxTargetPos = -Constants.QUARTER_CANVAS_WIDTH;
                break;
            case PlanetImagePosition.Middle:
                _planetImageTargetPos = 0.0f;
                _spaceImageTargetPos = 0.0f;
                _messageBoxTargetPos = 0.0f;
                break;
            case PlanetImagePosition.Right:
                _planetImageTargetPos = -Constants.HALF_CANVAS_WIDTH;
                _spaceImageTargetPos = -Constants.SPACE_IMAGE_TARGET_POSITION;
                _messageBoxTargetPos = Constants.QUARTER_CANVAS_WIDTH;
                break;
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 배경 rectTransform 참조
        _backgroundTransform = _background.rectTransform;
        _messageBoxBackgroundTransform = _messageBoxBackground.rectTransform;

        // 좌우 버튼 애니메이션 상수 산출
        _leftRightMultiply = (1.0f - _leftRightBtnMinBrightness) * 0.5f;
        _leftRightAdd = _leftRightBtnMinBrightness + _leftRightMultiply;

        // 좌우 목록 선택 애니메이션 상수 산출
        _selectionMultiply = (_selectionMaxBrightness - _selectionMinBrightness) * 0.5f;
        _selectionAdd = _selectionMinBrightness + _selectionMultiply;
    }


    private void Update()
    {
        // 계산용 지역변수
        float float0;
        Vector3 vector3_0;

        #region 노이즈 애니메이션
        vector3_0 = _backgroundTransform.localPosition + new Vector3(0.0f, _noiseSpeed, 0.0f);
        if (Constants.HALF_CANVAS_HEIGHT < vector3_0.y)
        {
            vector3_0 -= new Vector3(0.0f, Constants.CANVAS_HEIGHT, 0.0f);
        }
        _backgroundTransform.localPosition = vector3_0;
        _messageBoxBackgroundTransform.localPosition = vector3_0 * Constants.MASSAGEBOX_HEIGHT_RATIO;
        #endregion

        #region 노이즈 어두워지기
        float0 = _background.color.r + (_noiseBrightness - _background.color.r) * Time.deltaTime * _noiseEffectSpeedmult;
        _background.color = new Color(float0, float0, float0, _noiseAlpha);
        _messageBoxBackground.color = new Color(float0, float0, float0, _noiseAlpha);
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

        #region 좌우 목록 선택 오브젝트 애니메이션
        float0 = Mathf.Sin(_leftRightTimer) * _selectionMultiply + _selectionAdd;
        _selection.color = new Color(_selectionColor.r, _selectionColor.g, _selectionColor.b, float0);
        #endregion

        #region 행성 이미지, 우주 배경, 메세지 상자의 위치
        _planetImage.localPosition += new Vector3((_planetImageTargetPos - _planetImage.localPosition.x) * Time.deltaTime, 0.0f, 0.0f);
        _spaceImage.localPosition += new Vector3((_spaceImageTargetPos - _spaceImage.localPosition.x) * Time.deltaTime, 0.0f, 0.0f);
        _messageBoxTransform.localPosition += new Vector3((_messageBoxTargetPos - _messageBoxTransform.localPosition.x) * _messageBoxSpeedmult * Time.deltaTime, 0.0f, 0.0f);
        #endregion

        #region 3D 애니메이션
        // 게임이 일시정지가 아닐 때만
        if (Constants.GAME_RESUME == PlayManager.Instance.GameResume)
        {
            _planet.Rotate(0.0f, -_spinSpeedmult * Time.deltaTime, 0.0f);
            _sun.Rotate(0.0f, _spinSpeedmult * Time.deltaTime, 0.0f);
        }
        #endregion
    }
}
