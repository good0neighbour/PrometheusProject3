using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenPlanet : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private Image _sphereWireImage = null;
    [SerializeField] private TMP_Text _airMassText = null;
    [SerializeField] private TMP_Text _waterVolumeText = null;
    [SerializeField] private TMP_Text _carbonRatioText = null;
    [SerializeField] private TMP_Text _radiusText = null;
    [SerializeField] private TMP_Text _densityText = null;
    [SerializeField] private TMP_Text _distanceText = null;
    [SerializeField] private GameObject _previousScreen = null;
    [SerializeField] private GameObject _nextScreen = null;
    [SerializeField] private TMP_Text[] _btnTexts = null;
    [SerializeField] private TMP_Text[] _infoTexts = null;

    private float _timer = 0.0f;
    private float _airMass = 0.0f;
    private float _waterVolume = 0.0f;
    private float _carbonRatio = 0.0f;
    private float _radius = 0.0f;
    private float _density = 0.0f;
    private float _distance = 0.0f;
    private bool _animationPreceed = false;
    private bool _buttonAnimation = false;
    private bool _fadeIn = false;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // ��� �Ұ�
        if (_animationPreceed || _buttonAnimation)
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        TitleMenuManager.Instance.MoveScreen(TitleMenuManager.TextScreens.Main);
    }


    public void BtnRefresh()
    {
        // ��� �Ұ�
        if (_animationPreceed || _buttonAnimation)
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        FadeOut();
    }


    public void BtnNext()
    {
        // ��� �Ұ�
        if (_animationPreceed || _buttonAnimation)
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �� ����
        GameManager.Instance.AirMass = _airMass;
        GameManager.Instance.WaterVolume = _waterVolume;
        GameManager.Instance.CarbonRatio = _carbonRatio;
        GameManager.Instance.Radius = _radius;
        GameManager.Instance.Density = _density;
        GameManager.Instance.Distance = _distance;

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _nextScreen.SetActive(true);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// �� �̹���, ���� ǥ�� ���̵� ��
    /// </summary>
    private void FadeIn()
    {
        _animationPreceed = true;
        _fadeIn = true;
        _timer = Constants.PI;
    }


    /// <summary>
    /// �� �̹���, ���� ǥ�� ���̵� �ƿ�
    /// </summary>
    private void FadeOut()
    {
        _animationPreceed = true;
        _fadeIn = false;
        _timer = 0.0f;
    }


    /// <summary>
    /// �ؽ�Ʈ ���� ����
    /// </summary>
    private void SetTextColour(float alpha)
    {
        _airMassText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        _waterVolumeText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        _carbonRatioText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        _radiusText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        _densityText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        _distanceText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        for (byte i = 0; i < _infoTexts.Length; ++i)
        {
            _infoTexts[i].color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }


    /// <summary>
    /// ������ �� ����
    /// </summary>
    private void RandomValues()
    {
        _airMass = Random.Range(100.0f, 3000.0f);
        _waterVolume = Random.Range(10000.0f, 1000000.0f);
        _carbonRatio = Random.Range(100.0f, 900.0f);
        _radius = Random.Range(5000.0f, 7000.0f);
        _density = Random.Range(5.0f, 6.0f);
        _distance = Random.Range(0.9f, 1.1f);

        _airMassText.text = $"{_airMass.ToString("F2")}Tt";
        _waterVolumeText.text = $"{(_waterVolume * Constants.E_3).ToString("F2")}EL";
        _carbonRatioText.text = $"{_carbonRatio.ToString("F2")}ppm";
        _radiusText.text = $"{_radius.ToString("F2")}km";
        _densityText.text = $"{_density.ToString("F2")}g/cm��";
        _distanceText.text = $"{_distance.ToString("F2")}AU";
    }


    private void Awake()
    {
        // ó�� ���� �� ������ ��
        RandomValues();
    }


    private void OnEnable()
    {
        // ó�� ����
        SetTextColour(0.0f);
        _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        for (byte i = 0; i < _btnTexts.Length; ++i)
        {
            _btnTexts[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        // ȭ�� ���̵� ��
        FadeIn();
        _buttonAnimation = true;
    }


    private void Update()
    {
        if (_animationPreceed)
        {
            _timer += Time.deltaTime * Constants.STARTING_TEXT_SPEEDMULT;

            if (_fadeIn)
            {
                if (Constants.DOUBLE_PI >= _timer)
                {
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Cos(_timer) * 0.5f + 0.5f);
                }
                else if (Constants.TRIPLE_PI >= _timer)
                {
                    SetTextColour(Mathf.Cos(_timer - Constants.PI) * 0.5f + 0.5f);
                }
                else
                {
                    SetTextColour(1.0f);
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    _animationPreceed = false;
                    _timer = 0.0f;
                }
            }
            else
            {
                if (Constants.PI >= _timer)
                {
                    SetTextColour(Mathf.Cos(_timer) * 0.5f + 0.5f);
                }
                else if (Constants.DOUBLE_PI >= _timer)
                {
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Cos(_timer - Constants.PI) * 0.5f + 0.5f);
                }
                else
                {
                    SetTextColour(0.0f);
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    // ���� ��ħ
                    RandomValues();

                    // �ٽ� ���̵� ��
                    _fadeIn = true;
                    _timer = Constants.PI;
                }
            }

            return;
        }

        if (_buttonAnimation)
        {
            _timer += Time.deltaTime;
            for (byte i = 0; i < _btnTexts.Length; ++i)
            {
                _btnTexts[i].color = new Color(1.0f, 1.0f, 1.0f, _timer);
            }

            if (1.0f <= _timer)
            {
                for (byte i = 0; i < _btnTexts.Length; ++i)
                {
                    _btnTexts[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                _buttonAnimation = false;
            }
        }
    }
}
