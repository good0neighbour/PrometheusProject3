using UnityEngine;
using TMPro;

public class AutoTranslation : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private bool _onlyChangeFont = false;
    [SerializeField] private bool _shortenLineGap = true;

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

#if UNITY_EDITOR
        // 찾을 수 없으면
        if (null == _text)
        {
            Debug.LogError($"AutoTranslation 잘못된 위치에 부착 - {name}");
            return;
        }
#endif

        // 언어 대리자에 등록한다.
        Language.OnLanguageChange += OnLanguageChange;

        // 줄 간격을 줄이려 할 때
        if (_shortenLineGap)
        {
            _text.lineSpacing = -35.0f;
            _text.enableWordWrapping = false;
        }
        else
        {
            _text.lineSpacing = 0.0f;
        }

        // 폰트만 바꿀 경우 여기서 종료.
        if (_onlyChangeFont)
        {
            return;
        }

        // 이것이 한국어 키다.
        _koreanKey = _text.text;

#if UNITY_EDITOR
        // 해당 키가 존재하는지 확인
        if (!Language.Instance.GetContainsKey(_koreanKey))
        {
            Debug.LogError($"\"{_koreanKey}\" - 존재하지 않는 키. 오타 수정 및 삭제 요망.\n{transform.parent.name}/{name}");
            Language.OnLanguageChange -= OnLanguageChange;
        }
#endif
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 언어 변경 시 동작
    /// </summary>
    private void OnLanguageChange()
    {
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
}
