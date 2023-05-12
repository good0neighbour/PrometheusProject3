using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using TMPro;
using static Unity.VisualScripting.Icons;

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
            if (initialize)
            {
                Texts = new string[]
                {
                    "년",
                    "월",
                    "시작",
                    "설정",
                    "기타",
                    "종료",
                    "언어",
                    "뒤로",
                    "지지율",
                    "평균 대기압",
                    "평균 기온",
                    "물의 체적",
                    "산소 농도",
                    "탐사",
                    "대기압",
                    "물",
                    "탄소",
                    "광합성 생물",
                    "호흡 생물",
                    "탐사 장비 추가",
                    "대기 조정 인프라 건설",
                    "기온 조정 인프라 건설",
                    "분자 합성 인프라 건설",
                    "농도 조정 인프라 건설",
                    "가동 중인 탐사 장비",
                    "탐사 진행도",
                    "건설된 인프라",
                    "비용",
                    "총 기체 질량",
                    "수증기 질량",
                    "탄소 기체 질량",
                    "기타 기체 질량",
                    "중력가속도",
                    "행성 표면적",
                    "반사율",
                    "수증기 온실",
                    "탄소 온실",
                    "기타 대기 온실",
                    "궤도 장반경",
                    "기체",
                    "액체",
                    "고체",
                    "기권",
                    "수권",
                    "암권",
                    "생물권",
                    "생존 가능성",
                    "종자 요청",
                    "광합성 생물 안정도",
                    "호흡 생물 안정도",
                    "대기압 적절성",
                    "기온 적절성",
                    "액체 상태 물",
                    "채권",
                    "인구",
                    "기온",
                    "안정도",
                    "소도시",
                    "중형도시",
                    "대도시",
                    "자금",
                    "도시 건설",
                    "메뉴",
                    "토지",
                    "수용량",
                };
            }
            else
            {
                Texts = null;
            }
        }
    }


    /// <summary>
    /// 언어 변경 시 동작할 대리자
    /// </summary>
    public delegate void LanguageDelegate();



    /* ==================== Variables ==================== */

    private static Language _instance = null;
    public static LanguageDelegate OLC = null;

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
    /// 이것이 주 목적이므로 편리한 접근을 위해 만들었다. 한국어 '키'를 입력하면 번역된 '값'을 반환한다.
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
                Debug.LogError($"없는 한국어 키 \"{koreanKey}\"");
                return null;
            }
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
    /// 존재하는 키인지 확인
    /// </summary>
    public bool GetContainsKey(string koreanKey)
    {
        return _texts.ContainsKey(koreanKey);
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
            Debug.Log("언어 파일이 존재하지 않음.");

            // json 파일이 없으면 새로 저장한다.
            LanguageSave();

            // 구조체를 초기화한다.
            _jsonLanguage = new JsonLanguage(true);
        }

        // 대리자
        OLC?.Invoke();
        GameManager.Instance.CurrentLanguage = language;
    }


    /// <summary>
    /// 에디터에서 json 파일 생성한다.
    /// </summary>
    public void LanguageSave()
    {
        // 구조체를 초기화한다.
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // json 파일을 저장한다.
        string json = JsonUtility.ToJson(jsonLanguage, true);
        File.WriteAllText($"{Application.dataPath}/Resources/Korean.Json", json);

        // 구글 번역을 위해 텍스트 파일로 저장한다.
        TextFileForGoogleTranslate(jsonLanguage);

        // 에디터에 출력한다.
        Debug.Log(json);
    }


    /// <summary>
    /// 다른 언어 json 파일을 생성한다.
    /// </summary>
    public void SaveOtherLanguages()
    {
        // 준비 단계
        string[] path = Directory.GetFiles($"{Application.dataPath}/Translations/", "*.txt", SearchOption.AllDirectories);
        ushort num = (ushort)new JsonLanguage(true).Texts.Length;
        string jsonForm = Resources.Load("Korean").ToString();
        StringBuilder[] words = new StringBuilder[num];
        StringBuilder result = new StringBuilder();
        StringBuilder record = new StringBuilder();
        for (int j = 0; j < words.Length; ++j)
        {
            words[j] = new StringBuilder();
        }

        // 존재하는 모든 번역본 생성
        for (byte i = 0; i < path.Length; ++i)
        {
            // 언어 하나 준비 단계
            string text = File.ReadAllText(path[i]);
            ushort index = 0;
            for (int j = 0; j < words.Length; ++j)
            {
                words[j].Clear();
            }

            // 단어 추출
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

            // json화 시작 준비 단계
            int count = 0;
            index = 0;
            result.Clear();
            record.Clear();

            // json 형식 따라가기
            for (int j = 0; j < jsonForm.Length; ++j)
            {
                // 기록
                record.Append(jsonForm[j]);

                // 큰따옴표 세기
                if ('\"' == jsonForm[j])
                {
                    ++count;

                    // 큰따옴표 3개 이상일 때
                    if (3 <= count)
                    {
                        // 큰따옴표 열렸을 때
                        if (1 == count % 2)
                        {
                            // 기록 저장
                            result.Append(record.ToString());
                            result.Append(words[index]);
                            result.Append('\"');
                            ++index;
                        }
                        // 큰따옴표 닫혔을 때
                        else
                        {
                            // 기록 초기화
                            record.Clear();
                        }
                    }
                }
            }

            // 마지막 기록 저장
            result.Append(record.ToString());

            // json 파일로 저장
            File.WriteAllText($"{Application.dataPath}/Resources/{Path.GetFileNameWithoutExtension(path[i])}.Json", result.ToString());
            Debug.Log(result.ToString());
        }
    }



    /* ==================== Private Methods ==================== */

    private Language()
    {
        // 한국어로 초기화
        JsonLanguage jsonLanguage = new JsonLanguage(true);

        // Dictionary에 추가
        for (ushort i = 0; i < jsonLanguage.Texts.Length; ++i)
        {
            try
            {
                // 해당 한국어가 어느 인덱스에 있는지 저장
                _texts.Add(jsonLanguage.Texts[i], i);
            }
            catch
            {
                Debug.LogError($"같은 키가 존재 - \"{jsonLanguage.Texts[i]}\"");
            }
        }
    }


    /// <summary>
    /// 구글 번역을 위해 텍스트 파일로 저장
    /// </summary>
    private void TextFileForGoogleTranslate(JsonLanguage jsonLanguage)
    {
        // 문자열 생성
        StringBuilder text = new StringBuilder();
        for (ushort i = 0; i < jsonLanguage.Texts.Length; ++i)
        {
            text.Append($"{jsonLanguage.Texts[i]};\n");
        }

        // 텍스트 파일로 저장
        File.WriteAllText($"{Application.dataPath}/TranslateThis.txt", text.ToString());
    }
}
