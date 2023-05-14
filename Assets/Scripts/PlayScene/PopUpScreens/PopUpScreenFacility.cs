using System.Collections.Generic;

public class PopUpScreenFacility : TechTreeBase
{
    /* ==================== Variables ==================== */

    private City _currentCity = null;
    private List<byte>[] _nextNodes = null;
    bool[] _enabled = null;
    bool[] _adopted = null;



    /* ==================== Public Methods ==================== */

    public override void BtnAdopt()
    {
        // 사용 불가
        if (!IsAdoptAvailable)
        {
            return;
        }

        // 부모 클래스의 동작 수행
        base.BtnAdopt();

        // 승인 애니메이션
        //AdoptAnimation(PlayManager.Instance[VariableFloat.FacilitySupportRate]);
        AdoptAnimation(75.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 활성화 정보 전달
        _adopted[CurrentNode] = true;

        // 노드 아이콘 변경
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // 다음 노드 활성화
        List<byte> nextNodes = _nextNodes[CurrentNode];
        for (byte i = 0; i < nextNodes.Count; ++i)
        {
            if (EnableCheck(nextNodes[i]))
            {
                _enabled[nextNodes[i]] = true;
                NodeBtnObjects[nextNodes[i]].SetActive(true);
            }
        }

        // 상태 메세지
        StatusText.color = Constants.WHITE;
        StatusText.text = Language.Instance["승인 완료"];

        // 승인 버튼 사용 불가
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
    }


    protected override void OnFail()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Failed);

        // 상태 메세지
        StatusText.color = Constants.FAIL_TEXT;
        StatusText.text = Language.Instance["승인 실패"];

        // 승인 버튼 사용 가능
        IsAdoptAvailable = true;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 다음 노드 활성화 가능 여부
    /// </summary>
    private bool EnableCheck(byte nextNode)
    {
        // 이전 노드로 설정된 것
        FaciityTag[] previous = NodeData[nextNode].PreviousNodes;
        for (int i = 0; i < previous.Length; i++)
        {
            // 모두 승인된 것이 아니면 거짓 반환
            if (!_adopted[(int)previous[i]])
            {
                return false;
            }
        }

        // 모두 승인 됐으면 참 반환
        return true;
    }


    protected override void Awake()
    {
        // 부모클래스 함수 먼저 호출
        base.Awake();

        // 다음 노드 가변 배열 생성
        byte length = (byte)NodeData.Length;
        _nextNodes = new List<byte>[length];

        // 다음 노드 등록
        for (byte i = 0; i < length; ++i)
        {
            // 이전 노드 정보
            FaciityTag[] previousNodes = NodeData[i].PreviousNodes;

            // 다음 노드로 등록
            for (byte j = 0; j < (byte)previousNodes.Length; ++j)
            {
                // 가변배열 생성한 적 없으면 생성
                if (null == _nextNodes[(int)previousNodes[j]])
                {
                    _nextNodes[(int)previousNodes[j]] = new List<byte>();
                }

                // 이전 노드로 설정된 것에 현재 노드를 다음 것으로 등록
                _nextNodes[(int)previousNodes[j]].Add(i);
            }
        }
    }


    private void OnEnable()
    {
        // 현재 도시 정보
        _currentCity = ScreenCity.Instance.CurrentCity;

        // 배열 참조
        _enabled = _currentCity.GetFacilityEnabled();
        _adopted = _currentCity.GetFacilityAdopted();

        // 활성화, 비활성화
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // 사용 가능 여부
            NodeBtnObjects[i].SetActive(_enabled[i]);

            // 승인 완료, 미완료
            if (_adopted[i])
            {
                NodeIcons[i].text = Constants.FACILITY_ADOPTED;
            }
            else
            {
                NodeIcons[i].text = Constants.FACILITY_UNADOPTED;
            }
        }
    }
}
