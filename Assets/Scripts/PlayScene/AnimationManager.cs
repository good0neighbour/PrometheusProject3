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
    [SerializeField] private float _cloudSpinSpeedmult = 1.0f;

    [Header("행성 색상 업데이트")]
    [SerializeField] private float _planetColourUpdateTimer = 1.0f;
    [SerializeField] private Color _landMin = new Color(140.0f / 255.0f, 120.0f / 255.0f, 100.0f / 255.0f);
    [SerializeField] private Color _landMax = new Color(90.0f / 255.0f, 120.0f / 255.0f, 30.0f / 255.0f);
    [SerializeField] private Color _cloudColour = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    [SerializeField] private Color _atmosphereBaseColor = new Color();
    [SerializeField] private Color _imagePlanetAtmosphereBaseColor = new Color();
    [SerializeField] private Color _nightColour = new Color();
    [SerializeField] private float _oceanMin = 0.95f;
    [SerializeField] private float _oceanMiddle = 0.5f;
    [SerializeField] private float _iceMin = 0.95f;
    [SerializeField] private float _iceMiddle = 0.5f;
    [SerializeField] private float _nightBrightMultiply = 0.00001f;

    [Header("참조")]
    [SerializeField] private Image _background = null;
    [SerializeField] private Image _messageBoxBackground = null;
    [SerializeField] private TMP_Text[] _leftRightBtns = new TMP_Text[6];
    [SerializeField] private Image _selection = null;
    [SerializeField] private Image _imagePlanetAtmosphere = null;
    [SerializeField] private Transform _planet = null;
    [SerializeField] private Transform _cloudTransform = null;
    [SerializeField] private Transform _sun = null;
    [SerializeField] private RectTransform _planetImage = null;
    [SerializeField] private RectTransform _spaceImage = null;
    [SerializeField] private RectTransform _messageBoxTransform = null;
    [SerializeField] private Material _land = null;
    [SerializeField] private Material _ocean = null;
    [SerializeField] private Material _cloud = null;
    [SerializeField] private Material _overcloudy = null;
    [SerializeField] private Material _atmosphere = null;
    [SerializeField] private Material _night = null;
    [SerializeField] private Material _ice = null;

    private RectTransform _backgroundTransform = null;
    private RectTransform _messageBoxBackgroundTransform = null;
    private Color _landColourGap = new Color();
    private float _leftRightTimer = 0.0f;
    private float _leftRightAdd = 0.0f;
    private float _leftRightMultiply = 0.0f;
    private float _selectionAdd = 0.0f;
    private float _selectionMultiply = 0.0f;
    private float _planetImageTargetPos = 0.0f;
    private float _spaceImageTargetPos = 0.0f;
    private float _messageBoxTargetPos = 0.0f;
    private float _timer = 0.0f;
    private float _oceanGap = 0.0f;
    private float _iceGap = 0.0f;
    private float _waterLiquidMultiply = 1.0f / 1379705.3f;
    private float _waterGasMultiply = 1.0f / 2.7f;
    private float _airPressureMultiply = 1.0f / 1013.25f;
    private float _iceMultiply = 1.0f / 29000f;
    private int _breathStability = -1;
    private int _waterLiquid = -1;
    private int _waterGas = -1;
    private int _airPressure = -1;
    private int _totalPopulation = -1;
    private int _waterSolid = -1;

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

    /// <summary>
    /// 행성 색상 결정
    /// </summary>
    private void PlanetColour()
    {
        // 계산용 지역변수
        float float0;
        int int0;
        uint uint0;

        // 지표면
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.PhotoLifeStability]);
        if (_breathStability != int0)
        {
            _breathStability = int0;
            float0 = _breathStability * 0.01f;
            Color landColour = new Color(
                _landMin.r - _landColourGap.r * float0,
                _landMin.g - _landColourGap.g * float0,
                _landMin.b - _landColourGap.b * float0,
                1.0f);
            _land.color = landColour;
        }

        // 대양
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.WaterLiquid_PL]);
        if (_waterLiquid != int0)
        {
            _waterLiquid = int0;
            float0 = _oceanMin - _waterLiquid * _oceanGap * _waterLiquidMultiply;
            if (0.0f > float0)
            {
                float0 = 0.0f;
            }
            _ocean.SetFloat("_Cutoff", float0);
        }

        // 구름
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.WaterGas_PL]);
        if (_waterGas != int0)
        {
            _waterGas = int0;
            float0 = _waterGas * _waterGasMultiply;
            if (1.0f > float0)
            {
                _cloud.color = new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, float0);
                _overcloudy.color = new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, 0.0f);
            }
            else if (3.0f > float0)
            {
                _cloud.color = new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, 1.5f - float0 * 0.5f);
                _overcloudy.color = new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, float0 * 0.5f - 1.0f);
            }
            else
            {
                _cloud.color = new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, 0.0f);
                _overcloudy.color = new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, 1.0f);
            }
        }

        // 대기
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.TotalAirPressure_hPa]);
        if (_airPressure != int0)
        {
            _airPressure = int0;
            float0 = _airPressure * _airPressureMultiply;
            if (1.0f < float0)
            {
                float0 = 1.0f;
            }
            _atmosphere.color = new Color(_atmosphereBaseColor.r, _atmosphereBaseColor.g, _atmosphereBaseColor.b, float0);
            _imagePlanetAtmosphere.color = new Color(_imagePlanetAtmosphereBaseColor.r, _imagePlanetAtmosphereBaseColor.g, _imagePlanetAtmosphereBaseColor.b, float0);
        }

        // 야간 조명
        uint0 = PlayManager.Instance[VariableUint.TotalPopulation];
        if (int.MaxValue < uint0)
        {
            uint0 = int.MaxValue;
        }
        if (_totalPopulation != uint0)
        {
            _totalPopulation = (int)uint0;
            float0 = _totalPopulation * _nightBrightMultiply;
            if (1.0f < float0)
            {
                float0 = 1.0f;
            }
            _night.SetColor("_EmissionColor", new Color(_nightColour.r * float0, _nightColour.g * float0, _nightColour.b * float0, 1.0f));
        }

        // 빙하
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.WaterSolid_PL]);
        if (_waterSolid != int0)
        {
            _waterSolid = int0;
            float0 = _iceMin - _waterSolid * _iceGap * _iceMultiply;
            if (0.0f > float0)
            {
                float0 = 0.0f;
            }
            _ice.SetFloat("_Cutoff", float0);
        }
    }


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

        // 고정 값
        _landColourGap = _landMin - _landMax;
        _oceanGap = _oceanMin - _oceanMiddle;
        _iceGap = _iceMin - _iceMiddle;

        // 처음 시작 시 0.5초 후 행성 색상 업데이트
        _timer = _planetColourUpdateTimer - 0.5f;
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
        _background.color = new Color(float0, float0, float0, 1.0f);
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
            _cloudTransform.Rotate(0.0f, -_cloudSpinSpeedmult * Time.deltaTime, 0.0f);
            _sun.Rotate(0.0f, _spinSpeedmult * Time.deltaTime, 0.0f);
        }
        #endregion

        #region 행성 색상
        _timer += Time.deltaTime;
        if (_timer >= _planetColourUpdateTimer)
        {
            _timer -= _planetColourUpdateTimer;
            PlanetColour();
        }
        #endregion
    }
}
