using UnityEngine;
using TMPro;

public class AutoTranslation : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private LanguageType _currentLanguage = LanguageType.Korean;
    private string _koreanKey = null;
    private TMP_Text _text = null;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 처음 씬 로드 시 대부분 텍스트는 비활성화 상태기 때문에 다른 클래스에서 호출할 것이다.
    /// </summary>
    public void TranslationReady()
    {
        // 한국어 키 찾는다.
        _text = GetComponent<TMP_Text>();
        _koreanKey = _text.text;

        // 언어 대리자에 등록한다.
        Language.OLC += OnLanguageChange;
    }


    /// <summary>
    /// 언어 변경 시 동작
    /// </summary>
    public void OnLanguageChange()
    {
        _text.text = Language.Instance.GetLanguage(_koreanKey);
        _text.font = Language.Instance.GetFontAsset();
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 언어 설정이 한국어가 아닐 경우
        if (_currentLanguage != GameManager.Instance.CurrentLanguage)
        {
            _currentLanguage = GameManager.Instance.CurrentLanguage;
            OnLanguageChange();
        }
    }
}
