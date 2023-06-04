using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TechTreeViewBase : MonoBehaviour, IState
{
    /* ==================== Variables ==================== */

    [Header("�θ�Ŭ����")]
    [Header("��� ����")]
    [SerializeField] private float _width = 100.0f;
    [SerializeField] private float _height = 100.0f;
    [SerializeField] private bool _yCenterize = false;

    [Header("����")]
    [SerializeField] protected TMP_Text AdoptBtn = null;
    [SerializeField] protected TMP_Text GainsText = null;
    [SerializeField] protected TMP_Text StatusText = null;
    [SerializeField] protected GameObject NodeObject = null;
    [SerializeField] private TMP_Text _costsText = null;
    [SerializeField] private TMP_Text _backBtn = null;
    [SerializeField] private TMP_Text _descriptionText = null;
    [SerializeField] private Image _progressionImage = null;
    [SerializeField] private Transform _techTreeContentArea = null;
    [SerializeField] private GameObject _cursor = null;
    [SerializeField] private GameObject _previousScreen = null;

    protected List<TechTrees.SubNode>[] NextNodes = null;
    protected Dictionary<string, byte> NodeIndex = null;
    protected TechTrees.Node[] NodeData = null;
    protected GameObject[] NodeBtnObjects = null;
    protected TechTreeNode[] NodeBtns = null;
    protected TechTrees TechTreeData = null;
    protected byte CurrentNode = 0;
    protected float[][] Adopted = null;
    protected bool IsAdoptAvailable = false;
    protected bool IsBackAvailable = true;
    private Transform _cursorTransform = null;
    private float _supportRate = 0.0f;
    private float _timer = 0.0f;
    private bool _runAdoptProgression = false;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ���� ��ư ����
    /// </summary>
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

        // ��� ����
        FirstSpendCost();
    }


    /// <summary>
    /// ���� ��� ����
    /// </summary>
    public void SetCurrentNode(byte current, Vector3 position)
    {
        // �ִϸ��̼� ���� �߿��� �۵� �Ұ�
        if (_runAdoptProgression)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��� ����
        CurrentNode = current;

        // Ŀ�� ��ġ
        _cursorTransform.position = position;
        if (!_cursor.activeSelf)
        {
            _cursor.SetActive(true);
        }

        // ��� Ȯ��
        SetAdoptButtonAvailable(IsUnadopted() && CostAvailable());

        // ���� �ؽ�Ʈ ������Ʈ
        _descriptionText.text = $"[{Language.Instance[NodeData[CurrentNode].NodeName]}]\n{Language.Instance[NodeData[CurrentNode].DescriptionNum]}";

        // ��� �ؽ�Ʈ ������Ʈ
        _costsText.text = GetCostText();

        // ���� �ؽ�Ʈ ������Ʈ
        GainsText.text = GetGainText();

        // ���� �޼��� ����
        StatusText.text = null;
    }


    public void Execute()
    {
        // ó�� ���·� �ǵ�����.
        _cursor.SetActive(false);
        SetAdoptButtonAvailable(false);
        AdoptBtn.text = Language.Instance["����"];
        _descriptionText.text = null;
        GainsText.text = null;
        _costsText.text = null;
        StatusText.text = null;

        // �� ��ũƮ�� Ȱ��ȭ
        gameObject.SetActive(true);
    }


    public void ChangeState()
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

        // ���� ȭ�� Ȱ��ȭ
        _previousScreen.SetActive(true);
    }



    /* ==================== Protected Methods ==================== */

    /// <summary>
    /// ���� �ִϸ��̼� ����
    /// </summary>
    protected void AdoptAnimation(float supportRate)
    {
        _runAdoptProgression = true;
        _supportRate = supportRate;
    }


    /// <summary>
    /// �⺻���� ��ũƮ�� �ʱ�ȭ
    /// </summary>
    protected void BasicInitialize(byte length)
    {
        // ����
        NodeIndex = TechTreeData.GetIndexDictionary();
        Adopted = PlayManager.Instance.GetAdoptedData();
        _cursorTransform = _cursor.transform;

        // ���� ��ư ��� �Ұ�
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // �迭 ����
        NodeBtns = new TechTreeNode[length];
        NodeBtnObjects = new GameObject[length];

        // ��� ��ġ
        float sizeX = 0.0f;
        float sizeY = 0.0f;
        for (byte i = 0; i < length; ++i)
        {
            // ��ġ ���
            float posX = (NodeData[i].NodePosition.x + 0.5f) * _width;
            float posY = (NodeData[i].NodePosition.y + 0.5f) * _height;

            // ��� ���� �� ��ġ ����
            NodeBtnObjects[i] = Instantiate(NodeObject, _techTreeContentArea);
            NodeBtnObjects[i].transform.localPosition = new Vector3(posX, posY, 0.0f);

            // ��� ����
            NodeBtns[i] = NodeBtnObjects[i].GetComponent<TechTreeNode>();
            
            // ��� �ʱ�ȭ
            NodeBtns[i].SetTechTree(this, i);

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
        sizeX += _width * 0.5f;
        sizeY += _height * 0.5f;
        RectTransform contentArea = _techTreeContentArea.GetComponent<RectTransform>();
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeX);
        contentArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeY);

        // ��� ����
        float pivotX = 0.0f;
        float pivotY = 0.0f;
        if (Constants.TECHTREE_AREA_WIDTH > sizeX)
        {
            pivotX = (Constants.TECHTREE_AREA_WIDTH_CENTER * Constants.TECHTREE_AREA_WIDTH - sizeX * 0.5f) / (Constants.TECHTREE_AREA_WIDTH - sizeX);
        }
        if (_yCenterize && Constants.TECHTREE_AREA_HEIGHT > sizeY)
        {
            pivotY = (Constants.TECHTREE_AREA_HEIGHT_CENTER * Constants.TECHTREE_AREA_HEIGHT - sizeY * 0.5f) / (Constants.TECHTREE_AREA_HEIGHT - sizeY);
        }
        contentArea.pivot = new Vector2(pivotX, pivotY);
    }


    /// <summary>
    /// ���� ��� Ȱ��ȭ ���� ����
    /// </summary>
    protected virtual bool EnableCheck(TechTrees.SubNode nextNode)
    {
        // ���� ���� ������ ��
        TechTrees.SubNode[] requiredNodes = NodeData[NodeIndex[nextNode.NodeName]].Requirments;
        for (byte i = 0; i < requiredNodes.Length; ++i)
        {
            // ��� ���ε� ���� �ƴϸ� ���� ��ȯ
            if (0.0f >= Adopted[(int)requiredNodes[i].Type][NodeIndex[requiredNodes[i].NodeName]])
            {
                return false;
            }
        }

        // ��� ���ε����� �� ��ȯ
        return true;
    }


    /// <summary>
    /// ���� �ؽ�Ʈ �����Ѵ�.
    /// </summary>
    protected abstract string GetGainText();


    /// <summary>
    /// ���� ���� ��
    /// </summary>
    protected abstract void OnAdopt();


    /// <summary>
    /// ���� ���� ��
    /// </summary>
    protected abstract void OnFail();


    /// <summary>
    /// ���� ��� �������� Ȯ��
    /// </summary>
    protected abstract bool IsUnadopted();



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
        if (NodeData[CurrentNode].ResearchCost > PlayManager.Instance[VariableUint.Research])
        {
            return false;
        }
        if (NodeData[CurrentNode].CultureCost > PlayManager.Instance[VariableUint.Culture])
        {
            return false;
        }
        if (NodeData[CurrentNode].IronCost > PlayManager.Instance[VariableUshort.CurrentIron])
        {
            return false;
        }
        if (NodeData[CurrentNode].NukeCost > PlayManager.Instance[VariableUshort.CurrentNuke])
        {
            return false;
        }

        // ��� ����
        return true;
    }


    /// <summary>
    /// ���� ��ư Ȱ��ȭ, ��Ȱ��ȭ
    /// </summary>
    /// <param name="available"></param>
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
        if (0 < NodeData[CurrentNode].ResearchCost)
        {
            result.Append($"{Language.Instance["����"]} {NodeData[CurrentNode].ResearchCost}\n");
        }
        if (0 < NodeData[CurrentNode].CultureCost)
        {
            result.Append($"{Language.Instance["��ȭ"]} {NodeData[CurrentNode].CultureCost}\n");
        }
        if (0 < NodeData[CurrentNode].IronCost)
        {
            result.Append($"{Language.Instance["ö"]} {NodeData[CurrentNode].IronCost}\n");
        }
        if (0 < NodeData[CurrentNode].NukeCost)
        {
            result.Append($"{Language.Instance["�ٹ���"]} {NodeData[CurrentNode].NukeCost}\n");
        }
        if (0 < NodeData[CurrentNode].Maintenance)
        {
            result.Append($"{Language.Instance["�������"]} {NodeData[CurrentNode].Maintenance}\n");
        }
        if (0 < NodeData[CurrentNode].Injure)
        {
            result.Append($"{Language.Instance["�λ� ����"]}\n");
        }

        // ��ȯ. ������ \n�� �����Ѵ�.
        return result.Remove(result.Length - 1, 1).ToString();
    }


    /// <summary>
    /// ���� ��ư Ŭ�� �� ����
    /// </summary>
    private void FirstSpendCost()
    {
        // �ڱ� ��� ����
        if (0 < NodeData[CurrentNode].FundCost)
        {
            PlayManager.Instance[VariableLong.Funds] -= NodeData[CurrentNode].FundCost;
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Fund);
        }

        // ���� ��� ����
        if (0 < NodeData[CurrentNode].ResearchCost)
        {
            PlayManager.Instance[VariableUint.Research] -= NodeData[CurrentNode].ResearchCost;
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Research);
        }

        // ��ȭ ��� ����
        if (0 < NodeData[CurrentNode].CultureCost)
        {
            PlayManager.Instance[VariableUint.Culture] -= NodeData[CurrentNode].CultureCost;
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Culture);
        }
    }


    /// <summary>
    /// ���� ���� �� ����
    /// </summary>
    private void LastSpendCost()
    {
        // ö ��� ����
        if (0 < NodeData[CurrentNode].IronCost)
        {
            PlayManager.Instance[VariableUshort.CurrentIron] -= NodeData[CurrentNode].IronCost;
            PlayManager.Instance[VariableUshort.IronUsage] += NodeData[CurrentNode].IronCost;
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Iron);
        }

        // �ٹ��� ��� ����
        if (0 < NodeData[CurrentNode].NukeCost)
        {
            PlayManager.Instance[VariableUshort.CurrentNuke] -= NodeData[CurrentNode].NukeCost;
            PlayManager.Instance[VariableUshort.NukeUsage] += NodeData[CurrentNode].NukeCost;
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Nuke);
        }
    }


    /// <summary>
    /// ����
    /// </summary>
    private void Gains()
    {
        TechTrees.Node node = NodeData[CurrentNode];

        PlayManager.Instance[VariableInt.AnnualFund] += node.AnnualFund;
        PlayManager.Instance[VariableUshort.AnnualResearch] += node.AnnualResearch;
        PlayManager.Instance[VariableUshort.AnnualCulture] += node.AnnualCulture;
        PlayManager.Instance[VariableUint.Maintenance] += node.Maintenance;
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
                if (_supportRate >= Random.Range(0.0f, Constants.MAX_SUPPORT_RATE_ADOPTION))
                {
                    // �Ҹ� ���
                    AudioManager.Instance.PlayAuido(AudioType.Select);

                    // ���� �� ����
                    OnAdopt();

                    // ���� ��ư �ؽ�Ʈ ����
                    AdoptBtn.text = Language.Instance["���� �Ϸ�"];

                    // ���� �޼���
                    StatusText.color = Constants.WHITE;
                    StatusText.text = Language.Instance["��å ����"];

                    // ���� ��ư ��� �Ұ�
                    AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

                    // ��� ����
                    LastSpendCost();

                    // ����
                    Gains();
                }
                else
                {
                    // �Ҹ� ���
                    AudioManager.Instance.PlayAuido(AudioType.Failed);

                    // ���� �� ����
                    OnFail();

                    // ���� �޼���
                    StatusText.color = Constants.FAIL_TEXT;
                    StatusText.text = Language.Instance["��å ����"];

                    // ��� Ȯ�� �� ���� ��ư �ٽ� Ȱ��ȭ
                    SetAdoptButtonAvailable(CostAvailable());
                }

                // ����
                _progressionImage.fillAmount = 0.0f;
                _runAdoptProgression = false;
                _timer = 0.0f;

                // �ڷΰ��� ����
                IsBackAvailable = true;
                _backBtn.color = Constants.WHITE;
            }
        }
    }
}
