using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using TMPro;

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
            if (initialize)
            {
                Texts = new string[]
                {
                    "��",
                    "��",
                    "����",
                    "����",
                    "��Ÿ",
                    "����",
                    "���",
                    "�ڷ�",
                    "������",
                    "��� ����",
                    "��� ���",
                    "���� ü��",
                    "��� ��",
                    "Ž��",
                    "����",
                    "��",
                    "ź��",
                    "���ռ� ����",
                    "ȣ�� ����",
                    "Ž�� ��� �߰�",
                    "��� ���� ������ �Ǽ�",
                    "��� ���� ������ �Ǽ�",
                    "���� �ռ� ������ �Ǽ�",
                    "�� ���� ������ �Ǽ�",
                    "������ �Ǽ� ���",
                    "���� ���� Ž�� ���",
                    "Ž�� ���൵",
                    "�Ǽ��� ������",
                    "���",
                    "�� ��ü ����",
                    "������ ����",
                    "ź�� ��ü ����",
                    "��Ÿ ��ü ����",
                    "�߷°��ӵ�",
                    "�༺ ǥ����",
                    "�ݻ���",
                    "������ �½�",
                    "ź�� �½�",
                    "��Ÿ ��� �½�",
                    "�˵� ��ݰ�",
                    "��ü",
                    "��ü",
                    "��ü",
                    "���",
                    "����",
                    "�ϱ�",
                    "������",
                    "���� ���ɼ�",
                    "���� ��û",
                    "���ռ� ���� ������",
                    "ȣ�� ���� ������",
                    "���� ������",
                    "��� ������",
                    "��ü ���� ��",
                    "ä��",
                    "�α�",
                    "���",
                    "������",
                    "�ҵ���",
                    "��������",
                    "�뵵��",
                    "�ڱ�",
                    "���� �Ǽ�",
                    "�޴�",
                    "����",
                    "���뷮",
                    "ö",
                    "�ٹ���",
                    "����",
                    "����",
                    "���� ��ȭ",
                    "�̵�� ��ȭ",
                    "�ܱ�",
                    "����",
                    "����",
                    "��ȸ",
                    "���� �Ϸ�",
                    "��å ����",
                    "��å ����",
                    "���� �̸� �Է�",
                    "����",
                    "ȹ��",
                    "�ü� ��� ����",
                    "���ȭ ���� ����",
                    "��� ���� ����",
                    "���ȭ ���� ����",
                    "��� ���� ����",
                    "Ž�� ����",
                    "���� ����",
                    "�µ� ����",
                    "�� ����",
                    "ź�� ����",
                    "���ռ� ���� ����",
                    "ȣ�� ���� ����",
                    "ö ����",
                    "�ٹ��� ����",
                    "���� ����",
                };
            }
            else
            {
                Texts = null;
            }
        }
    }



    /* ==================== Variables ==================== */

    private static Language _instance = null;
    public static OnChangeDelegate OnLanguageChange = null;

    private JsonLanguage _jsonLanguage;
    private TMP_FontAsset _fontAsset = null;
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
    /// �̰��� �� �����̹Ƿ� ���� ������ ���� �������. �ѱ��� 'Ű'�� �Է��ϸ� ������ '��'�� ��ȯ�Ѵ�.
    /// </summary>
    public string this[string koreanKey]
    {
        get
        {
            try
            {
                return _jsonLanguage.Texts[_texts[koreanKey]];
            }
            catch
            {
                Debug.LogError($"���� �ѱ��� Ű \"{koreanKey}\"");
                return null;
            }
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
    /// �����ϴ� Ű���� Ȯ��
    /// </summary>
    public bool GetContainsKey(string koreanKey)
    {
        return _texts.ContainsKey(koreanKey);
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
                case LanguageType.German:
                    filename = "German";
                    break;
                case LanguageType.French:
                    filename = "French";
                    break;
                case LanguageType.Taiwanese:
                    filename = "Taiwanese";
                    break;
                case LanguageType.Japanese:
                    filename = "Japanese";
                    break;
                default:
                    Debug.LogError("LoadLangeage - LanguageType ���� ���.");
                    return;
            }

            // json ������ �ҷ��´�.
            _jsonLanguage = JsonUtility.FromJson<JsonLanguage>(Resources.Load(filename).ToString());
            _fontAsset = (TMP_FontAsset)Resources.Load($"{filename}Font SDF");
        }
        catch
        {
            Debug.Log("��� ������ �������� ����.");

            // json ������ ������ ���� �����Ѵ�.
            LanguageSave();

            // ����ü�� �ʱ�ȭ�Ѵ�.
            _jsonLanguage = new JsonLanguage(true);
        }

        // �븮��
        OnLanguageChange?.Invoke();
        GameManager.Instance.CurrentLanguage = language;
    }


    /// <summary>
    /// �����Ϳ��� json ���� �����Ѵ�.
    /// </summary>
    public void LanguageSave()
    {
        // ����ü�� �ʱ�ȭ�Ѵ�.
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // json ������ �����Ѵ�.
        File.WriteAllText($"{Application.dataPath}/Resources/Korean.Json", JsonUtility.ToJson(jsonLanguage, true));

        // ���� ������ ���� �ؽ�Ʈ ���Ϸ� �����Ѵ�.
        TextFileForGoogleTranslate(jsonLanguage);
    }


    /// <summary>
    /// �ٸ� ��� json ������ �����Ѵ�.
    /// </summary>
    public void SaveOtherLanguages()
    {
        // �غ� �ܰ�
        string[] path = Directory.GetFiles($"{Application.dataPath}/Translations/", "*.txt", SearchOption.AllDirectories);
        ushort num = (ushort)new JsonLanguage(true).Texts.Length;
        string jsonForm = Resources.Load("Korean").ToString();
        StringBuilder[] words = new StringBuilder[num];
        StringBuilder result = new StringBuilder();
        for (int j = 0; j < words.Length; ++j)
        {
            words[j] = new StringBuilder();
        }

        // �����ϴ� ��� ������ ����
        for (byte i = 0; i < path.Length; ++i)
        {
            // ��� �ϳ� �غ� �ܰ�
            string text = File.ReadAllText(path[i]);
            ushort index = 0;
            for (int j = 0; j < words.Length; ++j)
            {
                words[j].Clear();
            }

            // �ܾ� ����
            for (int j = 0; j < text.Length; ++j)
            {
                if (';' == text[j])
                {
                    j += 2;
                    ++index;
                }
                else
                {
                    words[index].Append(text[j]);
                }
            }

            // jsonȭ ���� �غ� �ܰ�
            int count = 0;
            bool proceed = true;
            index = 0;
            result.Clear();

            // json ���� ���󰡱�
            for (int j = 0; j < jsonForm.Length; ++j)
            {
                // ���
                if (proceed)
                {
                    result.Append(jsonForm[j]);
                }

                // ū����ǥ ����
                if ('\"' == jsonForm[j])
                {
                    ++count;

                    // ū����ǥ 3�� �̻��� ��
                    if (3 <= count)
                    {
                        // ū����ǥ ������ ��
                        if (1 == count % 2)
                        {
                            // ���ο� �ܾ� ����
                            result.Append(words[index]);
                            result.Append('\"');

                            // ���� �ܾ�
                            ++index;

                            // ��� ����
                            proceed = false;
                        }
                        // ū����ǥ ������ ��
                        else
                        {
                            // ��� �簳
                            proceed = true;
                        }
                    }
                }
            }

            // json ���Ϸ� ����
            File.WriteAllText($"{Application.dataPath}/Resources/{Path.GetFileNameWithoutExtension(path[i])}.Json", result.ToString());
        }
    }



    /* ==================== Private Methods ==================== */

    private Language()
    {
        // �ѱ���� �ʱ�ȭ
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // Dictionary�� �߰�
        for (ushort i = 0; i < jsonLanguage.Texts.Length; ++i)
        {
            try
            {
                // �ش� �ѱ�� ��� �ε����� �ִ��� ����
                _texts.Add(jsonLanguage.Texts[i], i);
            }
            catch
            {
                Debug.LogError($"���� Ű�� ���� - \"{jsonLanguage.Texts[i]}\"");
            }
        }
    }


    /// <summary>
    /// ���� ������ ���� �ؽ�Ʈ ���Ϸ� ����
    /// </summary>
    private void TextFileForGoogleTranslate(JsonLanguage jsonLanguage)
    {
        // ���ڿ� ����
        StringBuilder text = new StringBuilder();
        for (ushort i = 0; i < jsonLanguage.Texts.Length; ++i)
        {
            text.Append($"{jsonLanguage.Texts[i]};\n");
        }

        // �ؽ�Ʈ ���Ϸ� ����
        File.WriteAllText($"{Application.dataPath}/TranslateThis.txt", text.ToString());
    }
}
