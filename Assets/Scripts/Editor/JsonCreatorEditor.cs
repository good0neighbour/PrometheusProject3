using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JsonCreatorEditor : EditorWindow
{
    private struct JsonLanguage
    {
        public string[] Texts;
    }



    /* ==================== Variables ==================== */

    static private JsonCreatorEditor _window = null;
    private JsonLanguage _json;
    private Dictionary<string, int> _texts = null;
    private LanguageType _language = LanguageType.English;
    private string _status = null;
    private string _koreanKey = null;
    private string _value = null;
    private string _isLoadedString = null;
    private bool _isLoaded = false;



    /* ==================== Private Methods ==================== */

    [MenuItem("Window/PrometheusMission/Language Json Create")]
    private static void Open()
    {
        if (null == _window)
        {
            _window = CreateInstance<JsonCreatorEditor>();

            _window.position = new Rect(100.0f, 100.0f, 1000.0f, 1000.0f);
        }

        _window.Show();
    }


    private void OnGUI()
    {
        // json ���� ����
        GUILayout.Label("\njson ���� ����", EditorStyles.boldLabel);
        if (GUILayout.Button("�ѱ��� json �� ������ ���� ����"))
        {
            Language.Instance.LanguageSave();
            AssetDatabase.Refresh();
            _status = "�ѱ��� json �����.";
        }
        if (GUILayout.Button("�ٸ� ��� json ����"))
        {
            Language.Instance.SaveOtherLanguages();
            AssetDatabase.Refresh();
            _status = "�ٸ� ��� json �����.";
        }

        GUILayout.Label(_status, EditorStyles.boldLabel);

        // ��� �׽�Ʈ
        GUILayout.Label("\n��� �׽�Ʈ", EditorStyles.boldLabel);

        // json �ҷ�����
        EditorGUILayout.LabelField("===== json �ҷ����� =====");
        _language = (LanguageType)EditorGUILayout.EnumFlagsField(_language);
        if (GUILayout.Button("json �ҷ�����"))
        {
            _isLoaded = LoadJson(_language);
            if (_isLoaded)
            {
                _isLoadedString = "����";
            }
            else
            {
                _isLoadedString = "����";
            }
        }
        EditorGUILayout.LabelField(_isLoadedString);

        // �о���� ���� �ÿ���
        if (_isLoaded)
        {
            // �ѱ��� Ű �˻�
            EditorGUILayout.LabelField("===== �ѱ��� Ű �˻� =====");
            _koreanKey = EditorGUILayout.TextField("�ѱ��� Ű", _koreanKey);
            if (GUILayout.Button("�˻�"))
            {
                try
                {
                    _value = _json.Texts[_texts[_koreanKey]];
                }
                catch
                {
                    _value = "�������� �ʴ� Ű";
                }
            }

            // ��
            GUILayout.Label(_value, EditorStyles.boldLabel);
        }

        // �ݱ� ��ư
        if (GUILayout.Button("�ݱ�"))
        {
            _window.Close();
        }
    }


    private bool LoadJson(LanguageType language)
    {
        try
        {
            // �ؽ����̺� ������ �� ������
            if (null == _texts)
            {
                // �ѱ��� json �о����
                _json = JsonUtility.FromJson<JsonLanguage>(Resources.Load("Korean").ToString());

                // �ؽ����̺� ����
                _texts = new Dictionary<string, int>();

                // Ű, �� ���
                for (int i = 0; i < _json.Texts.Length; ++i)
                {
                    _texts.Add(_json.Texts[i], i);
                }
            }
            
            // ���� �� ����
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
                case LanguageType.Russian:
                    filename = "Russian";
                    break;
                case LanguageType.Spanish:
                    filename = "Spanish";
                    break;
                default:
                    Debug.LogError("�߸��� ��� ����.");
                    return false;
            }

            // json �о����
            _json = JsonUtility.FromJson<JsonLanguage>(Resources.Load(filename).ToString());

            // ����
            return true;
        }
        catch
        {
            // ����
            return false;
        }
    }
}