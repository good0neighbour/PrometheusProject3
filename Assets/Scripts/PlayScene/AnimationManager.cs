using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �༺ �̹��� ��ġ ���
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

    [Header("������")]
    [SerializeField] private float _noiseSpeed = 50.0f;
    [SerializeField] private float _noiseEffectSpeedmult = 2.0f;
    [SerializeField] private float _noiseBrightness = 0.6f;
    [SerializeField] private float _noiseAlpha = 0.6f;

    [Header("�¿� ��ư")]
    [SerializeField] private float _leftRightBtnMinBrightness = 0.5f;

    [Header("�¿� ��� ����")]
    [SerializeField] private float _selectionMaxBrightness = 1.0f;
    [SerializeField] private float _selectionMinBrightness = 0.5f;
    [SerializeField] private Color _selectionColor = Color.white;

    [Header("�޼��� ���� �̵� �ӵ�")]
    [SerializeField] private float _messageBoxSpeedmult = 2.0f;

    [Header("3D �ִϸ��̼�")]
    [SerializeField] private float _spinSpeedmult = 1.0f;

    [Header("�༺ ���� ������Ʈ")]
    [SerializeField] private float _planetColourUpdateTimer = 1.0f;
    [SerializeField] private Color _landMin = new Color(140.0f / 255.0f, 120.0f / 255.0f, 100.0f / 255.0f);
    [SerializeField] private Color _landMax = new Color(90.0f / 255.0f, 120.0f / 255.0f, 30.0f / 255.0f);
    [SerializeField] private Color _imagePlanetAtmosphereBaseColor = new Color();
    [SerializeField] private float _nightBrightMultiply = 0.00001f;

    [Header("����")]
    [SerializeField] private Image _background = null;
    [SerializeField] private Image _messageBoxBackground = null;
    [SerializeField] private TMP_Text[] _leftRightBtns = new TMP_Text[6];
    [SerializeField] private Image _selection = null;
    [SerializeField] private Image _imagePlanetAtmosphere = null;
    [SerializeField] private Transform _sun = null;
    [SerializeField] private RectTransform _planetImage = null;
    [SerializeField] private RectTransform _spaceImage = null;
    [SerializeField] private RectTransform _messageBoxTransform = null;
    [SerializeField] private Material _planetMaterial = null;
    [SerializeField] private MeshRenderer _planet = null;

    private RectTransform _backgroundTransform = null;
    private RectTransform _messageBoxBackgroundTransform = null;
    private Color _landColourGap = new Color();
    private Color _oceanColour = new Color();
    private Color _iceColour = new Color();
    private Color _cloudColour = new Color();
    private Color _atmoshpereColour = new Color();
    private Color _nightColour = new Color();
    private float _leftRightTimer = 0.0f;
    private float _leftRightAdd = 0.0f;
    private float _leftRightMultiply = 0.0f;
    private float _selectionAdd = 0.0f;
    private float _selectionMultiply = 0.0f;
    private float _planetImageTargetPos = 0.0f;
    private float _spaceImageTargetPos = 0.0f;
    private float _messageBoxTargetPos = 0.0f;
    private float _timer = 0.0f;
    private float _waterLiquidMultiply = 1.0f / 1379705.3f * 0.5f;
    private float _waterGasMultiply = 1.0f / 2.7f;
    private float _airPressureMultiply = 1.0f / 1013.25f;
    private float _iceMultiply = 1.0f / 29000f * 0.5f;
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
    /// ������ ȿ�� ����
    /// </summary>
    public void NoiseEffect()
    {
        _background.color = new Color(1.0f, 1.0f, 1.0f, _noiseAlpha);
        _messageBoxBackground.color = new Color(1.0f, 1.0f, 1.0f, _noiseAlpha);
    }


    /// <summary>
    /// �༺ �̹���, ���� ���, �޼��� ������ ��ǥ ��ġ ����
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
    /// �༺ ���� ����
    /// </summary>
    private void PlanetColour()
    {
        // ���� ��������
        float float0;
        int int0;
        uint uint0;

        // ��ǥ��
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.PhotoLifeStability]);
        if (_breathStability != int0)
        {
            _breathStability = int0;
            float0 = _breathStability * 0.01f;
            _planetMaterial.color = new Color(
                _landMin.r - _landColourGap.r * float0,
                _landMin.g - _landColourGap.g * float0,
                _landMin.b - _landColourGap.b * float0,
                1.0f);
        }

        // ���
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.WaterLiquid_PL]);
        if (_waterLiquid != int0)
        {
            _waterLiquid = int0;
            float0 = _waterLiquid * _waterLiquidMultiply;
            if (0.0f > float0)
            {
                float0 = 0.0f;
            }
            _planetMaterial.SetColor("_WaterColor", new Color(_oceanColour.r, _oceanColour.g, _oceanColour.b, float0));
        }

        // ����
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.WaterGas_PL]);
        if (_waterGas != int0)
        {
            _waterGas = int0;
            float0 = _waterGas * _waterGasMultiply;
            if (1.0f > float0)
            {
                _planetMaterial.SetColor("_CloudColor", new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, float0));
                _planetMaterial.SetFloat("_OverCloudyColor", 0.0f);
            }
            else if (3.0f > float0)
            {
                _planetMaterial.SetColor("_CloudColor", new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, 1.5f - float0 * 0.5f));
                _planetMaterial.SetFloat("_OverCloudyColor", float0 * 0.5f - 1.0f);
            }
            else
            {
                _planetMaterial.SetColor("_CloudColor", new Color(_cloudColour.r, _cloudColour.g, _cloudColour.b, 0.0f));
                _planetMaterial.SetFloat("_OverCloudyAlpha", 1.0f);
            }
        }

        // ���
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.TotalAirPressure_hPa]);
        if (_airPressure != int0)
        {
            _airPressure = int0;
            float0 = _airPressure * _airPressureMultiply;
            if (1.0f < float0)
            {
                float0 = 1.0f;
            }
            _imagePlanetAtmosphere.color = new Color(_imagePlanetAtmosphereBaseColor.r, _imagePlanetAtmosphereBaseColor.g, _imagePlanetAtmosphereBaseColor.b, float0);
            _planetMaterial.SetColor("_AtmosphereColour", new Color(_atmoshpereColour.r, _atmoshpereColour.g, _atmoshpereColour.b, float0));
        }

        // �߰� ����
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
            _planetMaterial.SetColor("_NightColour", new Color(_nightColour.r, _nightColour.g, _nightColour.b, float0));
        }

        // ����
        int0 = Mathf.RoundToInt(PlayManager.Instance[VariableFloat.WaterSolid_PL]);
        if (_waterSolid != int0)
        {
            _waterSolid = int0;
            float0 = _waterSolid * _iceMultiply;
            if (0.0f > float0)
            {
                float0 = 0.0f;
            }
            _planetMaterial.SetColor("_IceColor", new Color(_iceColour.r, _iceColour.g, _iceColour.b, float0));
        }
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // ��� rectTransform ����
        _backgroundTransform = _background.rectTransform;
        _messageBoxBackgroundTransform = _messageBoxBackground.rectTransform;

        // �¿� ��ư �ִϸ��̼� ��� ����
        _leftRightMultiply = (1.0f - _leftRightBtnMinBrightness) * 0.5f;
        _leftRightAdd = _leftRightBtnMinBrightness + _leftRightMultiply;

        // �¿� ��� ���� �ִϸ��̼� ��� ����
        _selectionMultiply = (_selectionMaxBrightness - _selectionMinBrightness) * 0.5f;
        _selectionAdd = _selectionMinBrightness + _selectionMultiply;

        // ���� ��
        _landColourGap = _landMin - _landMax;

        // ó�� ���� �� 0.5�� �� �༺ ���� ������Ʈ
        _timer = _planetColourUpdateTimer - 0.5f;

        // �༺ ���׸��� �����Ѵ�.
        _planetMaterial = Instantiate(_planet.material);

        // �༺ ������Ʈ�� ������ ���׸��� ���� �����ش�.
        _planet.material = _planetMaterial;

        // ���� ���� �����´�.
        _oceanColour = _planetMaterial.GetColor("_WaterColor");
        _iceColour = _planetMaterial.GetColor("_IceColor");
        _cloudColour = _planetMaterial.GetColor("_CloudColor");
        _atmoshpereColour = _planetMaterial.GetColor("_AtmosphereColour");
        _nightColour = _planetMaterial.GetColor("_NightColour");
    }


    private void Update()
    {
        // ���� ��������
        float float0;
        Vector3 vector3_0;

        #region ������ �ִϸ��̼�
        vector3_0 = _backgroundTransform.localPosition + new Vector3(0.0f, _noiseSpeed, 0.0f);
        if (Constants.HALF_CANVAS_HEIGHT < vector3_0.y)
        {
            vector3_0 -= new Vector3(0.0f, Constants.CANVAS_HEIGHT, 0.0f);
        }
        _backgroundTransform.localPosition = vector3_0;
        _messageBoxBackgroundTransform.localPosition = vector3_0 * Constants.MASSAGEBOX_HEIGHT_RATIO;
        #endregion

        #region ������ ��ο�����
        float0 = _background.color.r + (_noiseBrightness - _background.color.r) * Time.deltaTime * _noiseEffectSpeedmult;
        _background.color = new Color(float0, float0, float0, 1.0f);
        _messageBoxBackground.color = new Color(float0, float0, float0, _noiseAlpha);
        #endregion

        #region �¿� ��ư �ִϸ��̼�
        // ���� ����
        float0 = Mathf.Sin(_leftRightTimer) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[0].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[1].color = new Color(float0, float0, float0, 1.0f);

        float0 = Mathf.Sin(_leftRightTimer + Constants.HALF_PI) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[2].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[3].color = new Color(float0, float0, float0, 1.0f);

        float0 = Mathf.Sin(_leftRightTimer + Constants.PI) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[4].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[5].color = new Color(float0, float0, float0, 1.0f);

        // Ÿ�̸�
        _leftRightTimer += Time.deltaTime;
        if (_leftRightTimer > Constants.DOUBLE_PI)
        {
            _leftRightTimer -= Constants.DOUBLE_PI;
        }
        #endregion

        #region �¿� ��� ���� ������Ʈ �ִϸ��̼�
        float0 = Mathf.Sin(_leftRightTimer) * _selectionMultiply + _selectionAdd;
        _selection.color = new Color(_selectionColor.r, _selectionColor.g, _selectionColor.b, float0);
        #endregion

        #region �༺ �̹���, ���� ���, �޼��� ������ ��ġ
        _planetImage.localPosition += new Vector3((_planetImageTargetPos - _planetImage.localPosition.x) * Time.deltaTime, 0.0f, 0.0f);
        _spaceImage.localPosition += new Vector3((_spaceImageTargetPos - _spaceImage.localPosition.x) * Time.deltaTime, 0.0f, 0.0f);
        _messageBoxTransform.localPosition += new Vector3((_messageBoxTargetPos - _messageBoxTransform.localPosition.x) * _messageBoxSpeedmult * Time.deltaTime, 0.0f, 0.0f);
        #endregion

        #region ���籤�� �ִϸ��̼�
        _sun.Rotate(0.0f, _spinSpeedmult * Time.deltaTime, 0.0f);
        #endregion

        #region �༺ ����
        _timer += Time.deltaTime;
        if (_timer >= _planetColourUpdateTimer)
        {
            _timer -= _planetColourUpdateTimer;
            PlanetColour();
        }
        #endregion
    }
}
