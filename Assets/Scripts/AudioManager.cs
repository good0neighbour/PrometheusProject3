using UnityEngine;

public enum AudioType
{
    Touch
}

public class AudioManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    // �����ϴ� �ʿ��� �̹� �� �Ҵ�
    public static AudioManager Instance = null;

    [Header("����")]
    [Range(2, 255)]
    [SerializeField] private byte _numberOfChannel = 8;

    [Header("Ŭ��")]
    [SerializeField] private AudioClip _touchClip = null;
    [SerializeField] private float __touchVolume = 1.0f;

    [Header("����")]
    [SerializeField] private AudioSource _audioSource = null;

    private AudioSource[] _channels = null;
    private byte _currentChannel = 0;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// �̱��� ����
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
        Debug.Log("ä�� �迭 ����");

        // 1���� �⺻
        _channels[0] = GetComponent<AudioSource>();
        _channels[1] = _audioSource;
        Debug.Log("1���� �⺻");

        // �������� �߰� ����
        for (byte i = 2; i < _numberOfChannel; ++i)
        {
            AudioSource aS = Instantiate(_audioSource.gameObject, transform).GetComponent<AudioSource>();
            _channels[i] = aS;
            Debug.Log("�������� �߰� ����");
        }
    }
}
