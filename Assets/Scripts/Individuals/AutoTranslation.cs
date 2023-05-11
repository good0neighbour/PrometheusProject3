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
    /// ó�� �� �ε� �� ��κ� �ؽ�Ʈ�� ��Ȱ��ȭ ���±� ������ �ٸ� Ŭ�������� ȣ���� ���̴�.
    /// </summary>
    public void TranslationReady()
    {
        // ������Ʈ �����´�.
        _text = GetComponent<TMP_Text>();

        // ��� �븮�ڿ� ����Ѵ�.
        Language.OLC += OnLanguageChange;

        // ��Ʈ�� �ٲ� ��� ���⼭ ����.
        if (_onlyChangeFont)
        {
            return;
        }

        // �̰��� �ѱ��� Ű��.
        _koreanKey = _text.text;

        // �ش� Ű�� �����ϴ��� Ȯ��
        if (!Language.Instance.GetContainsKey(_koreanKey))
        {
            Debug.LogError($"\"{_koreanKey}\" - �������� �ʴ� Ű. ��Ÿ ���� �� ���� ���.");
        }
    }


    /// <summary>
    /// ��� ���� �� ����
    /// </summary>
    public void OnLanguageChange(LanguageType currentLanguage)
    {
        // ���� ��� ���� ����.
        _currentLanguage = currentLanguage;

        // ��Ʈ ����.
        _text.font = Language.Instance.GetFontAsset();

        // ��Ʈ�� �ٲ� ��� ���⼭ ����.
        if (_onlyChangeFont)
        {
            return;
        }

        // ���� �����´�.
        _text.text = Language.Instance[_koreanKey];
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ��� ������ �ѱ�� �ƴ� ���
        if (_currentLanguage != GameManager.Instance.CurrentLanguage)
        {
            _currentLanguage = GameManager.Instance.CurrentLanguage;
            OnLanguageChange(_currentLanguage);
        }
    }
}
