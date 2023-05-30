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
    [SerializeField] private TMP_Text[] _btnTexts = null;

    private TextScreenBase[] _states = new TextScreenBase[(int)TextScreens.TextScreenEnd];
    private string[] _texts = null;
    private TextScreenBase _currentScreen = null;
    private StringBuilder _textScreenBuilder = new StringBuilder();
    private byte _phase = 0;
    private byte _textLength = 0;
    private float _timer = 0;
    private bool _screenAnimation = false;

    public static TitleMenuManager Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// �ؽ�Ʈ ȭ�� ������Ʈ
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
    /// �޴� ȭ�� �̵�
    /// </summary>
    public void MoveScreen(TextScreens screen)
    {
        // ȭ�� ����.
        _textScreen.text = null;
        _textScreenBuilder.Clear();
        SetButtons();

        // ���� ȭ��
        _currentScreen = _states[(int)screen];
        _currentScreen.Execute();
    }


    /// <summary>
    /// ��ư �̸� ����
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
    /// �༺ ȭ�� Ȱ��ȭ
    /// </summary>
    public void PlanetScreenEnable()
    {
        gameObject.SetActive(false);
        _planetScreen.SetActive(true);
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    public void GameStart()
    {
        Language.OnLanguageChange = null;
        GameManager.Instance.IsThereSavedGame = false;
        GameManager.Instance.SaveSettings();
        Destroy(gameObject);
        _loadingScreen.SetActive(true);
    }


    public void BtnMenuButton(int index)
    {
        _currentScreen.ButtonAct((byte)index);
    }


    /// <summary>
    /// ��� ���� ȭ�� Ȱ��ȭ
    /// </summary>
    public void LanguageScreenEnable()
    {
        gameObject.SetActive(false);
        _languageScreen.SetActive(true);
    }


    public void BtnLanguageSelect(int index)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� �ҷ�����
        Language.Instance.LoadLangeage((LanguageType)index);

        // ��� ���� â �ݱ�
        _languageScreen.SetActive(false);
        gameObject.SetActive(true);
        MoveScreen(TextScreens.Main);

        // ���� ����
        GameManager.Instance.SaveSettings();
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // �� ȭ������ ���� ������ AudioManager�� �����ϰ� �ı��ϴ� ���� �����Ѵ�.
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioMamagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }

        // ���� ������ ��� AutoTranslation�� ã�´�.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>(true);

        // ��� AutoTranslation�� �غ��Ų��.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }

        // �ʴ� ������ �� ����
        Application.targetFrameRate = GameManager.Instance.TargetFrameRate;

        // ȭ�� ����
        _states[(int)TextScreens.Main] = new TextScreenMain();
        _states[(int)TextScreens.Start] = new TextScreenStart();
        _states[(int)TextScreens.Settings] = new TextScreenSettings();
        _states[(int)TextScreens.FPS] = new TextScreenFPS();
        _states[(int)TextScreens.SoundVolume] = new TextScreenSoundVolume();
        _states[(int)TextScreens.NewStartConfirm] = new TextScreenNewStartConfirm();

        // ��� ���� ���
        AudioManager.Instance.PlayThemeMusic(ThemeType.TitleMenu);

        // ó�� ȭ��
        switch (GameManager.Instance.CurrentLanguage)
        {
            // ��� ������ �� ���� ��
            case LanguageType.End:
                LanguageScreenEnable();
                break;

            // ��� ���� ������ ��
            default:
                Language.Instance.LoadLangeage(GameManager.Instance.CurrentLanguage);
                MoveScreen(TextScreens.Main);
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
                    // �Ҹ� ���
                    AudioManager.Instance.PlayAuido(AudioType.Touch);

                    _textScreenBuilder.Append(_texts[_phase]);
                    _textScreen.text = _textScreenBuilder.ToString();
                    ++_phase;
                    _timer -= Constants.TEXT_SCREEN_SPEEDMULT;
                }
                else
                {
                    _currentScreen.SetButtons();
                    _screenAnimation = false;
                    return;
                }
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }

        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _currentScreen.OnEscapeBtn();
        }
    }
}
