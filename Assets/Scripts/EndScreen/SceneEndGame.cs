using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneEndGame : MonoBehaviour
{
    [SerializeField] private float _showTimer = 0.5f;
    [SerializeField] private TMP_Text _titleText = null;
    [SerializeField] private TMP_Text _dateText = null;
    [SerializeField] private TMP_Text _societyText = null;
    [SerializeField] private TMP_Text _situationText = null;
    [SerializeField] private GameObject _backButton = null;

    private float _timer = 0.0f;
    private byte _phase = 0;


    public void BtnBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 배경음악 종료
        if (GameManager.Instance.IsGameWin)
        {
            AudioManager.Instance.FadeOutThemeMusic();
        }

        // 저장 해제
        GameManager.Instance.IsThereSavedGame = false;
        GameManager.Instance.SaveSettings();

        // 주 화면으로
        SceneManager.LoadScene(0);
    }


    private void Awake()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Show);

        if (GameManager.Instance.IsGameWin)
        {
            _titleText.text = Language.Instance["임무 완료"];
            _titleText.color = Constants.WHITE;
            _dateText.color = Constants.WHITE;
            _societyText.color = Constants.WHITE;
            _situationText.color = Constants.WHITE;
        }
        else
        {
            _titleText.text = Language.Instance["임무 실패"];
            _titleText.color = Constants.FAIL_TEXT;
            _dateText.color = Constants.FAIL_TEXT;
            _societyText.color = Constants.FAIL_TEXT;
            _situationText.color = Constants.FAIL_TEXT;
        }
    }


    private void Update()
    {
        if (4 <= _phase)
        {
            // 단축키 동작
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Return))
            {
                BtnBack();
            }
            return;
        }
        else if (_showTimer <= _timer)
        {
            switch (_phase)
            {
                case 0:
                    AudioManager.Instance.PlayAuido(AudioType.Show);
                    _dateText.text = UIString.Instance.GetDateString();
                    _timer -= _showTimer;
                    ++_phase;
                    break;

                case 1:
                    AudioManager.Instance.PlayAuido(AudioType.Show);
                    _societyText.text = Language.Instance[GameManager.Instance.LatestSocietyName];
                    _timer -= _showTimer;
                    ++_phase;
                    break;

                case 2:
                    AudioManager.Instance.PlayAuido(AudioType.Show);
                    _situationText.text = Language.Instance[GameManager.Instance.EndGameMessage];
                    _timer -= _showTimer;
                    ++_phase;
                    break;

                case 3:
                    AudioManager.Instance.PlayAuido(AudioType.Show);
                    _backButton.SetActive(true);
                    ++_phase;
                    break;
            }
        }

        _timer += Time.deltaTime;
    }
}
