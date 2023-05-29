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
        // json 파일 생성
        GUILayout.Label("\njson 파일 생성", EditorStyles.boldLabel);
        if (GUILayout.Button("한국어 json 및 번역용 파일 생성"))
        {
            ((JsonCreator)target).LanSave();
            _status = "한국어 json 저장됨.";
        }
        if (GUILayout.Button("다른 언어 json 생성"))
        {
            ((JsonCreator)target).OhterLanSave();
            _status = "다른 언어 json 저장됨.";
        }

        GUILayout.Label(_status, EditorStyles.boldLabel);

        // 언어 테스트
        GUILayout.Label("\n언어 테스트", EditorStyles.boldLabel);

        // json 불러오기
        EditorGUILayout.LabelField("===== json 불러오기 =====");
        _language = (LanguageType)EditorGUILayout.EnumFlagsField(_language);
        if (GUILayout.Button("json 불러오기"))
        {
            _isLoaded = LoadJson(_language);
            if (_isLoaded)
            {
                _isLoadedString = "성공";
            }
            else
            {
                _isLoadedString = "실패";
            }
        }
        EditorGUILayout.LabelField(_isLoadedString);

        // 읽어오기 성공 시에만
        if (_isLoaded)
        {
            // 한국어 키 검색
            EditorGUILayout.LabelField("===== 한국어 키 검색 =====");
            _koreanKey = EditorGUILayout.TextField("한국어 키", _koreanKey);
            if (GUILayout.Button("검색"))
            {
                try
                {
                    _value = _json.Texts[_texts[_koreanKey]];
                }
                catch
                {
                    _value = "존재하지 않는 키";
                }
            }

            // 값
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
            // 해쉬테이블 생성한 적 없으면
            if (null == _texts)
            {
                // 한국어 json 읽어오기
                _json = JsonUtility.FromJson<JsonLanguage>(Resources.Load("Korean").ToString());

                // 해쉬테이블 생성
                _texts = new Dictionary<string, int>();

                // 키, 값 등록
                for (int i = 0; i < _json.Texts.Length; ++i)
                {
                    _texts.Add(_json.Texts[i], i);
                }
            }
            
            // 파일 명 설정
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
                    Debug.LogError("잘못된 언어 종류.");
                    return false;
            }

            // json 읽어오기
            _json = JsonUtility.FromJson<JsonLanguage>(Resources.Load(filename).ToString());

            // 성공
            return true;
        }
        catch
        {
            // 실패
            return false;
        }
    }
}