using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TechTreeBase : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("�θ�Ŭ����")]
    [Header("��� ����")]
    [SerializeField] protected float Width = 100.0f;
    [SerializeField] protected float Height = 100.0f;

    [Header("����")]
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
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��ư ���� ����
        IsAdoptAvailable = false;

        // �ڷΰ��� ����
        IsBackAvailable = false;
        BackBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // ���� �޼��� ����
        StatusText.text = null;
    }


    public void SetCurrentNode(byte current, Vector3 position)
    {
        // ���� ��� ����
        CurrentNode = current;

        // Ŀ�� ��ġ
        _cursorTransform.position = position;
        if (!_cursor.activeSelf)
        {
            _cursor.SetActive(true);
        }

        // ��� Ȯ��
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

        // ���� �޼��� ����
        StatusText.text = null;
    }


    public void BtnBack()
    {
        // ��� �Ұ�
        if (!IsBackAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // â ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ó�� ���·� �ǵ�����.
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
    /// ���� �ִϸ��̼� ����
    /// </summary>
    protected void AdoptAnimation(float supportRate)
    {
        _runAdoptProgression = true;
        _supportRate = supportRate;
    }


    /// <summary>
    /// ���� ���� ��
    /// </summary>
    protected abstract void OnAdopt();

    /// <summary>
    /// ���� ���� ��
    /// </summary>
    protected abstract void OnFail();



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� Ȯ��
    /// </summary>
    private bool CostAvailable()
    {
        // ��� Ȯ��
        if (NodeData[CurrentNode].FundCost > PlayManager.Instance[VariableLong.Funds])
        {
            return false;
        }

        // ��� ����
        return true;
    }


    private void Awake()
    {
        // ����
        _cursorTransform = _cursor.transform;

        // ���� ��ư ��� �Ұ�
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // ��� ���� ��������
        NodeData = PlayManager.Instance.GetTechTreeData().GetFacilityNodes();

        // �迭 ����
        byte length = (byte)NodeData.Length;
        NodeBtns = new TechTreeNode[length];
        _nodeBtnObjects = new GameObject[length];
        NodeIcons = new TMP_Text[length];

        // ��� ��ġ
        float sizeX = 0.0f;
        float sizeY = 0.0f;
        for (byte i = 0; i < length; ++i)
        {
            // ��ġ ���
            float posX = (NodeData[i].NodePosition.x + 0.5f) * Width;
            float posY = (NodeData[i].NodePosition.y + 0.5f) * Height;

            // ��� ���� �� ��ġ ����
            byte nodeIndex = (byte)NodeData[i].Tag;
            NodeBtns[nodeIndex] = Instantiate(NodeObject, TechTreeContentArea).GetComponent<TechTreeNode>();
            NodeBtns[nodeIndex].transform.localPosition = new Vector3(posX, posY, 0.0f);

            // ��� �ʱ�ȭ
            NodeBtns[nodeIndex].SetTechTree(this, nodeIndex);

            // ��� ����
            _nodeBtnObjects[nodeIndex] = NodeBtns[nodeIndex].gameObject;
            NodeIcons[nodeIndex] = _nodeBtnObjects[nodeIndex].GetComponentInChildren<TMP_Text>();

            // x, y �ִ� ��
            if (posX > sizeX)
            {
                sizeX = posX;
            }
            if (posY > sizeY)
            {
                sizeY = posY;
            }
        }

        // ��ü ũ��
        float areaWidth = sizeX + Width * 0.5f;
        RectTransform contentArea = TechTreeContentArea.GetComponent<RectTransform>();
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, areaWidth);
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeY + Height * 0.5f);

        // ��� ����
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
            // �ִϸ��̼� ����
            _timer += Time.deltaTime;
            ProgressionImage.fillAmount = _timer;

            // �ִϸ��̼� �Ϸ�
            if (1.0f <= _timer)
            {
                if (_supportRate >= Random.Range(0.0f, 100.0f))
                {
                    // ����
                    OnAdopt();
                }
                else
                {
                    // ����
                    OnFail();
                }

                // ����
                ProgressionImage.fillAmount = 0.0f;
                _runAdoptProgression = false;
                _timer = 0.0f;

                // �ڷΰ��� ����
                IsBackAvailable = true;
                BackBtn.color = Constants.WHITE;
            }
        }
    }
}
