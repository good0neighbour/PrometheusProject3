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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 배경 음악 재생
        AudioManager.Instance.PlayThemeMusic(ThemeType.Play);

        // 시작 화면은 더이상 사용하지 않기 때문에 파괴한다.
        Destroy(gameObject);

        // 시작 시 활성화할 것.
        for (byte i = 0; i < _enableOnStart.Length; ++i)
        {
            _enableOnStart[i].SetActive(true);
        }

        // 노이즈 효과
        AnimationManager.Instance.NoiseEffect();

        // 플레이 중으로 전환
        PlayManager.Instance.IsPlaying = true;

        // 게임 진행
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // 메세지
        if (GameManager.Instance.IsNewGame)
        {
            MessageBox.Instance.EnqueueMessage(Language.Instance[
                "당신은 테라포밍 프로젝트의 총괄을 목적으로 개발된 한 기업의 인공지능 모델입니다. 행성의 환경과 문명을 안정화하거나 다른 경쟁 기업을 잠식하는 것이 당신의 목표입니다."
                ]);
        }
        else
        {
            MessageBox.Instance.EnqueueMessage(Language.Instance[
                "임무를 재개합니다."
                ]);
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 언어 적용
        _loadingText.text = Language.Instance["불러오는 중"];

        // 이미지 컴포넌트 가져오기
        _startScreen = GetComponent<Image>();

        // 아래 값은 식을 간단히 하기 위해 음수로 유지
        _backBarCos = ((Constants.CANVAS_HEIGHT * _backBarHeightRatio) * 0.5f - Constants.HALF_CANVAS_HEIGHT) * 0.5f;

        // 시작 전에는 비활성화할 것.
        for (byte i = 0; i < _enableOnStart.Length; ++i)
        {
            _enableOnStart[i].SetActive(false);
        }

        // 참조
        _startText = _startButton.GetComponentInChildren<TMP_Text>();

        // 폰트 변경
        _startText.font = Language.Instance.GetFontAsset();
    }

    private void Update()
    {
        // 애니메이션 진행 중인지 확인
        if (!_isProceeding)
        {
#if PLATFORM_STANDALONE_WIN
            // 단축키 동작
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
            {
                BtnStartPlay();
            }
#endif
            // PC, 모바일 공용
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                BtnStartPlay();
            }
            return;
        }

        // 불러오는 중 텍스트 어두워지기
        if (_timer < Constants.PI)
        {
            _loadingText.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Cos(_timer) * 0.5f + 0.5f);
        }
        // 전체 배경 어두워지기
        else if (_timer < Constants.DOUBLE_PI)
        {
            _startScreen.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Cos((_timer - Constants.PI)) * 0.5f + 0.5f);
        }
        // 배경 바 움직이기
        else if (_timer < Constants.TRIPLE_PI)
        {
            _backgroundBar.localPosition = new Vector3(0.0f, Mathf.Cos(_timer) * _backBarCos + _backBarCos, 0.0f);
        }
        // 시작 버튼 활성화
        else
        {
            if (GameManager.Instance.IsNewGame)
            {
                _startText.text = Language.Instance["시작"];
            }
            else
            {
                _startText.text = Language.Instance["계속"];
            }
            _startButton.SetActive(true);

            // 애니메이션 끝
            _isProceeding = false;
            return;
        }

        // 시간 경과
        _timer += Time.deltaTime * Constants.STARTING_TEXT_SPEEDMULT;
    }
}
