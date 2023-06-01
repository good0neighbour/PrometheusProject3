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
            AudioManager.Instance.PlayAuido(AudioType.Unable);
            return;
        }

        // �θ� Ŭ������ ���� ����
        base.BtnAdopt();

        // ���� �ִϸ��̼�
        AdoptAnimation(PlayManager.Instance[VariableFloat.FacilitySupportRate]);
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

        // �ü� �� ����
        ++_currentCity.NumOfFacility;

        // �ü� ����
        FacilityGains();

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
            result.Append($"{Language.Instance["���� ����"]} {node.AnnualResearch.ToString()}\n");
        }
        if (0 < node.AnnualCulture)
        {
            result.Append($"{Language.Instance["���� ��ȭ"]} {node.AnnualCulture.ToString()}\n");
        }
        if (0 < node.Stability)
        {
            result.Append($"{Language.Instance["������ ����"]} {node.AnnualCulture.ToString()}\n");
        }
        if (0 < node.PopulationMovement)
        {
            result.Append($"{Language.Instance["�α� ����"]}\n");
        }
        if (0 < node.Police)
        {
            result.Append($"{Language.Instance["������ ����"]}\n");
        }
        if (0 < node.Health)
        {
            result.Append($"{Language.Instance["���� ����"]}\n");
        }
        if (0 < node.Safety)
        {
            result.Append($"{Language.Instance["�λ� ����"]}\n");
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
                        result.Append($"{Language.Instance["���ȭ ���� ����"]} - {Language.Instance[requirments[i].NodeName]}\n");
                        break;
                    case TechTreeType.Thought:
                        result.Append($"{Language.Instance["��� ���� ����"]} - {Language.Instance[requirments[i].NodeName]}\n");
                        break;
                    case TechTreeType.Society:
                        result.Append($"{Language.Instance["��ȸ ä�� ����"]} - {Language.Instance[requirments[i].NodeName]}\n");
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


    /// <summary>
    /// �ü� ����
    /// </summary>
    private void FacilityGains()
    {
        _currentCity.PopulationMovementMultiply += NodeData[CurrentNode].PopulationMovement;
        _currentCity.AnnualFund += (short)(NodeData[CurrentNode].AnnualFund - NodeData[CurrentNode].Maintenance);
        _currentCity.AnnualResearch += NodeData[CurrentNode].AnnualResearch;
        _currentCity.InjurePosibility += NodeData[CurrentNode].Injure;
        _currentCity.Police += NodeData[CurrentNode].Police;
        _currentCity.Health += NodeData[CurrentNode].Health;
        _currentCity.Safety += NodeData[CurrentNode].Safety;
        _currentCity.StabilityAdd += NodeData[CurrentNode].Stability;
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
        _enabled = _currentCity.FacilityEnabled;
        _adopted = _currentCity.FacilityAdopted;

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
