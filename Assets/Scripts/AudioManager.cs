using UnityEngine;

public enum AudioType
{
    Touch,
    Select,
    Failed
}

public class AudioManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // 생성하는 쪽에서 이미 값 할당
    private static AudioManager _instance = null;

    [Header("설정")]
    [Range(2, byte.MaxValue)]
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

    [Header("참조")]
    [SerializeField] private AudioSource _audioSource = null;

    private AudioSource[] _channels = null;
    private byte _currentChannel = 0;

    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            if (null == _instance)
            {
                _instance = value;
            }
            else if (value != _instance)
            {
                // 이미 생성돼있는 경우 새로 생성한 것을 파괴한다.
                Destroy(value.gameObject);
            }
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
        }
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 오디오 채널 할당 후 재생
    /// </summary>
    private void UseChannel(AudioClip clip, float volume)
    {
        // 오디오 채널에 소리 등록하고 재생
        _channels[_currentChannel].clip = clip;
        _channels[_currentChannel].volume = volume;
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
        _channels[0] = GetComponent<AudioSource>();
        _channels[1] = _audioSource;

        // 나머지는 추가 생성
        for (byte i = 2; i < _numberOfChannel; ++i)
        {
            AudioSource aS = Instantiate(_audioSource.gameObject, transform).GetComponent<AudioSource>();
            _channels[i] = aS;
        }
    }
}
