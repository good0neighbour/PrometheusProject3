using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TechTreeBase : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("부모클래스")]
    [Header("노드 간격")]
    [SerializeField] protected float Width = 100.0f;
    [SerializeField] protected float Height = 100.0f;

    [Header("참조")]
    [SerializeField] protected TMP_Text AdoptBtn = null;
    [SerializeField] protected TMP_Text Description = null;
    [SerializeField] protected TMP_Text GainsText = null;
    [SerializeField] protected TMP_Text CostsText = null;
    [SerializeField] protected TMP_Text StatusText = null;
    [SerializeField] protected TMP_Text BackBtn = null;
    [SerializeField] protected Image ProgressionImage = null;
    [SerializeField] protected GameObject NodeObject = null;
    [SerializeField] protected Transform TechTreeContentArea = null;
    [SerializeField] private GameObject _cursor = null;

    protected TechTrees.Node[] NodeData = null;
    protected TechTreeNode[] NodeBtns = null;
    protected TMP_Text[] NodeIcons = null;
    protected byte CurrentNode = 0;
    protected bool IsAdoptAvailable = false;
    protected bool IsBackAvailable = true;
    private GameObject[] _nodeBtnObjects = null;
    private Transform _cursorTransform = null;
    private float _supportRate = 0.0f;
    private float _timer = 0.0f;
    private bool _runAdoptProgression = false;



    /* ==================== Public Methods ==================== */

    public virtual void BtnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 승인 버튼 재사용 금지
        IsAdoptAvailable = false;

        // 뒤로가기 금지
        IsBackAvailable = false;
        BackBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // 상태 메세지 제거
        StatusText.text = null;
    }


    public void SetCurrentNode(byte current, Vector3 position)
    {
        // 현재 노드 정보
        CurrentNode = current;

        // 커서 위치
        _cursorTransform.position = position;
        if (!_cursor.activeSelf)
        {
            _cursor.SetActive(true);
        }

        // 비용 확인
        if (CostAvailable())
        {
            IsAdoptAvailable = true;
            AdoptBtn.color = Constants.WHITE;
        }
        else
        {
            IsAdoptAvailable = false;
            AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
        }

        // 상태 메세지 제거
        StatusText.text = null;
    }


    public void BtnBack()
    {
        // 사용 불가
        if (!IsBackAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 창 비활성화
        gameObject.SetActive(false);

        // 처음 상태로 되돌린다.
        _cursor.SetActive(false);
        IsAdoptAvailable = false;
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
        Description = null;
        GainsText = null;
        CostsText = null;
        StatusText = null;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 승인 애니메이션 시작
    /// </summary>
    protected void AdoptAnimation(float supportRate)
    {
        _runAdoptProgression = true;
        _supportRate = supportRate;
    }


    /// <summary>
    /// 승인 성공 시
    /// </summary>
    protected abstract void OnAdopt();

    /// <summary>
    /// 승인 실패 시
    /// </summary>
    protected abstract void OnFail();



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 비용 확인
    /// </summary>
    private bool CostAvailable()
    {
        // 비용 확인
        if (NodeData[CurrentNode].FundCost > PlayManager.Instance[VariableLong.Funds])
        {
            return false;
        }

        // 사용 가능
        return true;
    }


    private void Awake()
    {
        // 참조
        _cursorTransform = _cursor.transform;

        // 승인 버튼 사용 불가
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // 노드 정보 가져오기
        NodeData = PlayManager.Instance.GetTechTreeData().GetFacilityNodes();

        // 배열 생성
        byte length = (byte)NodeData.Length;
        NodeBtns = new TechTreeNode[length];
        _nodeBtnObjects = new GameObject[length];
        NodeIcons = new TMP_Text[length];

        // 노드 배치
        float sizeX = 0.0f;
        float sizeY = 0.0f;
        for (byte i = 0; i < length; ++i)
        {
            // 위치 계산
            float posX = (NodeData[i].NodePosition.x + 0.5f) * Width;
            float posY = (NodeData[i].NodePosition.y + 0.5f) * Height;

            // 노드 생성 후 위치 조정
            byte nodeIndex = (byte)NodeData[i].Tag;
            NodeBtns[nodeIndex] = Instantiate(NodeObject, TechTreeContentArea).GetComponent<TechTreeNode>();
            NodeBtns[nodeIndex].transform.localPosition = new Vector3(posX, posY, 0.0f);

            // 노드 초기화
            NodeBtns[nodeIndex].SetTechTree(this, nodeIndex);

            // 노드 참조
            _nodeBtnObjects[nodeIndex] = NodeBtns[nodeIndex].gameObject;
            NodeIcons[nodeIndex] = _nodeBtnObjects[nodeIndex].GetComponentInChildren<TMP_Text>();

            // x, y 최대 값
            if (posX > sizeX)
            {
                sizeX = posX;
            }
            if (posY > sizeY)
            {
                sizeY = posY;
            }
        }

        // 전체 크기
        float areaWidth = sizeX + Width * 0.5f;
        RectTransform contentArea = TechTreeContentArea.GetComponent<RectTransform>();
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, areaWidth);
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeY + Height * 0.5f);

        // 가운데 정렬
        float pivotX = (Constants.TECHTREE_AREA_CENTER * Constants.TECHTREE_AREA_WIDTH - areaWidth * 0.5f) / (Constants.TECHTREE_AREA_WIDTH - areaWidth);
        if (0.0f > pivotX)
        {
            pivotX = 0.0f;
        }
        contentArea.pivot = new Vector2(pivotX, 0.0f);
    }


    private void Update()
    {
        if (_runAdoptProgression)
        {
            // 애니메이션 진행
            _timer += Time.deltaTime;
            ProgressionImage.fillAmount = _timer;

            // 애니메이션 완료
            if (1.0f <= _timer)
            {
                if (_supportRate >= Random.Range(0.0f, 100.0f))
                {
                    // 성공
                    OnAdopt();
                }
                else
                {
                    // 실패
                    OnFail();
                }

                // 복귀
                ProgressionImage.fillAmount = 0.0f;
                _runAdoptProgression = false;
                _timer = 0.0f;

                // 뒤로가기 가능
                IsBackAvailable = true;
                BackBtn.color = Constants.WHITE;
            }
        }
    }
}
