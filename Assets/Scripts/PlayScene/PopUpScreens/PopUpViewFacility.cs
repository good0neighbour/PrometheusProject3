using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class PopUpViewFacility : TechTreeViewBase
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _nodeIcons = null;
    private bool[] _enabled = null;
    private bool[] _adopted = null;
    private City _currentCity = null;



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
        //AdoptAnimation(PlayManager.Instance[VariableFloat.FacilitySupportRate]);
        AdoptAnimation(75.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // Ȱ��ȭ ���� ����
        _adopted[CurrentNode] = true;

        // ������ ���
        PlayManager.Instance[VariableFloat.FacilitySupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (100.0f < PlayManager.Instance[VariableFloat.FacilitySupportRate])
        {
            PlayManager.Instance[VariableFloat.FacilitySupportRate] = 100.0f;
        }

        // ������ ��� �̳����̼�
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);

        // ��� ������ ����
        _nodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;

        // ���� ��� Ȱ��ȭ
        foreach (TechTrees.SubNode nextNodes in NextNodes[CurrentNode])
        {
            switch (nextNodes.Type)
            {
                case TechTreeType.Facility:
                    if (EnableCheck(nextNodes))
                    {
                        byte index = NodeIndex[nextNodes.NodeName];
                        _enabled[index] = true;
                        SetIcon(index);
                        NodeBtnObjects[index].SetActive(true);
                    }
                    break;
                case TechTreeType.Society:
                    // ��ȸ ��ũƮ������ �ൿ���� �ʴ´�.
                    break;
                default:
                    break;
            }
        }
    }


    protected override void OnFail()
    {
        // ������ ����
        PlayManager.Instance[VariableFloat.FacilitySupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (0.0f > PlayManager.Instance[VariableFloat.FacilitySupportRate])
        {
            PlayManager.Instance[VariableFloat.FacilitySupportRate] = 0.0f;
        }

        // ������ �ִϸ��̼�
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);
    }


    protected override bool IsUnadopted()
    {
        bool result = _adopted[NodeIndex[NodeData[CurrentNode].NodeName]];
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


    protected override bool EnableCheck(TechTrees.SubNode nextNode)
    {
        // ���� ���� ������ ��
        TechTrees.SubNode[] requiredNodes = NodeData[NodeIndex[nextNode.NodeName]].Requirments;
        for (int i = 0; i < requiredNodes.Length; i++)
        {
            // ��� ���ε� ���� �ƴϸ� ���� ��ȯ
            switch (requiredNodes[i].Type)
            {
                case TechTreeType.Facility:
                    if (!_adopted[NodeIndex[requiredNodes[i].NodeName]])
                    {
                        return false;
                    }
                    break;
                default:
                    if (1.0f > Adopted[(int)requiredNodes[i].Type][NodeIndex[requiredNodes[i].NodeName]])
                    {
                        return false;
                    }
                    break;
            }
        }

        // ��� ���� ������ �� ��ȯ
        return true;
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
                    case TechTreeType.Tech:
                        result.Append($"{Language.Instance["���ȭ ���� ����"]} - {requirments[i].NodeName}\n");
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
    /// ��� ���� �Ϸ�, �̿Ϸ� ǥ��
    /// </summary>
    private void SetIcon(byte index)
    {
        if (_adopted[index])
        {
            _nodeIcons[index].text = Constants.FACILITY_ADOPTED;
        }
        else
        {
            _nodeIcons[index].text = Constants.FACILITY_UNADOPTED;
        }
    }


    private void Awake()
    {
        // ��� ���� ��������
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Facility);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Facility);

        // �迭 ����
        byte length = (byte)NodeData.Length;
        _nodeIcons = new TMP_Text[length];
        
        BasicInitialize(length);

        // ����
        for (byte i = 0; i < length; ++i)
        {
            _nodeIcons[i] = NodeBtnObjects[i].GetComponentInChildren<TMP_Text>();
        }
    }


    private void OnEnable()
    {
        // ���� ���� ����
        _currentCity = ScreenCity.Instance.CurrentCity;

        // �迭 ����
        _enabled = _currentCity.GetFacilityEnabled();
        _adopted = _currentCity.GetFacilityAdopted();

        // Ȱ��ȭ, ��Ȱ��ȭ
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // ��� ���� ����
            if (_enabled[i])
            {
                // ��� ����
                NodeBtnObjects[i].SetActive(true);

                // ���� �Ϸ�, �̿Ϸ�
                SetIcon(i);
            }
            else
            {
                TechTrees.SubNode[] requiredNode = NodeData[i].Requirments;
                bool enable = true;

                for (byte j = 0; j < requiredNode.Length; ++j)
                {
                    switch (requiredNode[j].Type)
                    {
                        case TechTreeType.Facility:
                            if (!_adopted[NodeIndex[requiredNode[j].NodeName]])
                            {
                                // ��� �Ұ�
                                enable = false;
                            }
                            break;
                        default:
                            if (1.0f > Adopted[(int)requiredNode[j].Type][NodeIndex[requiredNode[j].NodeName]])
                            {
                                // ��� �Ұ�
                                enable = false;
                            }
                            break;
                    }

                    if (!enable)
                    {
                        break;
                    }
                }

                if (enable)
                {
                    // ���� �Ϸ�, �̿Ϸ�
                    SetIcon(i);

                    // Ȱ��ȭ
                    NodeBtnObjects[i].SetActive(true);
                }
                else
                {
                    //  ��Ȱ��ȭ
                    NodeBtnObjects[i].SetActive(false);
                }
            }
        }
    }
}
