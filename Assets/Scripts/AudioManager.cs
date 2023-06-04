using UnityEngine;

public enum AudioType
{
    Touch,
    Select,
    Failed,
    Alert,
    Unable,
    Income,
    Show,
    TakeOff
}

public enum ThemeType
{
    TitleMenu,
    Play,
    None
}

public class AudioManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // 생성하는 쪽에서 이미 값 할당
    private static AudioManager _instance = null;

    [Header("설정")]
    [Range(1, byte.MaxValue)]
    [SerializeField] private byte _numberOfChannel = 8;

    [Header("Touch")]
    [SerializeField] private AudioClip _touchClip = null;
    [SerializeField] private float _touchVolume = 1.0f;

    [Header("Select")]
    [SerializeField] private AudioClip _selectClip = null;
    [SerializeField] private float _selectVolume = 1.0f;

    [Header("Failed")]
    [SerializeField] private AudioClip _failedClip = null;
    [SerializeField] private float _failedVolume = 1.0f;

    [Header("Alert")]
    [SerializeField] private AudioClip _alertClip = null;
    [SerializeField] private float _alertVolume = 1.0f;

    [Header("Unable")]
    [SerializeField] private AudioClip _unableClip = null;
    [SerializeField] private float _unableVolume = 1.0f;

    [Header("Income")]
    [SerializeField] private AudioClip _incomeClip = null;
    [SerializeField] private float _incomeVolume = 1.0f;

    [Header("Show")]
    [SerializeField] private AudioClip _showClip = null;
    [SerializeField] private float _showVolume = 1.0f;

    [Header("TakeOff")]
    [SerializeField] private AudioClip _takeOffClip = null;
    [SerializeField] private float _takeOffVolume = 1.0f;

    [Header("Theme")]
    [SerializeField] private AudioClip _titleMenuTheme = null;
    [SerializeField] private AudioClip _playTheme = null;

    [Header("참조")]
    [SerializeField] private AudioSource _audioSource = null;

    private AudioSource[] _channels = null;
    private AudioSource _themeChanel = null;
    private ThemeType _reserve = ThemeType.None;
    private byte _currentChannel = 0;
    private bool _themeMusicFadeOut = false;

    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
        set
        {
#if UNITY_EDITOR
            if (null == _instance)
            {
#endif
                _instance = value;
#if UNITY_EDITOR
            }
            else if (value != _instance)
            {
                // 이미 생성돼있는 경우 새로 생성한 것을 파괴한다.
                Destroy(value.gameObject);
                Debug.LogError("이미 생성된 AudioManager.");
            }
#endif
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 소리 재생
    /// </summary>
    public void PlayAuido(AudioType audio)
    {
        switch (audio)
        {
            case AudioType.Touch:
                UseChannel(_touchClip, _touchVolume);
                return;

            case AudioType.Select:
                UseChannel(_selectClip, _selectVolume);
                return;

            case AudioType.Failed:
                UseChannel(_failedClip, _failedVolume);
                return;

            case AudioType.Alert:
                UseChannel(_alertClip, _alertVolume);
                return;

            case AudioType.Unable:
                UseChannel(_unableClip, _unableVolume);
                return;

            case AudioType.Income:
                UseChannel(_incomeClip, _incomeVolume);
                return;

            case AudioType.Show:
                UseChannel(_showClip, _showVolume);
                return;

            case AudioType.TakeOff:
                UseChannel(_takeOffClip, _takeOffVolume);
                return;
        }
    }


    /// <summary>
    /// 배경 음악 재생
    /// </summary>
    public void PlayThemeMusic(ThemeType theme)
    {
        // 재생 중일 때 예약
        if (_themeChanel.isPlaying)
        {
            _reserve = theme;
            return;
        }

        switch (theme)
        {
            case ThemeType.TitleMenu:
                _themeChanel.clip = _titleMenuTheme;
                break;

            case ThemeType.Play:
                _themeChanel.clip = _playTheme;
                break;
        }

        _themeChanel.volume = Constants.THEME_VOLUME * GameManager.Instance.SoundVolume;
        _themeChanel.Play();
        _reserve = ThemeType.None;
    }


    /// <summary>
    /// 배경 음악 서서히 종료
    /// </summary>
    public void FadeOutThemeMusic()
    {
        _themeMusicFadeOut = true;
    }


    /// <summary>
    /// 배경 음악 즉시 종료
    /// </summary>
    public void ForceStopThemeMusic()
    {
        _themeChanel.Stop();
    }


    /// <summary>
    /// 음량 설정이 변경됐을 때
    /// </summary>
    public void OnSoundVolumeChanged()
    {
        _themeChanel.volume = Constants.THEME_VOLUME * GameManager.Instance.SoundVolume;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 오디오 채널 할당 후 재생
    /// </summary>
    private void UseChannel(AudioClip clip, float volume)
    {
        // 오디오 채널에 소리 등록하고 재생
        _channels[_currentChannel].clip = clip;
        _channels[_currentChannel].volume = volume * GameManager.Instance.SoundVolume;
        _channels[_currentChannel].Play();

        // 사용할 채널 인덱스 증가
        ++_currentChannel;

        // 채널 개수 초과 시 되돌아온다.
        if (_currentChannel == _numberOfChannel)
        {
            _currentChannel = 0;
        }
    }


    private void Awake()
    {
        // 채널 배열 생성
        _channels = new AudioSource[_numberOfChannel];

        // 1개는 기본
        _themeChanel = GetComponent<AudioSource>();
        _channels[0] = _audioSource;

        // 나머지는 추가 생성
        for (byte i = 1; i < _numberOfChannel; ++i)
        {
            AudioSource aS = Instantiate(_audioSource.gameObject, transform).GetComponent<AudioSource>();
            _channels[i] = aS;
        }
    }


    private void Update()
    {
        if (_themeMusicFadeOut)
        {
            _themeChanel.volume -= Time.deltaTime * Constants.THEME_FADE_OUT_SPEEDMULT;
            if (0.0f >= _themeChanel.volume)
            {
                _themeChanel.Stop();
                _themeMusicFadeOut = false;
                if (ThemeType.None != _reserve)
                {
                    PlayThemeMusic(_reserve);
                }
            }
        }
    }
}
