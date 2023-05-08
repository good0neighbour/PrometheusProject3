using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;

/// <summary>
/// 사용 가능한 언어 종류
/// </summary>
public enum LanguageType
{
    Korean,
    English,
    End
}

/// <summary>
/// 번역된 언어를 위한 언어 관리 클래스
/// </summary>
public class Language
{
    /// <summary>
    /// json 파일로부터 읽어온 언어를 담을 구조체
    /// </summary>
    private struct JsonLanguage
    {
        public string[] Texts;

        /// <summary>
        /// 초기화는 여기서
        /// </summary>
        public JsonLanguage(bool initialize)
        {
            Texts = new string[]
            {
                "시작",
                "설정",
                "기타",
                "종료",
                "평균 대기압",
                "평균 기온",
                "물의 체적",
                "산소 농도",
                "대기압",
                "물",
                "탄소",
                "광합성 생물",
                "호흡 생물",
                "대기 조정 인프라 건설",
                "건설된 인프라",
            };
        }
    }


    /// <summary>
    /// 언어 변경 시 동작할 대리자
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
    /// 편리한 접근을 위해 만들었다. 한국어 '키'를 입력하면 번역된 '값'을 반환.
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
    /// 폰트를 가져온다.
    /// </summary>
    public TMP_FontAsset GetFontAsset()
    {
        return _fontAsset;
    }


    /// <summary>
    /// json 파일 불러온다.
    /// </summary>
    public void LoadLangeage(LanguageType language)
    {
        try
        {
            // json 파일 명을 담는다.
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
                    return;
            }

            // json 파일을 불러온다.
            _jsonLanguage = JsonUtility.FromJson<JsonLanguage>(Resources.Load(filename).ToString());
            _fontAsset = (TMP_FontAsset)Resources.Load($"{filename}Font SDF");
        }
        catch
        {
            // json 파일이 없으면 새로 저장한다.
            LanSave();

            // 구조체를 초기화한다.
            _jsonLanguage = new JsonLanguage(true);
        }
    }


    /// <summary>
    /// 에디터에서 json 파일 생성한다.
    /// </summary>
    public void LanSave()
    {
        // 구조체를 초기화한다.
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // json 파일을 저장한다.
        string json = JsonUtility.ToJson(jsonLanguage, true);
        File.WriteAllText(Application.dataPath + "/Resources/Korean.Json", json);

        // 구글 번역을 위해 텍스트 파일로 저장한다.
        TextFileForGoogleTranslate();

        // 에디터에 출력한다.
        Debug.Log(json);
    }



    /* ==================== Private Methods ==================== */

    private Language()
    {
        // 한국어 구조체 생성
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // Dictionary에 추가
        for (ushort i = 0; i < jsonLanguage.Texts.Length; ++i)
        {
            try
            {
                // 해당 한국에가 어느 인덱스에 있는지 저장
                _texts.Add(jsonLanguage.Texts[i], i);
            }
            catch
            {
                Debug.LogError($"같은 키가 존재 - {jsonLanguage.Texts[i]}");
            }
        }
    }


    /// <summary>
    /// 구글 번역을 위해 텍스트 파일로 저장
    /// </summary>
    private void TextFileForGoogleTranslate()
    {
        // 문자열 생성
        StringBuilder text = new StringBuilder();
        for (ushort i = 0; i < _jsonLanguage.Texts.Length; ++i)
        {
            text.Append($"{_jsonLanguage.Texts[i]};\n");
        }

        // 텍스트 파일로 저장
        File.WriteAllText(Application.dataPath + "/TranslateThis.txt", text.ToString());
    }
}
