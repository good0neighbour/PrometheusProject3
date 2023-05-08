using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;

/// <summary>
/// ��� ������ ��� ����
/// </summary>
public enum LanguageType
{
    Korean,
    English,
    End
}

/// <summary>
/// ������ �� ���� ��� ���� Ŭ����
/// </summary>
public class Language
{
    /// <summary>
    /// json ���Ϸκ��� �о�� �� ���� ����ü
    /// </summary>
    private struct JsonLanguage
    {
        public string[] Texts;

        /// <summary>
        /// �ʱ�ȭ�� ���⼭
        /// </summary>
        public JsonLanguage(bool initialize)
        {
            Texts = new string[]
            {
                "����",
                "����",
                "��Ÿ",
                "����",
                "��� ����",
                "��� ���",
                "���� ü��",
                "��� ��",
                "����",
                "��",
                "ź��",
                "���ռ� ����",
                "ȣ�� ����",
                "��� ���� ������ �Ǽ�",
                "�Ǽ��� ������",
            };
        }
    }


    /// <summary>
    /// ��� ���� �� ������ �븮��
    /// </summary>
    public delegate void LanguageDelegate(LanguageType currentLanguage);



    /* ==================== Variables ==================== */

    private static Language _instance = null;
    public static LanguageDelegate OLC = null;

    private JsonLanguage _jsonLanguage;
    private TMP_FontAsset _fontAsset;
    private Dictionary<string, ushort> _texts = new Dictionary<string, ushort>();

    public static Language Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new Language();
            }

            return _instance;
        }
    }

    /// <summary>
    /// ���� ������ ���� �������. �ѱ��� 'Ű'�� �Է��ϸ� ������ '��'�� ��ȯ.
    /// </summary>
    public string this[string koreanKey]
    {
        get
        {
            return _jsonLanguage.Texts[_texts[koreanKey]];
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��Ʈ�� �����´�.
    /// </summary>
    public TMP_FontAsset GetFontAsset()
    {
        return _fontAsset;
    }


    /// <summary>
    /// json ���� �ҷ��´�.
    /// </summary>
    public void LoadLangeage(LanguageType language)
    {
        try
        {
            // json ���� ���� ��´�.
            string filename;
            switch (language)
            {
                case LanguageType.Korean:
                    filename = "Korean";
                    break;
                case LanguageType.English:
                    filename = "English";
                    break;
                default:
                    Debug.LogError("�߸��� ��� ����.");
                    return;
            }

            // json ������ �ҷ��´�.
            _jsonLanguage = JsonUtility.FromJson<JsonLanguage>(Resources.Load(filename).ToString());
            _fontAsset = (TMP_FontAsset)Resources.Load($"{filename}Font SDF");
        }
        catch
        {
            // json ������ ������ ���� �����Ѵ�.
            LanSave();

            // ����ü�� �ʱ�ȭ�Ѵ�.
            _jsonLanguage = new JsonLanguage(true);
        }
    }


    /// <summary>
    /// �����Ϳ��� json ���� �����Ѵ�.
    /// </summary>
    public void LanSave()
    {
        // ����ü�� �ʱ�ȭ�Ѵ�.
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // json ������ �����Ѵ�.
        string json = JsonUtility.ToJson(jsonLanguage, true);
        File.WriteAllText(Application.dataPath + "/Resources/Korean.Json", json);

        // ���� ������ ���� �ؽ�Ʈ ���Ϸ� �����Ѵ�.
        TextFileForGoogleTranslate();

        // �����Ϳ� ����Ѵ�.
        Debug.Log(json);
    }



    /* ==================== Private Methods ==================== */

    private Language()
    {
        // �ѱ��� ����ü ����
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // Dictionary�� �߰�
        for (ushort i = 0; i < jsonLanguage.Texts.Length; ++i)
        {
            try
            {
                // �ش� �ѱ����� ��� �ε����� �ִ��� ����
                _texts.Add(jsonLanguage.Texts[i], i);
            }
            catch
            {
                Debug.LogError($"���� Ű�� ���� - {jsonLanguage.Texts[i]}");
            }
        }
    }


    /// <summary>
    /// ���� ������ ���� �ؽ�Ʈ ���Ϸ� ����
    /// </summary>
    private void TextFileForGoogleTranslate()
    {
        // ���ڿ� ����
        StringBuilder text = new StringBuilder();
        for (ushort i = 0; i < _jsonLanguage.Texts.Length; ++i)
        {
            text.Append($"{_jsonLanguage.Texts[i]};\n");
        }

        // �ؽ�Ʈ ���Ϸ� ����
        File.WriteAllText(Application.dataPath + "/TranslateThis.txt", text.ToString());
    }
}
