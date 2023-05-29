using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using TMPro;

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
                    "제 1시대",
                    "시작",
                    "불러오기",
                    "언어",
                    "주 메뉴",
                    "뒤로",
                    "설정",
                    "초당 프레임 수",
                    "현재 초당 프레임 수",
                    "음량",
                    "현재 음량",
                    "임무 철회",
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
                    "인프라 건설 비용",
                    "가동 중인 탐사 장비",
                    "탐사 진행도",
                    "건설된 인프라",
                    "비용",
                    "대기 총 질량",
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
                    "기록",
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
                    "철",
                    "핵물질",
                    "보석",
                    "승인",
                    "미디어 문화",
                    "외교",
                    "연구",
                    "도시",
                    "사회",
                    "승인 완료",
                    "승인 불가",
                    "정책 성공",
                    "정책 실패",
                    "도시 이름 입력",
                    "개월",
                    "수익",
                    "시설 사용 가능",
                    "상용화 연구 가능",
                    "사상 연구 가능",
                    "사회 채택 가능",
                    "상용화 연구 없음",
                    "사상 연구 없음",
                    "작가 협회",
                    "가족 중심",
                    "정부 자산",
                    "쾌락 중심",
                    "대중영화",
                    "인구 변화율 감소",
                    "지지율 제어 [비용: 문화 1]",
                    "예술 의식",
                    "혼인률 제어 [비용: 문화 1]",
                    "정부 영향력",
                    "시상식",
                    "자유 예술",
                    "영향력 [비용: 문화 1]",
                    "인터넷 밈 유머",
                    "시나리오 검수",
                    "인구 변화율 증가",
                    "자산화 [비용: 문화 1]",
                    "문화",
                    "시설 지지율",
                    "연구 지지율",
                    "외교 지지율",
                    "사회 지지율",
                    "연간 자금",
                    "연간 연구",
                    "연간 문화",
                    "유지비용",
                    "시설 승인에 대한 지지율",
                    "연구 승인에 대한 지지율",
                    "사회 승인에 대한 지지율",
                    "외교 승인에 대한 지지율",
                    "질병",
                    "부상",
                    "범죄율",
                    "인구 변화",
                    "인구 증가",
                    "안정도 증가",
                    "범죄율 감소",
                    "질병 감소",
                    "부상 감소",
                    "부상 증가",
                    "공공외교",
                    "무역",
                    "공작활동",
                    "대사 파견",
                    "속국화 진행",
                    "우호도",
                    "대기 중",
                    "사용 가능",
                    "플레이어 소프트파워",
                    "상대 소프트파워",
                    "음모론",
                    "선동",
                    "속국화",
                    "적대자",
                    "우호도 약간 증가",
                    "우호도 증가",
                    "우호도 변화 없음",
                    "우호도 매우 증가",
                    "적대자 약간 증가",
                    "적대자 증가",
                    "적대자 매우 증가",
                    "혼란 약간 증가",
                    "혼란 증가",
                    "혼란 매우 증가",
                    "공작 방어 약간 감소",
                    "공작 방어 감소",
                    "공작 방어 매우 감소",
                    "수입",
                    "수출",
                    "연간 수익",
                    "연간 지출",
                    "세계 의제",
                    "적도반경",
                    "평균 밀도",
                    "탄소 농도",
                    "물 체적",
                    "기체 질량",
                    "새로 고침",
                    "다음",
                    "종료",
                    "새로 시작",
                    "계속",
                    "지구",
                    "세력1",
                    "세력2",
                    "세력3",
                    "런던",
                    "베를린",
                    "카이로",
                    "동경",
                    "모스크바",
                    "파리",
                    "리야드",
                    "워싱턴",
                    "의제 없음",
                    "기후 변화",
                    "지구 평균 기온 상승에 의해 지구의 기후 패턴이 급격하게 변화하면서 화석 연료 사용에 대한 문제가 대두되었습니다. 세계는 화석 연료를 사용하지 않는 기술의 연구, 개발에 많은 투자를 하게 됩니다. 이러한 기술은 훗날 인류가 외계 행성에 정착할 때 생산 활동에 도움을 줍니다.",
                    "환경 파괴",
                    "대기, 토양, 물 등에 오염물질이 유출되면서 자연 환경과 생태계가 파괴되는 문제가 발생했습니다. 세계는 문제의 원인을 화석 연료를 주 원료로 사용하는 플라스틱에 두었고, 플라스틱을 대체할 소재를 개발하는데 노력하게 됩니다. 이러한 노력은 훗날 인류가 외계 행성에 정착할 때 자원 활용에 도움을 줍니다.",
                    "자원 고갈",
                    "19세기 경부터 인류 문명은 화석 연료에 크게 의지하는 형태로 발전해왔으나 화석 연료가 형성되는 속도보다 고갈되는 속도가 더 빨라지면서 자원 고갈 문제가 제기되었습니다. 세계는 천연 자원 및 재생 에너지 사용을 위해 노력하게 됩니다. 이러한 노력은 훗날 인류가 외계 행성에 정착할 때 자원 사용에 도움을 줍니다.",
                    "문화잠식",
                    "문화강탈",
                    "문화공정",
                    "음악 공연",
                    "동양화",
                    "의복 유행",
                    "영화 수출",
                    "문화 편견",
                    "자국 영웅화",
                    "역사 기원",
                    "역사 비하",
                    "정치인 압박",
                    "극단적 채식주의 선동",
                    "약자 혐오 선동",
                    "전자 페미니즘 선동",
                    "창조과학론 투입",
                    "설거지론 투입",
                    "정치 음모론 투입",
                    "탐사를 시작하면 채굴할 광물이 매장된 토지를 획득할 수 있습니다. 혹은 당신의 경쟁 기업이 세운 또다른 국가와 조우할 수도 있습니다.",
                    "행성의 대기량을 조절하여 적합한 대기압을 만드십시오. 자금을 투자하여 대기량을 증가시키거나 감소시킬 수 있습니다. 기압은 행성의 온도와 물 순환, 탄소 순환에 영향을 줍니다. 이상적인 기압은 1013.25hPa입니다.",
                    "생명이 정착하기 적절한 온도를 형성하십시오. 행성의 온도는 물 순환에 영향을 줍니다. 행성으로 유입되는 열과 방출되는 열은 평형을 이루기 때문에 자금 투자에 의한 온도 변화는 일시적입니다. 이상적인 온도는 15℃입니다.",
                    "생명이 살아가는 데에 물은 필수적입니다. 액체 상태의 물은 생명 정착을 시작할 수 있게 하고 기체 상태의 물은 기온 상승을 유발함과 동시에 기압 상승에 미미한 영향을 줍니다. 그래프를 참고하여 물의 체적을 이상적인 양으로 조절할 수 있습니다.",
                    "탄소는 생명의 기본 재료이면서 문명을 위한 재료이기도 합니다. 자금을 투자하여 기권으로의 탄소 유입을 조절할 수 있으나 높은 농도의 탄소가 기권으로 유입되면 기온 상승을 유발합니다.",
                    "자금을 투자하여 지구로부터 종자를 보급받을 수 있습니다. 생물은 액체 상태의 물, 기압, 온도, 탄소 순환의 안정도에 영향을 받습니다. 광합성 생물을 산소를 발생시킵니다.",
                    "자금을 투자하여 지구로부터 종자를 보급받을 수 있습니다. 생물은 액체 상태의 물, 기압, 온도, 탄소 순환의 안정도에 영향을 받습니다. 호흡 생물이 생존하기 위해서는 산소를 필요로 합니다.",
                    "철 설명",
                    "핵물질 설명",
                    "보석 설명",
                    "태양광발전소",
                    "가장 기본적인 전력 생산 방식입니다.",
                    "원자력발전소",
                    "핵분열 에너지를 이용해 전기를 대량 생산합니다.",
                    "핵융합발전소",
                    "존재만 한다면 정전은 없을 것입니다.",
                    "곡창",
                    "식량을 저장하고 낭비를 줄입니다.",
                    "농업단지",
                    "농산물 생산량이 증가합니다.",
                    "목축업단지",
                    "육류와 유제품 생산량이 증가합니다.",
                    "식료품점",
                    "식량이 필요한 시민의 접근성을 높입니다.",
                    "비료공장",
                    "농산물의 품질과 생산량을 증대합니다.",
                    "농산물류",
                    "식량 유통을 관리하여 공급을 안정화합니다.",
                    "보건소",
                    "질병에 감염된 시민들을 치료합니다.",
                    "보건교육",
                    "질병에 대한 시민들의 인지도를 높입니다.",
                    "외과센타",
                    "한 분야에 집중적으로 훈련받은 의사는 사망률을 낮추는 데 기여합니다.",
                    "응급실",
                    "응급환자가 치료를 기다리는 중에 사망하는 경우를 최소화합니다.",
                    "병원",
                    "환자 수용량을 늘리고 전문적인 치료를 제공합니다.",
                    "의학연구소",
                    "질병과 치료 연구를 통해 보다 나은 의료 서비스를 제공합니다.",
                    "소방서",
                    "긴급 출동이 가능한 소방 시설입니다.",
                    "안전교육",
                    "생활 및 공업 환경에서 발생할 수 있는 안전 사고에 대한 인지도를 높입니다.",
                    "소방드론",
                    "접근이 어려운 지역을 신속하게 정찰하고 구조 작업을 진행합니다.",
                    "재해 경보",
                    "안전관리본부",
                    "안전 관리 체계를 구축하고 효율적인 안전 활동을 전개합니다.",
                    "파출소",
                    "경범죄를 처벌하고 민간 갈등을 중재합니다.",
                    "범죄예방교육",
                    "위법 행위에 대한 주민들의 인지도를 높입니다.",
                    "경찰서",
                    "긴급 출동을 실시하고 경찰의 권력을 강화합니다.",
                    "교도소",
                    "범죄자에 대한 구속력을 강화하여 범죄를 예방합니다.",
                    "형사",
                    "잠적한 범죄자를 수사하고 찾아내어 모든 범죄자에 대한 차별 없는 법 집행을 실시합니다.",
                    "범죄수사국",
                    "범죄자의 행동 경향성과 범행 가능성을 분석하고 수사 중인 형사에게 정보를 공유하여 법 집행에 가속을 붙입니다.",
                    "상수도",
                    "시설 유지에 필요한 물을 공급합니다.",
                    "정수처리시설",
                    "오염물을 제거하고 깨끗한 물을 공급합니다.",
                    "관로 개선",
                    "수압을 높이기 위해 펌프를 설치하고 시설 내 모든 개인 공간에 관로를 연결합니다.",
                    "자동화 정수처리장",
                    "하수도",
                    "시설에서 사용한 하폐수를 모으기 위한 관거를 설치합니다.",
                    "하수처리시설",
                    "하폐수의 오염물 제거하여 재사용 가능성을 높입니다.",
                    "관거 개선",
                    "하수와 폐수의 관거를 분리하여 효과적인 오염 제거가 가능하도록 합니다.",
                    "자동화 하수처리장",
                    "작업장",
                    "공장",
                    "기계화 공장",
                    "산업단지",
                    "산업 시설의 수를 늘려 더 많은 품종의 물자를 생산합니다.",
                    "첨단산업",
                    "현 시대에 걸맞는 첨단 산업 시설을 건설합니다.",
                    "자동화 공장",
                    "교육시설",
                    "기본 학문과 생활에 필요한 교육을 행합니다.",
                    "정보센타",
                    "열람 가능한 기록과 정보를 저장하여 시민들에게 지식을 제공합니다.",
                    "고등교육시설",
                    "심화 학문에 대한 교육을 진행하여 인재를 양성합니다.",
                    "대학",
                    "고도화된 기술과 학문을 교육하여 전문가를 육성합니다.",
                    "연구시설",
                    "공학과 과학, 사회학, 경제학 등의 학문을 심도있게 살필 수 있는 환경을 마련합니다.",
                    "입자가속기",
                    "원자론",
                    "핵분열",
                    "강력",
                    "양자",
                    "핵융합",
                    "양자매트릭스",
                    "중력장",
                    "매개입자",
                    "미생물학",
                    "호기성미생물",
                    "혐기성미생물",
                    "적응미생물",
                    "기계",
                    "내연기관",
                    "전자장비",
                    "소프트웨어",
                    "인공지능",
                    "사용자학습",
                    "법치주의",
                    "경험주의",
                    "절대주의",
                    "회의주의",
                    "실존주의",
                    "허무주의",
                    "염세주의",
                    "신권주의",
                    "자유주의",
                    "상대주의",
                    "민족주의",
                    "중상주의",
                    "식민주의",
                    "인문주의",
                    "원칙주의",
                    "전체주의",
                    "제국주의",
                    "집단주의",
                    "개인주의",
                    "합리주의",
                    "세속주의",
                    "실용주의",
                    "계몽주의",
                    "팀장제",
                    "대부분이 전문가로 구성된 테라포밍 팀원 중 총괄 업무를 맡은 사람이 지도자 역할을 수행합니다.",
                    "군주제",
                    "한 사람이 국가 주권을 가진 군주가 되어 효율적인 결정 권한을 가집니다.",
                    "공화제",
                    "모든 사회 구성원이 주권을 가지고 국가 통치에 임합니다.",
                    "전제군주제",
                    "국가를 통치하는 군주가 가장 강력한 권력을 가지는 이상적인 형태의 사회를 실현합니다.",
                    "입헌군주제",
                    "헌법에 의한 군주와 의회의 권력 분립을 인정합니다.",
                    "귀족공화제",
                    "특권이 인정된 귀족이 합법적 통치 지휘를 가지는 공화제입니다.",
                    "독재주의",
                    "인민투표 등의 형태로서 대중의 지지를 받는 소수가 지배하는 사회입니다.",
                    "군국주의",
                    "군사력에 의한 대외적 발전과 치안 강화가 국가의 가장 중요한 목적이 됩니다.",
                    "사회주의",
                    "사유 재산 제도를 폐지하고 생산 수단을 사회화합니다.",
                    "민주주의",
                    "국민이 권력을 가지고 그 권력을 스스로 행사합니다.",
                    "정보사회",
                    "자동화사회",
                    "기업사회",
                    "문화사회",
                    "평등사회",
                    "관료제",
                    "봉건제",
                    "대의제",
                    "국민주권",
                    "율령",
                    "관등제",
                    "중앙집권",
                    "의회",
                    "권리장전",
                    "권력분립",
                    "과두제",
                    "원로원",
                    "호민관",
                    "정보 폐쇠",
                    "경찰국가",
                    "신뢰도 공격",
                    "개인숭배",
                    "징병제",
                    "군사화",
                    "동원령",
                    "병영국가",
                    "소득재분배",
                    "당 지도부",
                    "문화대혁명",
                    "5개년 계획",
                    "시민사회",
                    "자본주의",
                    "보통선거",
                    "뉴딜정책",
                    "임무를 철회하겠습니까?",
                    "기존 임무를 철회하고 새로운 임무를 시작하겠습니까?",
                    "당신은 테라포밍 프로젝트의 총괄을 목적으로 개발된 한 기업의 인공지능 모델입니다. 행성의 환경과 문명을 안정화하거나 다른 경쟁 기업을 잠식하는 것이 당신의 목표입니다.",
                    "임무를 재개합니다.",
                    "새로운 토지를 발견했습니다.",
                    "{도시}(이)가 건설됐습니다.",
                    "{도시}에서 범죄 조직이 발생했습니다.",
                    "{도시}에서 역병이 창궐했습니다.",
                    "{기술} 연구가 완료됐습니다.",
                    "{세력}(와)과의 거래를 개시합니다.",
                    "{세력}(와)과의 거래가 만료됐습니다.",
                    ""
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
    /// 이것이 주 목적이므로 편리한 접근을 위해 만들었다. 한국어 '키'를 입력하면 번역된 '값'을 반환한다.
    /// </summary>
    public string this[string koreanKey]
    {
        get
        {
            if (_texts.ContainsKey(koreanKey))
            {
                return _jsonLanguage.Texts[_texts[koreanKey]];
            }
            else
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
                    Debug.LogError("LoadLangeage - LanguageType 수정 요망.");
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

            // 폰트를 가져온다.
            _fontAsset = (TMP_FontAsset)Resources.Load("KoreanFont SDF");
        }

        // 대리자
        OnLanguageChange?.Invoke();
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
        File.WriteAllText($"{Application.dataPath}/Resources/Korean.Json", JsonUtility.ToJson(jsonLanguage, true));

        // 구글 번역을 위해 텍스트 파일로 저장한다.
        TextFileForGoogleTranslate(jsonLanguage);
    }


    /// <summary>
    /// 다른 언어 json 파일을 생성한다.
    /// </summary>
    public void SaveOtherLanguages()
    {
        // 준비 단계
        string[] path = Directory.GetFiles($"{Application.dataPath}/Translations/", "*.txt", SearchOption.AllDirectories);
        string jsonForm = Resources.Load("Korean").ToString();
        List<StringBuilder> words = new List<StringBuilder>();
        StringBuilder result = new StringBuilder();

        // 존재하는 모든 번역본 생성
        for (byte i = 0; i < path.Length; ++i)
        {
            // 언어 하나 준비 단계
            string text = File.ReadAllText(path[i]);
            ushort index = 0;
            words.Clear();

            // 단어 추출
            for (int j = 0; j < text.Length; ++j)
            {
                if (';' == text[j])
                {
                    // 다음 단어
                    j += 2;
                    ++index;
                }
                else
                {
                    // 가변배열에 추가 안 됐으면 추가
                    if (index == words.Count)
                    {
                        words.Add(new StringBuilder());
                    }

                    // 단어 기록
                    words[index].Append(text[j]);
                }
            }

            // json화 시작 준비 단계
            int count = 0;
            bool proceed = true;
            index = 0;
            result.Clear();

            // json 형식 따라가기
            for (int j = 0; j < jsonForm.Length; ++j)
            {
                // 기록
                if (proceed)
                {
                    result.Append(jsonForm[j]);
                }

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
                            // 새로운 단어 저장
                            if (index < words.Count)
                            {
                                result.Append(words[index]);
                            }

                            result.Append('\"');

                            // 다음 단어
                            ++index;

                            // 기록 중지
                            proceed = false;
                        }
                        // 큰따옴표 닫혔을 때
                        else
                        {
                            // 기록 재개
                            proceed = true;
                        }
                    }
                }
            }

            // json 파일로 저장
            File.WriteAllText($"{Application.dataPath}/Resources/{Path.GetFileNameWithoutExtension(path[i])}.Json", result.ToString());
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
