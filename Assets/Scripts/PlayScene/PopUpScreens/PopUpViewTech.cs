using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Text;

public class PopUpViewTech : TechTreeViewBase, IActivateFirst
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _titleTexts = null;
    private TMP_Text[] _remainTexts = null;
    private Image[] _progreesionImages = null;
    private List<byte> _onProgress = new List<byte>();



    /* ==================== Public Methods ==================== */

    public override void BtnAdopt()
    {
        // ��� �Ұ�
        if (!IsAdoptAvailable)
        {
            return;
        }

        // �θ� Ŭ������ ���� ����
        base.BtnAdopt();

        // ���� �ִϸ��̼�
        AdoptAnimation(PlayManager.Instance[VariableFloat.ResearchSupportRate]);
    }


    public void Activate()
    {
        // ��� ���� ��������
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Tech);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Tech);

        // �迭 ����
        byte length = (byte)NodeData.Length;
        _titleTexts = new TMP_Text[length];
        _remainTexts = new TMP_Text[length];
        _progreesionImages = new Image[length];

        // UIString ��� ���� ���·� �����.
        UIString.Instance.TechInitialize(length);

        BasicInitialize(length);

        for (byte i = 0; i < length; ++i)
        {
            // ����
            Transform node = NodeBtnObjects[i].transform;
            _titleTexts[i] = node.Find("TextTitle").GetComponent<TMP_Text>();
            _remainTexts[i] = node.transform.Find("TextRemain").GetComponent<TMP_Text>();
            _progreesionImages[i] = node.transform.Find("ImageProgressionBackground").Find("ImageProgression").GetComponent<Image>();

            // ���� ���� ���̾��� �� ���� �迭�� �߰�
            if (0.0f < Adopted[(int)TechTreeType.Tech][i])
            {
                _onProgress.Add(i);
            }

            // ��Ȱ��ȭ�� ��
            TechTrees.SubNode[] requiredNodes = NodeData[i].Requirments;
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // ��� ���ε� ���� �ƴϸ� ��Ȱ��ȭ
                if (0.0f >= Adopted[(int)requiredNodes[j].Type][NodeIndex[requiredNodes[j].NodeName]])
                {
                    NodeBtnObjects[i].SetActive(false);
                    break;
                }
            }

            // ���⼭ ProgressionValue�� ���� �ӵ��� ����.
            if (0.0f >= NodeData[i].ProgressionValue)
            {
                // 0�̸� 0.03���� �ٲ۴�.
                NodeData[i].ProgressionValue = 0.3f;
            }
        }

        // �ؽ�Ʈ ������Ʈ
        OnLanguageChange();

        // �븮�� ���
        Language.OnLanguageChange += OnLanguageChange;
        PlayManager.OnPlayUpdate += TechResearchProgress;
    }


    /// <summary>
    /// ���� ���� �� ��� �����´�.
    /// </summary>
    public List<byte> GetProgressionList()
    {
        return _onProgress;
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // Ȱ��ȭ ���� ����
        Adopted[(int)TechTreeType.Tech][CurrentNode] = 0.0001f;

        // ������ ���
        PlayManager.Instance[VariableFloat.ResearchSupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (100.0f < PlayManager.Instance[VariableFloat.ResearchSupportRate])
        {
            PlayManager.Instance[VariableFloat.ResearchSupportRate] = 100.0f;
        }

        // ������ ��� �̳����̼�
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);

        // ���� �������� ����Ʈ�� �߰�
        _onProgress.Add(CurrentNode);

        // ���� ��� Ȱ��ȭ
        List<TechTrees.SubNode> nextNodes = NextNodes[CurrentNode];
        for (byte i = 0; i < nextNodes.Count; ++i)
        {
            switch (nextNodes[i].Type)
            {
                case TechTreeType.Tech:
                    // ���� ����� ���� �����ϴ� �������� Ȱ��ȭ�Ѵ�.
                    if (EnableCheck(nextNodes[i]))
                    {
                        byte index = NodeIndex[nextNodes[i].NodeName];
                        NodeBtnObjects[index].SetActive(true);
                        NodeUpdate(index);
                    }
                    break;
                case TechTreeType.Society:
                    // ��ȸ ��ũƮ������ �ൿ���� �ʴ´�.
                    break;
                default:
                    // �ٸ� ��ũƮ���� Ȱ��ȭ ���θ� �ش� ��ũƮ���� ������ �����Ѵ�.
                    break;
            }
        }
    }


    protected override void OnFail()
    {
        // ������ ����
        PlayManager.Instance[VariableFloat.ResearchSupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (0.0f > PlayManager.Instance[VariableFloat.ResearchSupportRate])
        {
            PlayManager.Instance[VariableFloat.ResearchSupportRate] = 0.0f;
        }

        // ������ �ִϸ��̼�
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);
    }


    protected override bool IsUnadopted()
    {
        bool result = (0.0f < Adopted[(int)TechTreeType.Tech][NodeIndex[NodeData[CurrentNode].NodeName]]);
        if (result)
        {
            AdoptBtn.text = Language.Instance["���� �Ϸ�"];
        }
        else
        {
            AdoptBtn.text = Language.Instance["����"];
        }
        return !result;
    }


    protected override string GetGainText()
    {
        StringBuilder result = new StringBuilder();
        result.Append($"[{Language.Instance["����"]}]\n");

        TechTrees.Node node = NodeData[CurrentNode];
        if (0 < node.AnnualFund)
        {
            result.Append($"{Language.Instance["���� �ڱ�"]} {node.AnnualFund.ToString()}\n");
        }
        if (0 < node.AnnualResearch)
        {
            result.Append($"{Language.Instance["���� ����"]} {node.AnnualFund.ToString()}\n");
        }
        if (0 < node.AnnualCulture)
        {
            result.Append($"{Language.Instance["���� ��ȭ"]} {node.AnnualFund.ToString()}\n");
        }

        // ���� ��� ����
        List<TechTrees.SubNode> requirments = NextNodes[CurrentNode];
        if (0 < requirments.Count)
        {
            for (byte i = 0; i < requirments.Count; ++i)
            {
                switch (requirments[i].Type)
                {
                    case TechTreeType.Facility:
                        result.Append($"{Language.Instance["�ü� ��� ����"]} - {requirments[i].NodeName}\n");
                        break;
                    case TechTreeType.Thought:
                        result.Append($"{Language.Instance["��� ���� ����"]} - {requirments[i].NodeName}\n");
                        break;
                    case TechTreeType.Society:
                        result.Append($"{Language.Instance["��ȸ ä�� ����"]} - {requirments[i].NodeName}\n");
                        break;
                    default:
                        // �������� ǥ������ �ʴ´�.
                        break;
                }
            }
        }

        // ��ȯ. ������ \n�� �����Ѵ�.
        return result.Remove(result.Length - 1, 1).ToString();
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� ǥ�� ������Ʈ
    /// </summary>
    private void NodeUpdate(byte index)
    {
        // ���� ����
        _remainTexts[index].text = UIString.Instance.GetRemainString(TechTreeType.Tech, index, NodeData);

        // ���� ���� �̹���
        _progreesionImages[index].fillAmount = Adopted[(int)TechTreeType.Tech][index];
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    private void TechResearchProgress()
    {
        for (byte i = 0; i < _onProgress.Count; ++i)
        {
            // ���� ����
            Adopted[(int)TechTreeType.Tech][_onProgress[i]] += NodeData[_onProgress[i]].ProgressionValue * Constants.MONTHLY_MULTIPLY * PlayManager.Instance.GameSpeed * Time.deltaTime;

            // ���� �Ϸ�
            if (1.0f <= Adopted[(int)TechTreeType.Tech][_onProgress[i]])
            {
                // ���� ���� ���� ����Ʈ���� ����
                _onProgress.Remove(_onProgress[i]);

                // ������ ���� �迭 �ε����� �����.
                --i;
            }
        }
    }


    private void OnLanguageChange()
    {
        for (byte i = 0; i < _titleTexts.Length; ++i)
        {
            // ��� �̸�. 
            _titleTexts[i].text = NodeData[i].NodeName;
        }
    }


    private void OnEnable()
    {
        // Ȱ��ȭ, ��Ȱ��ȭ
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // �̹� Ȱ��ȭ �� ��
            if (NodeBtnObjects[i].activeSelf)
            {
                NodeUpdate(i);
            }
            // ��Ȱ��ȭ�� ��
            else
            {
                TechTrees.SubNode[] requiredNode = NodeData[i].Requirments;
                bool enable = true;

                for (byte j = 0; j < requiredNode.Length; ++j)
                {
                    if (1.0f > Adopted[(int)requiredNode[j].Type][NodeIndex[requiredNode[j].NodeName]])
                    {
                        // ��� �Ұ�
                        enable = false;
                    }

                    if (!enable)
                    {
                        break;
                    }
                }

                // ��� ������ ���
                if (enable)
                {
                    NodeBtnObjects[i].SetActive(true);
                    NodeUpdate(i);
                }
            }
        }
    }
}
