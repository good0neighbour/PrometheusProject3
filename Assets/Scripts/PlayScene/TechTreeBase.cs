using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;

public abstract class TechTreeBase : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("�θ�Ŭ����")]
    [Header("��� ����")]
    [SerializeField] private float _width = 100.0f;
    [SerializeField] private float _height = 100.0f;

    [Header("����")]
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
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��ư ���� ����
        IsAdoptAvailable = false;

        // �ڷΰ��� ����
        IsBackAvailable = false;
        _backBtn.color = Constants.TEXT_BUTTON_DISABLE;

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
        SetAdoptButtonAvailable(CostAvailable());

        // ���� �ؽ�Ʈ ������Ʈ
        _descriptionText.text = $"[{NodeData[CurrentNode].NodeName}]\n{NodeData[CurrentNode].Description}";

        // ��� �ؽ�Ʈ ������Ʈ
        _costsText.text = GetCostText();

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

        // ���� �簳
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // ó�� ���·� �ǵ�����.
        _cursor.SetActive(false);
        SetAdoptButtonAvailable(false);
        _descriptionText.text = null;
        GainsText.text = null;
        _costsText.text = null;
        StatusText.text = null;
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


    protected void BasicInitialize(byte nodeLength)
    {
        // ����
        NodeIndex = PlayManager.Instance.GetTechTreeData().GetIndexDictionary();
        Adopted = PlayManager.Instance.GetAdoptedData();
        _cursorTransform = _cursor.transform;

        // ���� ��ư ��� �Ұ�
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // �迭 ����
        NodeBtns = new TechTreeNode[nodeLength];
        NodeBtnObjects = new GameObject[nodeLength];
        NodeIcons = new TMP_Text[nodeLength];

        // ��� ��ġ
        float sizeX = 0.0f;
        float sizeY = 0.0f;
        for (byte i = 0; i < nodeLength; ++i)
        {
            // ��ġ ���
            float posX = (NodeData[i].NodePosition.x + 0.5f) * _width;
            float posY = (NodeData[i].NodePosition.y + 0.5f) * _height;

            // ��� ���� �� ��ġ ����
            NodeBtns[i] = Instantiate(NodeObject, _techTreeContentArea).GetComponent<TechTreeNode>();
            NodeBtns[i].transform.localPosition = new Vector3(posX, posY, 0.0f);
            
            // ��� �ʱ�ȭ
            NodeBtns[i].SetTechTree(this, i);
            
            // ��� ����
            NodeBtnObjects[i] = NodeBtns[i].gameObject;
            NodeIcons[i] = NodeBtnObjects[i].GetComponentInChildren<TMP_Text>();

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
        float areaWidth = sizeX + _width * 0.5f;
        RectTransform contentArea = _techTreeContentArea.GetComponent<RectTransform>();
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, areaWidth);
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY + _height * 0.5f);

        // ��� ����
        float pivotX = (Constants.TECHTREE_AREA_CENTER * Constants.TECHTREE_AREA_WIDTH - areaWidth * 0.5f) / (Constants.TECHTREE_AREA_WIDTH - areaWidth);
        if (0.0f > pivotX)
        {
            pivotX = 0.0f;
        }
        contentArea.pivot = new Vector2(pivotX, 0.0f);
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
        //if (NodeData[CurrentNode].IronCost > PlayManager.Instance[VariableLong.Funds])
        //{
        //    return false;
        //}
        //if (NodeData[CurrentNode].NukeCost > PlayManager.Instance[VariableLong.Funds])
        //{
        //    return false;
        //}

        // ��� ����
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
    /// ��� �ؽ�Ʈ �����Ѵ�.
    /// </summary>
    private string GetCostText()
    {
        // ���ڿ� �����
        StringBuilder result = new StringBuilder();
        result.Append($"[{Language.Instance["���"]}]\n");

        // 0 �̻��� ��� ǥ��
        if (0 < NodeData[CurrentNode].FundCost)
        {
            result.Append($"{Language.Instance["�ڱ�"]} {NodeData[CurrentNode].FundCost}\n");
        }
        if (0 < NodeData[CurrentNode].IronCost)
        {
            result.Append($"{Language.Instance["ö"]} {NodeData[CurrentNode].IronCost}\n");
        }
        if (0 < NodeData[CurrentNode].NukeCost)
        {
            result.Append($"{Language.Instance["�ٹ���"]} {NodeData[CurrentNode].NukeCost}\n");
        }

        // ��ȯ
        return result.ToString();
    }


    private void Update()
    {
        if (_runAdoptProgression)
        {
            // �ִϸ��̼� ����
            _timer += Time.deltaTime;
            _progressionImage.fillAmount = _timer;

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
                _progressionImage.fillAmount = 0.0f;
                _runAdoptProgression = false;
                _timer = 0.0f;

                // ��� Ȯ��
                SetAdoptButtonAvailable(CostAvailable());

                // �ڷΰ��� ����
                IsBackAvailable = true;
                _backBtn.color = Constants.WHITE;
            }
        }
    }
}
