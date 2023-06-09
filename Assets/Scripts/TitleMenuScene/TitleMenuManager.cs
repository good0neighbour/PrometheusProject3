using System.Text;
using UnityEngine;
using TMPro;

public class TitleMenuManager : MonoBehaviour
{
    public enum TextScreens
    {
        Main,
        Start,
        Settings,
        FPS,
        SoundVolume,
        NewStartConfirm,
        TextScreenEnd
    }



    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _audioMamagerPrefab = null;
    [SerializeField] private GameObject _planetScreen = null;
    [SerializeField] private GameObject _loadingScreen = null;
    [SerializeField] private GameObject _languageScreen = null;
    [SerializeField] private TMP_Text _textScreen = null;
    [SerializeField] private TMP_Text _warningText = null;
    [SerializeField] private TMP_Text[] _btnTexts = null;

    private TextScreenBase[] _states = new TextScreenBase[(int)TextScreens.TextScreenEnd];
    private string[] _texts = null;
    private TextScreenBase _currentScreen = null;
    private StringBuilder _textScreenBuilder = new StringBuilder();
    private byte _phase = 0;
    private byte _textLength = 0;
    private float _timer = 0;
    private bool _screenAnimation = false;
    private bool _firstStart = false;

    public static TitleMenuManager Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 텍스트 화면 업데이트
    /// </summary>
    public void SetTextScreen(params string[] texts)
    {
        _texts = texts;
        _textLength = (byte)_texts.Length;
        _phase = 0;
        _timer = 0.0f;
        _screenAnimation = true;
    }


    /// <summary>
    /// 메뉴 화면 이동
    /// </summary>
    public void MoveScreen(TextScreens screen)
    {
        // 화면 비운다.
        _textScreen.text = null;
        _textScreenBuilder.Clear();
        SetButtons();

        // 다음 화면
        _currentScreen = _states[(int)screen];
        _currentScreen.Execute();
    }


    /// <summary>
    /// 버튼 이름 설정
    /// </summary>
    public void SetButtons(params string[] buttonNames)
    {
        byte i;
        for (i = 0; i < buttonNames.Length; ++i)
        {
            _btnTexts[i].text = buttonNames[i];
        }
        for (; i < _btnTexts.Length; ++i)
        {
            _btnTexts[i].text = null;
        }
    }


    /// <summary>
    /// 행성 화면 활성화
    /// </summary>
    public void PlanetScreenEnable()
    {
        gameObject.SetActive(false);
        _planetScreen.SetActive(true);
    }


    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        Language.OnLanguageChange = null;
        Destroy(gameObject);
        _loadingScreen.SetActive(true);
    }


    public void BtnMenuButton(int index)
    {
        _currentScreen.ButtonAct((byte)index);
    }


    /// <summary>
    /// 언어 선택 화면 활성화
    /// </summary>
    public void LanguageScreenEnable()
    {
        gameObject.SetActive(false);
        _languageScreen.SetActive(true);
    }


    public void BtnLanguageSelect(int index)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 언어 불러오기
        Language.Instance.LoadLangeage((LanguageType)index);

        // 언어 선택 창 닫기
        _languageScreen.SetActive(false);
        gameObject.SetActive(true);

        // 처음 실행이어서 언어 선택한 것인지 확인
        if (GameManager.Instance.IsApplicationFirstStarted)
        {
            _firstStart = true;
            _timer = -Constants.PI;
        }
        else
        {
            MoveScreen(TextScreens.Main);
        }

        // 설정 저장
        GameManager.Instance.SaveSettings();
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 주 화면으로 들어올 때마다 AudioManager를 생성하고 파괴하는 것을 방지한다.
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioMamagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }

        // 현재 씬에서 모든 AutoTranslation을 찾는다.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>(true);

        // 모든 AutoTranslation을 준비시킨다.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }

        // 초당 프레임 수 제한
        Application.targetFrameRate = GameManager.Instance.TargetFrameRate;

        // 화면 참조
        _states[(int)TextScreens.Main] = new TextScreenMain();
        _states[(int)TextScreens.Start] = new TextScreenStart();
        _states[(int)TextScreens.Settings] = new TextScreenSettings();
        _states[(int)TextScreens.FPS] = new TextScreenFPS();
        _states[(int)TextScreens.SoundVolume] = new TextScreenSoundVolume();
        _states[(int)TextScreens.NewStartConfirm] = new TextScreenNewStartConfirm();

        // 배경 음악 재생
        AudioManager.Instance.PlayThemeMusic(ThemeType.TitleMenu);

        // 처음 화면
        switch (GameManager.Instance.CurrentLanguage)
        {
            // 언어 설정한 적 없을 때
            case LanguageType.End:
                {
                    LanguageScreenEnable();
                }
                break;

            // 언어 설정 돼있을 때
            default:
                {
                    // 언어 불러온다.
                    Language.Instance.LoadLangeage(GameManager.Instance.CurrentLanguage);

                    // 처음 실행한 경우 경고 문구 표시
                    if (GameManager.Instance.IsApplicationFirstStarted)
                    {
                        _firstStart = true;
                        _timer = -Constants.PI;
                    }
                    else
                    {
                        Destroy(_warningText.gameObject);
                        MoveScreen(TextScreens.Main);
                    }
                }
                break;
        }
    }


    private void Update()
    {
        if (_screenAnimation)
        {
            if (Constants.TEXT_SCREEN_SPEEDMULT <= _timer)
            {
                if (_textLength > _phase)
                {
                    // 소리 재생
                    AudioManager.Instance.PlayAuido(AudioType.Touch);

                    // 화면 차례대로 표시
                    _textScreenBuilder.Append(_texts[_phase]);
                    _textScreen.text = _textScreenBuilder.ToString();
                    ++_phase;
                    _timer -= Constants.TEXT_SCREEN_SPEEDMULT;
                }
                else
                {
                    // 표시 완료
                    _currentScreen.SetButtons();
                    _screenAnimation = false;
                }
            }
            else
            {
                // 시간 경과
                _timer += Time.deltaTime;
            }

            //여기서 함수 종료
            return;
        }
        else if (_firstStart)
        {
            if (0 >= _timer)
            {
                // 밝아지는 중
                _warningText.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Cos(_timer) * 0.5f + 0.5f);
            }
            else if (Constants.DOUBLE_PI < _timer)
            {
                // 더이상 필요 없음
                Destroy(_warningText.gameObject);
                _firstStart = false;
                GameManager.Instance.IsApplicationFirstStarted = false;
                MoveScreen(TextScreens.Main);
            }
            else if (Constants.PI < _timer)
            {
                // 어두워지는 중
                _warningText.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Cos(_timer - Constants.PI) * 0.5f + 0.5f);
            }

            // 시간 경과
            _timer += Time.deltaTime * Constants.STARTING_TEXT_SPEEDMULT;

            // 여기서 함수 종료
            return;
        }

        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _currentScreen.OnEscapeBtn();
        }
    }
}
