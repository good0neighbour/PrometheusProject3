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
    /// ó�� �� �ε� �� ��κ� �ؽ�Ʈ�� ��Ȱ��ȭ ���±� ������ �ٸ� Ŭ�������� ȣ���� ���̴�.
    /// </summary>
    public void TranslationReady()
    {
        // ������Ʈ �����´�.
        _text = GetComponent<TMP_Text>();

#if UNITY_EDITOR
        // ã�� �� ������
        if (null == _text)
        {
            Debug.LogError($"AutoTranslation �߸��� ��ġ�� ���� - {name}");
            return;
        }
#endif

        // ��� �븮�ڿ� ����Ѵ�.
        Language.OnLanguageChange += OnLanguageChange;

        // �� ������ ���̷� �� ��
        if (_shortenLineGap)
        {
            _text.lineSpacing = -35.0f;
            _text.enableWordWrapping = false;
        }
        else
        {
            _text.lineSpacing = 0.0f;
        }

        // ��Ʈ�� �ٲ� ��� ���⼭ ����.
        if (_onlyChangeFont)
        {
            return;
        }

        // �̰��� �ѱ��� Ű��.
        _koreanKey = _text.text;

#if UNITY_EDITOR
        // �ش� Ű�� �����ϴ��� Ȯ��
        if (!Language.Instance.GetContainsKey(_koreanKey))
        {
            Debug.LogError($"\"{_koreanKey}\" - �������� �ʴ� Ű. ��Ÿ ���� �� ���� ���.\n{transform.parent.name}/{name}");
            Language.OnLanguageChange -= OnLanguageChange;
        }
#endif
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� ���� �� ����
    /// </summary>
    private void OnLanguageChange()
    {
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
}
