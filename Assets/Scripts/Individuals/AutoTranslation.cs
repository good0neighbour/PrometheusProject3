using UnityEngine;
using TMPro;

public class AutoTranslation : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private bool _onlyChangeFont = false;

    private LanguageType _currentLanguage = LanguageType.Korean;
    private string _koreanKey = null;
    private TMP_Text _text = null;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 처음 씬 로드 시 대부분 텍스트는 비활성화 상태기 때문에 다른 클래스에서 호출할 것이다.
    /// </summary>
    public void TranslationReady()
    {
        // 컴포넌트 가져온다.
        _text = GetComponent<TMP_Text>();

        // 언어 대리자에 등록한다.
        Language.OLC += OnLanguageChange;

        // 폰트만 바꿀 경우 여기서 종료.
        if (_onlyChangeFont)
        {
            return;
        }

        // 이것이 한국어 키다.
        _koreanKey = _text.text;

        // 해당 키가 존재하는지 확인
        if (!Language.Instance.GetContainsKey(_koreanKey))
        {
            Debug.LogError($"\"{_koreanKey}\" - 존재하지 않는 키. 오타 수정 및 삭제 요망.");
        }
    }


    /// <summary>
    /// 언어 변경 시 동작
    /// </summary>
    public void OnLanguageChange(LanguageType currentLanguage)
    {
        // 현재 언어 정보 변경.
        _currentLanguage = currentLanguage;

        // 폰트 변경.
        _text.font = Language.Instance.GetFontAsset();

        // 폰트만 바꿀 경우 여기서 종료.
        if (_onlyChangeFont)
        {
            return;
        }

        // 번역 가져온다.
        _text.text = Language.Instance[_koreanKey];
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 언어 설정이 한국어가 아닐 경우
        if (_currentLanguage != GameManager.Instance.CurrentLanguage)
        {
            _currentLanguage = GameManager.Instance.CurrentLanguage;
            OnLanguageChange(_currentLanguage);
        }
    }
}
