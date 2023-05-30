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
        if (_showTimer <= _timer)
        {
            AudioManager.Instance.PlayAuido(AudioType.Show);
            _dateText.text = UIString.Instance.GetDateString();
        }
        else if (_showTimer * 2 <= _timer)
        {
            AudioManager.Instance.PlayAuido(AudioType.Show);
            _societyText.text = GameManager.Instance.LatestSocietyName;
        }
        else if (_showTimer * 3 <= _timer)
        {
            AudioManager.Instance.PlayAuido(AudioType.Show);
            _situationText.text = "여기에 텍스트 입력";
        }
        else if (_showTimer * 4 <= _timer)
        {
            AudioManager.Instance.PlayAuido(AudioType.Show);
            _backButton.SetActive(true);
        }

        _timer += Time.deltaTime;
    }
}
