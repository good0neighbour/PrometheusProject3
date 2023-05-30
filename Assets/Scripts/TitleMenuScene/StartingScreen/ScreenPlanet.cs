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

    private short _direction = 1;
    private float _timer = 0.0f;
    private float _airMass = 0.0f;
    private float _waterVolume = 0.0f;
    private float _carbonRatio = 0.0f;
    private float _radius = 0.0f;
    private float _density = 0.0f;
    private float _distance = 0.0f;
    private bool _animationPreceed = false;
    private bool _buttonAnimation = false;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        // 사용 불가
        if (_animationPreceed || _buttonAnimation)
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // 화면 전환
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        TitleMenuManager.Instance.MoveScreen(TitleMenuManager.TextScreens.Main);
    }


    public void BtnRefresh()
    {
        // 사용 불가
        if (_animationPreceed || _buttonAnimation)
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        FadeOut();
    }


    public void BtnNext()
    {
        // 사용 불가
        if (_animationPreceed || _buttonAnimation)
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 값 전달
        GameManager.Instance.AirMass = _airMass;
        GameManager.Instance.WaterVolume = _waterVolume;
        GameManager.Instance.CarbonRatio = _carbonRatio;
        GameManager.Instance.Radius = _radius;
        GameManager.Instance.Density = _density;
        GameManager.Instance.Distance = _distance;

        // 화면 전환
        gameObject.SetActive(false);
        _nextScreen.SetActive(true);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 구 이미지, 정보 표시 페이드 인
    /// </summary>
    private void FadeIn()
    {
        _animationPreceed = true;
        _timer = 0.0f;
        _direction = 1;
    }


    /// <summary>
    /// 구 이미지, 정보 표시 페이드 아웃
    /// </summary>
    private void FadeOut()
    {
        _animationPreceed = true;
        _timer = 2.0f;
        _direction = -1;
    }


    /// <summary>
    /// 텍스트 색상 설정
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
    /// 무작위 값 설정
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
        _densityText.text = $"{_density.ToString("F2")}g/cm³";
        _distanceText.text = $"{_distance.ToString("F2")}AU";
    }


    private void Awake()
    {
        // 처음 시작 시 무작위 값
        RandomValues();
    }


    private void OnEnable()
    {
        // 처음 상태
        SetTextColour(0.0f);
        _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        for (byte i = 0; i < _btnTexts.Length; ++i)
        {
            _btnTexts[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        // 화면 페이드 인
        FadeIn();
        _buttonAnimation = true;
    }


    private void Update()
    {
        if (_animationPreceed)
        {
            if (1.0f >= _timer)
            {
                _timer += Time.deltaTime * _direction;
                if (0.0f >= _timer)
                {
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    // 새로 고침
                    RandomValues();

                    // 다시 페이드 인
                    _direction = 1;
                }
                else if (1.0f <= _timer)
                {
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                else
                {
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, _timer);
                }
            }
            else
            {
                _timer += Time.deltaTime * _direction;
                if (1.0f >= _timer)
                {
                    SetTextColour(0.0f);
                }
                else if (2.0f <= _timer)
                {
                    SetTextColour(1.0f);
                    _sphereWireImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                    // 애니메이션 종료
                    _animationPreceed = false;
                }
                else
                {
                    SetTextColour(_timer - 1.0f);
                }
            }

            return;
        }

        if (_buttonAnimation)
        {
            _timer += Time.deltaTime;
            for (byte i = 0; i < _btnTexts.Length; ++i)
            {
                _btnTexts[i].color = new Color(1.0f, 1.0f, 1.0f, _timer - 2.0f);
            }

            if (3.0f <= _timer)
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
