using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonCreator))]
public class JsonCreatorEditor : Editor
{
    private struct JsonLanguage
    {
        public string[] Texts;
    }

    private JsonLanguage _json;
    private Dictionary<string, int> _texts = null;
    private LanguageType _language = LanguageType.English;
    private string _status = null;
    private string _koreanKey = null;
    private string _value = null;
    private string _isLoadedString = null;
    private bool _isLoaded = false;


    public override void OnInspectorGUI()
    {
        // json ���� ����
        GUILayout.Label("\njson ���� ����", EditorStyles.boldLabel);
        if (GUILayout.Button("�ѱ��� json �� ������ ���� ����"))
        {
            ((JsonCreator)target).LanSave();
            _status = "�ѱ��� json �����.";
        }
        if (GUILayout.Button("�ٸ� ��� json ����"))
        {
            ((JsonCreator)target).OhterLanSave();
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
            if (null != _value)
            {
                GUILayout.Label(_value, EditorStyles.boldLabel);
            }
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