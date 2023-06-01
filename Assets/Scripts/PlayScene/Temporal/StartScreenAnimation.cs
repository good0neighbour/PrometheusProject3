using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenAnimation : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private RectTransform _backgroundBar = null;
    [SerializeField] private TMP_Text _loadingText = null;
    [SerializeField] private GameObject _startButton = null;
    [SerializeField] private GameObject[] _enableOnStart = null;

    private TMP_Text _startText = null;
    private Image _startScreen = null;
    private float _backBarHeightRatio = 0.3f;
    private float _backBarCos = 0.0f;
    private float _timer = 0.0f;
    private bool _isProceeding = true;



    /* ==================== Public Methods ==================== */

    public void BtnStartPlay()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ���� ���
        AudioManager.Instance.PlayThemeMusic(ThemeType.Play);

        // ���� ȭ���� ���̻� ������� �ʱ� ������ �ı��Ѵ�.
        Destroy(gameObject);

        // ���� �� Ȱ��ȭ�� ��.
        for (byte i = 0; i < _enableOnStart.Length; ++i)
        {
            _enableOnStart[i].SetActive(true);
        }

        // ������ ȿ��
        AnimationManager.Instance.NoiseEffect();

        // �÷��� ������ ��ȯ
        PlayManager.Instance.IsPlaying = true;

        // ���� ����
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // �޼���
        if (GameManager.Instance.IsNewGame)
        {
            MessageBox.Instance.EnqueueMessage(Language.Instance[
                "����� �׶����� ������Ʈ�� �Ѱ��� �������� ���ߵ� �� ����� �ΰ����� ���Դϴ�. �༺�� ȯ��� ������ ����ȭ�ϰų� �ٸ� ���� ����� ����ϴ� ���� ����� ��ǥ�Դϴ�."
                ]);
        }
        else
        {
            MessageBox.Instance.EnqueueMessage(Language.Instance[
                "�ӹ��� �簳�մϴ�."
                ]);
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ��� ����
        _loadingText.text = Language.Instance["�ҷ����� ��"];

        // �̹��� ������Ʈ ��������
        _startScreen = GetComponent<Image>();

        // �Ʒ� ���� ���� ������ �ϱ� ���� ������ ����
        _backBarCos = ((Constants.CANVAS_HEIGHT * _backBarHeightRatio) * 0.5f - Constants.HALF_CANVAS_HEIGHT) * 0.5f;

        // ���� ������ ��Ȱ��ȭ�� ��.
        for (byte i = 0; i < _enableOnStart.Length; ++i)
        {
            _enableOnStart[i].SetActive(false);
        }

        // ����
        _startText = _startButton.GetComponentInChildren<TMP_Text>();

        // ��Ʈ ����
        _startText.font = Language.Instance.GetFontAsset();
    }

    private void Update()
    {
        // �ִϸ��̼� ���� ������ Ȯ��
        if (!_isProceeding)
        {
#if PLATFORM_STANDALONE_WIN
            // ����Ű ����
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            {
                BtnStartPlay();
            }
#endif
            // PC, ����� ����
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnStartPlay();
            }
            return;
        }

        // �ҷ����� �� �ؽ�Ʈ ��ο�����
        if (_timer < Constants.PI)
        {
            _loadingText.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Cos(_timer) * 0.5f + 0.5f);
        }
        // ��ü ��� ��ο�����
        else if (_timer < Constants.DOUBLE_PI)
        {
            _startScreen.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Cos((_timer - Constants.PI)) * 0.5f + 0.5f);
        }
        // ��� �� �����̱�
        else if (_timer < Constants.TRIPLE_PI)
        {
            _backgroundBar.localPosition = new Vector3(0.0f, Mathf.Cos(_timer) * _backBarCos + _backBarCos, 0.0f);
        }
        // ���� ��ư Ȱ��ȭ
        else
        {
            if (GameManager.Instance.IsNewGame)
            {
                _startText.text = Language.Instance["����"];
            }
            else
            {
                _startText.text = Language.Instance["���"];
            }
            _startButton.SetActive(true);

            // �ִϸ��̼� ��
            _isProceeding = false;
            return;
        }

        // �ð� ���
        _timer += Time.deltaTime * Constants.STARTING_TEXT_SPEEDMULT;
    }
}
