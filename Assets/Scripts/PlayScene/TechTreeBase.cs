using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;

public abstract class TechTreeBase : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("부모클래스")]
    [Header("노드 간격")]
    [SerializeField] private float _width = 100.0f;
    [SerializeField] private float _height = 100.0f;

    [Header("참조")]
    [SerializeField] protected TMP_Text AdoptBtn = null;
    [SerializeField] protected TMP_Text GainsText = null;
    [SerializeField] protected TMP_Text StatusText = null;
    [SerializeField] protected GameObject NodeObject = null;
    [SerializeField] private TMP_Text _costsText = null;
    [SerializeField] private TMP_Text _backBtn = null;
    [SerializeField] private TMP_Text _descriptionText = null;
    [SerializeField] private Image _progressionImage = null;
    [SerializeField] private GameObject _cursor = null;
    [SerializeField] private Transform _techTreeContentArea = null;

    protected Dictionary<string, byte> NodeIndex = null;
    protected TechTrees.Node[] NodeData = null;
    protected GameObject[] NodeBtnObjects = null;
    protected TechTreeNode[] NodeBtns = null;
    protected TMP_Text[] NodeIcons = null;
    protected byte CurrentNode = 0;
    protected bool[][] Adopted = null;
    protected bool IsAdoptAvailable = false;
    protected bool IsBackAvailable = true;
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
        _backBtn.color = Constants.TEXT_BUTTON_DISABLE;

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
        SetAdoptButtonAvailable(CostAvailable());

        // 설명 텍스트 업데이트
        _descriptionText.text = $"[{NodeData[CurrentNode].NodeName}]\n{NodeData[CurrentNode].Description}";

        // 비용 텍스트 업데이트
        _costsText.text = GetCostText();

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

        // 게임 재개
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // 처음 상태로 되돌린다.
        _cursor.SetActive(false);
        SetAdoptButtonAvailable(false);
        _descriptionText.text = null;
        GainsText.text = null;
        _costsText.text = null;
        StatusText.text = null;
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


    protected void BasicInitialize(byte nodeLength)
    {
        // 참조
        NodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        Adopted = PlayManager.Instance.GetAdoptedData();
        _cursorTransform = _cursor.transform;

        // 승인 버튼 사용 불가
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // 배열 생성
        NodeBtns = new TechTreeNode[nodeLength];
        NodeBtnObjects = new GameObject[nodeLength];
        NodeIcons = new TMP_Text[nodeLength];

        // 노드 배치
        float sizeX = 0.0f;
        float sizeY = 0.0f;
        for (byte i = 0; i < nodeLength; ++i)
        {
            // 위치 계산
            float posX = (NodeData[i].NodePosition.x + 0.5f) * _width;
            float posY = (NodeData[i].NodePosition.y + 0.5f) * _height;

            // 노드 생성 후 위치 조정
            NodeBtns[i] = Instantiate(NodeObject, _techTreeContentArea).GetComponent<TechTreeNode>();
            NodeBtns[i].transform.localPosition = new Vector3(posX, posY, 0.0f);
            
            // 노드 초기화
            NodeBtns[i].SetTechTree(this, i);
            
            // 노드 참조
            NodeBtnObjects[i] = NodeBtns[i].gameObject;
            NodeIcons[i] = NodeBtnObjects[i].GetComponentInChildren<TMP_Text>();

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
        float areaWidth = sizeX + _width * 0.5f;
        RectTransform contentArea = _techTreeContentArea.GetComponent<RectTransform>();
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, areaWidth);
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY + _height * 0.5f);

        // 가운데 정렬
        float pivotX = (Constants.TECHTREE_AREA_CENTER * Constants.TECHTREE_AREA_WIDTH - areaWidth * 0.5f) / (Constants.TECHTREE_AREA_WIDTH - areaWidth);
        if (0.0f > pivotX)
        {
            pivotX = 0.0f;
        }
        contentArea.pivot = new Vector2(pivotX, 0.0f);
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
        //if (NodeData[CurrentNode].IronCost > PlayManager.Instance[VariableLong.Funds])
        //{
        //    return false;
        //}
        //if (NodeData[CurrentNode].NukeCost > PlayManager.Instance[VariableLong.Funds])
        //{
        //    return false;
        //}

        // 사용 가능
        return true;
    }


    private void SetAdoptButtonAvailable(bool available)
    {
        if (available)
        {
            IsAdoptAvailable = true;
            AdoptBtn.color = Constants.WHITE;
        }
        else
        {
            IsAdoptAvailable = false;
            AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
        }
    }


    /// <summary>
    /// 비용 텍스트 생성한다.
    /// </summary>
    private string GetCostText()
    {
        // 문자열 만들기
        StringBuilder result = new StringBuilder();
        result.Append($"[{Language.Instance["비용"]}]\n");

        // 0 이상인 비용 표시
        if (0 < NodeData[CurrentNode].FundCost)
        {
            result.Append($"{Language.Instance["자금"]} {NodeData[CurrentNode].FundCost}\n");
        }
        if (0 < NodeData[CurrentNode].IronCost)
        {
            result.Append($"{Language.Instance["철"]} {NodeData[CurrentNode].IronCost}\n");
        }
        if (0 < NodeData[CurrentNode].NukeCost)
        {
            result.Append($"{Language.Instance["핵물질"]} {NodeData[CurrentNode].NukeCost}\n");
        }

        // 반환
        return result.ToString();
    }


    private void Update()
    {
        if (_runAdoptProgression)
        {
            // 애니메이션 진행
            _timer += Time.deltaTime;
            _progressionImage.fillAmount = _timer;

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
                _progressionImage.fillAmount = 0.0f;
                _runAdoptProgression = false;
                _timer = 0.0f;

                // 비용 확인
                SetAdoptButtonAvailable(CostAvailable());

                // 뒤로가기 가능
                IsBackAvailable = true;
                _backBtn.color = Constants.WHITE;
            }
        }
    }
}
