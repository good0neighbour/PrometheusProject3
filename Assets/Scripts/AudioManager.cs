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

    // �����ϴ� �ʿ��� �̹� �� �Ҵ�
    private static AudioManager _instance = null;

    [Header("����")]
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

    [Header("����")]
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
                // �̹� �������ִ� ��� ���� ������ ���� �ı��Ѵ�.
                Destroy(value.gameObject);
            }
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// �Ҹ� ���
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
    /// ����� ä�� �Ҵ� �� ���
    /// </summary>
    private void UseChannel(AudioClip clip, float volume)
    {
        // ����� ä�ο� �Ҹ� ����ϰ� ���
        _channels[_currentChannel].clip = clip;
        _channels[_currentChannel].volume = volume;
        _channels[_currentChannel].Play();

        // ����� ä�� �ε��� ����
        ++_currentChannel;

        // ä�� ���� �ʰ� �� �ǵ��ƿ´�.
        if (_currentChannel == _numberOfChannel)
        {
            _currentChannel = 0;
        }
    }


    private void Awake()
    {
        // ä�� �迭 ����
        _channels = new AudioSource[_numberOfChannel];

        // 1���� �⺻
        _channels[0] = GetComponent<AudioSource>();
        _channels[1] = _audioSource;

        // �������� �߰� ����
        for (byte i = 2; i < _numberOfChannel; ++i)
        {
            AudioSource aS = Instantiate(_audioSource.gameObject, transform).GetComponent<AudioSource>();
            _channels[i] = aS;
        }
    }
}
