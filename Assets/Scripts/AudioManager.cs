using UnityEngine;

public enum AudioType
{
    Touch
}

public class AudioManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // 생성하는 쪽에서 이미 값 할당
    public static AudioManager Instance = null;

    [Header("설정")]
    [Range(2, 255)]
    [SerializeField] private byte _numberOfChannel = 8;

    [Header("클릭")]
    [SerializeField] private AudioClip _touchClip = null;
    [SerializeField] private float __touchVolume = 1.0f;

    [Header("참조")]
    [SerializeField] private AudioSource _audioSource = null;

    private AudioSource[] _channels = null;
    private byte _currentChannel = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 미구현 상태
    /// </summary>
    public void PlayAuido(AudioType audio)
    {
        switch (audio)
        {
            case AudioType.Touch:
                UseChannel(_touchClip, __touchVolume);
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
        Debug.Log("채널 배열 생성");

        // 1개는 기본
        _channels[0] = GetComponent<AudioSource>();
        _channels[1] = _audioSource;
        Debug.Log("1개는 기본");

        // 나머지는 추가 생성
        for (byte i = 2; i < _numberOfChannel; ++i)
        {
            AudioSource aS = Instantiate(_audioSource.gameObject, transform).GetComponent<AudioSource>();
            _channels[i] = aS;
            Debug.Log("나머지는 추가 생성");
        }
    }
}
