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
    /// ó�� �� �ε� �� ��κ� �ؽ�Ʈ�� ��Ȱ��ȭ ���±� ������ �ٸ� Ŭ�������� ȣ���� ���̴�.
    /// </summary>
    public void TranslationReady()
    {
        // �ѱ��� Ű ã�´�.
        _text = GetComponent<TMP_Text>();
        _koreanKey = _text.text;

        // ��� �븮�ڿ� ����Ѵ�.
        Language.OLC += OnLanguageChange;
    }


    /// <summary>
    /// ��� ���� �� ����
    /// </summary>
    public void OnLanguageChange()
    {
        _text.text = Language.Instance.GetLanguage(_koreanKey);
        _text.font = Language.Instance.GetFontAsset();
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ��� ������ �ѱ�� �ƴ� ���
        if (_currentLanguage != GameManager.Instance.CurrentLanguage)
        {
            _currentLanguage = GameManager.Instance.CurrentLanguage;
            OnLanguageChange();
        }
    }
}
