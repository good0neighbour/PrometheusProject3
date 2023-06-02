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
                    "불러오는 중",
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
                    "전설적인 락 밴드, 어두운 내면을 노래하는 힙합, 우아한 외모와 춤동작으로 대중에게 인기를 끄는 댄스 가수 등 국가의 문화를 해외에 알립니다.",
                    "동양화",
                    "과거 지구의 한 열도에서 시작된 독특한 작화가 전세계 곳곳에 마니아를 만들어냈습니다. 해당 문화에 매료된 시민들은 그 특유한 작화를 자신의 기술로 받아들이고 더 많은 문화 콘텐츠를 만들어냅니다.",
                    "의복 유행",
                    "플레이어 국가의 유행이 세련됐다고 믿는 사람은 플레이어 국가 시민의 옷차림을 멋의 기준으로 삼습니다.",
                    "영화 수출",
                    "해당 국가를 배경으로 하는 영화를 플레이어 국가에서 제작합니다. 모든 제작물은 플레이어의 언어를 사용하고, 이러한 영화에 매료된 시민들은 해당 국가를 배경으로 하는 영화를 보기 위해 플레이어 국가의 제작물을 찾을 것입니다. 플레이어의 문화가 해당 국가를 압도할 수록 효과가 증가합니다.",
                    "문화 편견",
                    "미디어에서 특정 민족에 대해 무술, 종이접기, 마피아 등 특정한 이미지를 각인시킵니다. 시민들은 해당 민족에 대해 각인된 이미지 이외에 다른 것을 보지 못 합니다. 플레이어의 문화가 해당 국가를 압도할 수록 효과가 증가합니다.",
                    "자국 영웅화",
                    "전쟁 영화, 영웅 영화 등에서 가장 강력하고 정의로운 인물은 항상 플레이어 국가 출신입니다. 해당 국가의 시민들은 플레이어 국가를 정의로운 국가로 인식하고 자신의 국가를 하찮게 여깁니다. 플레이어의 문화가 해당 국가를 압도할 수록 효과가 증가합니다.",
                    "역사 기원",
                    "해당 국가의 역사적, 문화적 기원이 모두 플레이어 국가에 있음을 알립니다. 이는 학술적 검증을 회피하거나 플레이어 국가의 맞춤형 검증을 거치고 미디어에 노출시킵니다. 플레이어의 문화가 해당 국가를 압도하지 않으면 역효과가 발생합니다.",
                    "역사 비하",
                    "해당 국가의 왕족, 귀족, 문화 등을 비하하고 플레이어 국가의 역사를 우상으로 여기는 영화, 드라마 등을 방영합니다. 플레이어의 문화가 해당 국가를 압도하지 않으면 역효과가 발생합니다.",
                    "정치인 압박",
                    "속국화 작업을 위해 해당 국가의 정치인을 압박하거나 매수합니다. 압박당한 정치인은 플레이어 국가에 대한 적대자 수를 줄이려 할 것입니다. 플레이어의 문화가 해당 국가를 압도하지 않으면 역효과가 발생합니다.",
                    "극단적 채식주의 선동",
                    "올바름이라는 단어로 위장하여 극단적인 동물 보호를 위해 평범한 식욕을 가진 인간을 희생합니다. 동물에 대한 폭력을 막기 위해 인간에게 폭언, 폭행을 가하며 해당 국가에 내부갈등을 유발합니다.",
                    "약자 혐오 선동",
                    "노인, 여성, 장애인 등에 대한 비하 단어를 창조해 유행시키고 이들에 대한 불쾌한 감정을 느끼도록 유도합니다. 이들을 비하하는 시민은 자신의 감정을 객관적인 지표로 사용하고 비하 행위를 정의로 여기며 자신의 행동에 잘못된 점을 인지하지 못합니다. 이는 해당 국가에 내부갈등을 유발합니다.",
                    "전자 페미니즘 선동",
                    "정치적 우위를 차지하기 위해 페미니즘의 이름을 사용하는 집단을 출현시킵니다. 고의로 자극적인 발언을 하며 자신들의 존재를 알리고 본래 페미니즘의 명예를 실추시킵니다. 이들은 해당 국가에 내부갈등을 유발합니다.",
                    "창조과학론 투입",
                    "신의 창조와 법칙을 거부하고 오직 자신의 눈만이 진실이라 믿는 이교도 집단을 형성합니다. 이들은 학계에 발표된 내용을 자신들의 목적에 맞게 편집해 신의 진리를 왜곡합니다. 해당 국가는 국력을 들여 이러한 음모론에 방어합니다.",
                    "설거지론 투입",
                    "성비불균형에서 비롯된 현상을 모든 국가에 적용 가능한 이야기처럼 꾸밉니다. 시민 간의 불신을 야기하고 혼인률 저하, 저출산 등을 유도해 장기적으로 해당 국가의 국력에 타격을 가합니다.",
                    "정치 음모론 투입",
                    "영화 소재로 사용될 법한 정부 음모에 대한 이야기가 현실에서 발생한다는 믿음을 줍니다. 음모론을 뒷받침할 근거는 턱없이 부족하지만 비전문가에게는 상당히 설득력있게 들리는 탓에 모든 상황에서 정부를 불신하며 특정 세력의 개입을 의심합니다.",
                    "탐사를 시작하면 채굴할 광물이 매장된 토지를 획득할 수 있습니다. 혹은 당신의 경쟁 기업이 세운 또다른 국가와 조우할 수도 있습니다.",
                    "행성의 대기량을 조절하여 적합한 대기압을 만드십시오. 자금을 투자하여 대기량을 증가시키거나 감소시킬 수 있습니다. 기압은 행성의 온도와 물 순환, 탄소 순환에 영향을 줍니다. 이상적인 기압은 1013.25hPa입니다.",
                    "생명이 정착하기 적절한 온도를 형성하십시오. 행성의 온도는 물 순환에 영향을 줍니다. 행성으로 유입되는 열과 방출되는 열은 평형을 이루기 때문에 자금 투자에 의한 온도 변화는 일시적입니다. 이상적인 온도는 15℃입니다.",
                    "생명이 살아가는 데에 물은 필수적입니다. 액체 상태의 물은 생명 정착을 시작할 수 있게 하고 기체 상태의 물은 기온 상승을 유발함과 동시에 기압 상승에 미미한 영향을 줍니다. 그래프를 참고하여 물의 체적을 이상적인 양으로 조절할 수 있습니다.",
                    "탄소는 생명의 기본 재료이면서 문명을 위한 재료이기도 합니다. 자금을 투자하여 기권으로의 탄소 유입을 조절할 수 있으나 높은 농도의 탄소가 기권으로 유입되면 기온 상승을 유발합니다.",
                    "자금을 투자하여 지구로부터 종자를 보급받을 수 있습니다. 생물은 액체 상태의 물, 기압, 온도, 탄소 순환의 안정도에 영향을 받습니다. 광합성 생물을 산소를 발생시킵니다.",
                    "자금을 투자하여 지구로부터 종자를 보급받을 수 있습니다. 생물은 액체 상태의 물, 기압, 온도, 탄소 순환의 안정도에 영향을 받습니다. 호흡 생물이 생존하기 위해서는 산소를 필요로 합니다.",
                    "=== 철 설명 ===",
                    "=== 핵물질 설명 ===",
                    "=== 보석 설명 ===",
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
                    "재해경보",
                    "=== 재해경보 설명 ===",
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
                    "최소한의 관리가 필요한 자동화된 정수처리장입니다.",
                    "하수도",
                    "시설에서 사용한 하폐수를 모으기 위한 관거를 설치합니다.",
                    "하수처리시설",
                    "하폐수의 오염물 제거하여 재사용 가능성을 높입니다.",
                    "관거 개선",
                    "하수와 폐수의 관거를 분리하여 효과적인 오염 제거가 가능하도록 합니다.",
                    "자동화 하수처리장",
                    "적응 미생물을 활용하여 처리하기 어려운 오염물질을 정화합니다.",
                    "작업장",
                    "생산활동을 위한 시민의 작업 공간입니다.",
                    "공장",
                    "자원을 가공하고 제품을 생산할 설비를 갖춘 공간입니다.",
                    "기계화 공장",
                    "기계를 사용하여 생산 공정이 자동화된 공장입니다.",
                    "산업단지",
                    "산업 시설의 수를 늘려 더 많은 품종의 물자를 생산합니다.",
                    "첨단산업",
                    "현 시대에 걸맞는 첨단 산업 시설을 건설합니다.",
                    "자동화 공장",
                    "관리 체계가 자동으로 이루어지는 공장입니다.",
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
                    "안전의 이유로 모행성에서 행하지 못한 실험을 신행성에서 진행합니다.",
                    "원자론",
                    "모든 물질을 이루는 입자를 연구합니다.",
                    "핵분열",
                    "원자핵이 분열하는 현상을 연구합니다.",
                    "강력",
                    "원자핵의 결합 힘을 연구합니다.",
                    "양자",
                    "물질의 최소 단위를 연구합니다.",
                    "핵융합",
                    "가벼운 원자가 무거운 원자로 결합하는 과정을 연구합니다.",
                    "양자역학",
                    "미시세계를 다루는 기초 이론을 연구합니다.",
                    "중력장",
                    "중력이 작용하는 공간을 연구합니다.",
                    "매개입자",
                    "두 입자 사이에 힘을 매개하는 입자를 연구합니다.",
                    "미생물학",
                    "미생물에 관해 연구합니다.",
                    "호기성미생물",
                    "산소 호흡하는 미생물을 연구합니다.",
                    "혐기성미생물",
                    "무산소로 성장하는 미생물을 연구합니다.",
                    "적응미생물",
                    "주위 환경에 따라 적응하는 미생물을 연구합니다.",
                    "기계",
                    "동력을 들여 특정 동작을 하는 장치를 연구합니다.",
                    "내연기관",
                    "실린더 내의 연료 폭발을 동력으로 하는 기계를 연구합니다.",
                    "전자장비",
                    "전기 신호로 작동하는 장비를 연구합니다.",
                    "정보통신",
                    "=== 정보통신 설명 ===",
                    "소프트웨어",
                    "전자장비를 동작하게 할 프로그램을 연구합니다.",
                    "인공지능",
                    "인간의 지능을 가지는 프로그램을 연구합니다.",
                    "사용자학습",
                    "사용자의 특성을 실시간으로 학습하는 인공지능을 연구합니다.",
                    "법치주의",
                    "법률로써 백성을 다스리는 사상입니다.",
                    "경험주의",
                    "경험의 내용이 곧 인식의 내용이 된다는 이론입니다.",
                    "절대주의",
                    "군주에게 절대적인 권력을 부여하는 정치사상입니다.",
                    "회의주의",
                    "진리의 절대성을 의심하고 궁극적인 판단을 하지 않으려는 사상입니다.",
                    "실존주의",
                    "개인으로서의 인간의 주체적 존재성을 강조하는 철학입니다.",
                    "허무주의",
                    "사물이나 현상은 존재하거나 인식되지 않고 아무 가치도 지니지 않는다는 사상입니다.",
                    "염세주의",
                    "삶을 불행하고 비참한 것으로 보고 개혁이나 진보는 불가능하다고 보는 사상입니다.",
                    "신권주의",
                    "통치자의 권력이 신에게서 받은 신성한 것이라고 여기는 사상입니다.",
                    "자유주의",
                    "개인 인격의 존엄성을 인정하고 개성을 자발적으로 발전시키고자 하는 사상입니다.",
                    "상대주의",
                    "진리나 가치의 절대적 타당성을 부인하고 모든 것은 상대적이라고 보는 사상입니다.",
                    "민족주의",
                    "민족의 독립과 통일을 가장 중시하는 사상입니다.",
                    "중상주의",
                    "나라의 부를 늘리고 상업을 중요하게 여기는 사상입니다.",
                    "식민주의",
                    "식민지의 획득과 유지를 지향하는 대외 정책입니다.",
                    "인문주의",
                    "인간의 존재를 중요시하고 인간의 능력과 성품 그리고 인간의 현재적 소망과 행복을 귀중하게 생각하는 사상입니다.",
                    "원칙주의",
                    "기본적인 규칙과 법칙대로 하려는 사상입니다.",
                    "전체주의",
                    "개인의 모든 활동은 민족, 국가와 같은 전체의 존립과 발전을 위하여서만 존재한다는 이념 아래 개인의 자유를 억압하는 사상입니다.",
                    "제국주의",
                    "우월한 군사력과 경제력으로 다른 나라나 민족을 정벌하여 대국가를 건설하려는 침략주의적 사상입니다.",
                    "집단주의",
                    "개인의 이익보다는 집단의 이익을 존중하는 사상입니다.",
                    "개인주의",
                    "국가나 사회에 대하여 개인의 가치를 인정하는 사상입니다.",
                    "합리주의",
                    "이성이나 논리적 타당성에 근거하여 사물을 인식하거나 판단하는 사상입니다.",
                    "세속주의",
                    "사회 제도나 그 운영 등에서 종교적 영향력을 제거하고 세속과 종교 각각의 독립적인 영역을 구분하고 인정하는 사상입니다.",
                    "실용주의",
                    "실제 결과가 진리를 판단하는 기준이라고 주장하는 사상입니다.",
                    "계몽주의",
                    "인류의 무한한 진보를 위하여 이성의 힘으로 현존질서를 타파하고 사회를 개혁하려는 사상입니다.",
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
                    "=== 정보사회 설명 ===",
                    "자동화사회",
                    "=== 자동화사회 설명 ===",
                    "기업사회",
                    "=== 기업사회 설명 ===",
                    "문화사회",
                    "=== 문화사회 설명 ===",
                    "평등사회",
                    "=== 평등사회 설명 ===",
                    "관료제",
                    "특권을 가진 관료가 국가 권력을 장악하고 지배합니다.",
                    "봉건제",
                    "영주가 가신에게 봉토를 주고 그 대신에 군역의 의무를 부과하는 주종 관계를 기본으로 한 통치 제도입니다.",
                    "대의제",
                    "국민이 스스로 선출한 대표자를 통하여 국가 권력을 행사합니다.",
                    "국민주권",
                    "국민이 대의 기관을 통하거나 직접적으로 입법 및 그 밖의 국정 사항을 결정하는 권력을 가집니다.",
                    "율령",
                    "법률을 나라의 기본으로 합니다.",
                    "관등제",
                    "관리나 벼슬의 등급을 규정짓습니다.",
                    "중앙집권",
                    "국가의 통치 권력을 중앙 정부에 집중합니다.",
                    "의회",
                    "입법 및 기타 중요한 국가 작용에 참여하는 권능을 가진 합의체입니다.",
                    "권리장전",
                    "의회의 입법권과 과세 승인권 따위를 규정함으로써 왕권을 제약합니다.",
                    "권력분립",
                    "한 개인이나 집단 또는 특정 기관에 권력이 집중되는 것을 방지할 목적으로 권력을 분할, 배치하여 상호 견제와 균형을 이룹니다.",
                    "과두제",
                    "적은 수의 우두머리가 국가의 최고 기관을 조직하여 행합니다.",
                    "원로원",
                    "입법, 자문 기관으로 내정과 외교를 지도합니다.",
                    "호민관",
                    "군사적인 문제를 처리하거나 시민들을 업무를 수행합니다.",
                    "정보 폐쇠",
                    "경찰국가",
                    "경찰권을 마음대로 행사하여 국민 생활을 감시하고 통제합니다.",
                    "신뢰도 공격",
                    "=== 신뢰도 공격 설명 ===",
                    "개인숭배",
                    "독재자를 우상화하고 떠받들어 모십니다.",
                    "징병제",
                    "국민 모두에게 강제적으로 병역을 의무화 합니다.",
                    "군사화",
                    "국가를 군사화합니다.",
                    "동원령",
                    "전쟁 따위의 비상사태가 발생하였을 때 병력이나 군수 물자 따위를 동원하기 위해 명령을 내립니다.",
                    "병영국가",
                    "모든 면에서 군사적 관점을 우선시 합니다.",
                    "소득재분배",
                    "정책적으로 소득 분포를 수정합니다.",
                    "당 지도부",
                    "=== 당 지도부 설명 ===",
                    "문화대혁명",
                    "=== 문화대혁명 설명 ===",
                    "5개년 계획",
                    "국민경제 발전을 위해 국가에서 5개년 단위로 수립하는 경제계획입니다.",
                    "시민사회",
                    "신분적 구속에 지배되지 않으며, 자유롭고 평등한 개인의 이성적 결합으로 이루어진 사회입니다.",
                    "자본주의",
                    "생산 수단을 자본으로서 소유한 자본가가 이윤 획득을 위하여 생산 활동을 하도록 보장합니다.",
                    "보통선거",
                    "선거인의 자격에 재산, 신분, 성별, 교육 정도 따위의 제한을 두지 않고 성년에 도달하면 누구에게나 선거권이 주어집니다.",
                    "뉴딜정책",
                    "=== 뉴딜정책 설명 ===",
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
                    "개발자용 치트를 사용했습니다. 자금 +50000",
                    "개발자용 치트를 사용했습니다. 속국화 90%",
                    "임무 완료",
                    "임무 실패",
                    "생물 안정도가 일정 수준 이상에 도달했습니다. 이 우주에 생명이 살아가는 또다른 행성이 탄생한 순간입니다. 당신은 임무를 완료했습니다.",
                    "모든 세력을 당신의 속국으로 만들었습니다. 이제 전 행성의 주권은 당신의 세력에게 있습니다. 당신은 임무를 완료했습니다.",
                    "당신의 세력은 행성 테라포밍의 최다 공로를 인정받았습니다.",
                    "당신의 세력은 행성을 정복하여 그 강력함을 인정받았습니다.",
                    "이상적인 {물리량}에 가까워지고 있습니다.",
                    "이상적인 {물리량}에서 멀어지고 있습니다.",
                    "{생물}의 생존률이 올라갑니다.",
                    "{생물}이 생존할 수 없습니다.",
                    "시민들이 새로운 사회를 받아들여 다음 시대가 시작됐습니다.",
                    "{도시}에 범죄율이 높습니다. 치안 시설을 승인해 범죄를 통제하십시오.",
                    "{도시}에 범죄율이 안정되고 있습니다.",
                    "{도시}에 사망률이 높습니다. 의료 시설을 승인해 질병을 통제하십시오.",
                    "{도시}에 사망률이 낮아지고 있습니다.",
                    "{도시}의 안정도가 낮습니다. 안정도가 낮으면 세금 수입이 적어집니다.",
                    "{도시}의 안정도가 상승합니다. 시민들이 당신의 통치에 만족해합니다.",
                    "다수의 {세력} 시민들이 플레이어 국가에 우호적입니다. 해당 국가의 시민들은 플레이어의 행동을 쉽게 받아들일 것입니다.",
                    "플레이어 국가에 대한 {세력} 시민들의 우호도가 낮아지고 있습니다.",
                    "다수의 {세력} 시민들이 플레이어 국가에 적대적입니다. 해당 국가와의 교역에서 거래 가격이 플레이어에게 불리해집니다.",
                    "플레이어 국가에 대한 {세력} 시민들의 적대감이 낮아지고 있습니다.",
                    "{세력}(이)가 혼란 상태입니다. 끊임없는 내부갈등과 무능한 정부의 모습에 일부 시민들은 새로운 지배자를 원합니다.",
                    "{세력}(이)가 혼란을 극복하고 있습니다.",
                    "{세력}(이)가 플레이어 국가의 속국이 되었습니다. 많은 시민들은 절망하였지만 플레이어에게 우호적인 세력은 기꺼이 당신에게 나라는 바칩니다.",
                    "{생물} 종자를 요청했습니다. {0}개월 후에 지구로부터 도착 예정입니다.",
                    "지구로부터 {생물} 종자가 도착했습니다. 증식을 시작합니다.",
                    "산소 농도가 너무 높습니다. 호흡 생물의 생존률에 악영향을 줍니다.",
                    "산소 농도가 낮아지고 있습니다.",
                    "이 게임에 등장하는 물리, 사회을 포함한 모든 컨셉은 현실과 차이가 있습니다.",
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

    /// <summary>
    /// 인덱스로 직접 접근
    /// </summary>
    public string this[ushort index]
    {
        get
        {
            return _jsonLanguage.Texts[index];
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
    /// 문자열의 인덱스 번호 반환
    /// </summary>
    public ushort GetLanguageIndex(string koreanKey)
    {
        return _texts[koreanKey];
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

        // 대리자
        OnLanguageChange?.Invoke();
        GameManager.Instance.CurrentLanguage = language;
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



    /* ==================== UNITY_EDITOR ==================== */

#if UNITY_EDITOR
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
#endif
}
