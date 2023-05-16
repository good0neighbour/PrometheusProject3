using System.Collections.Generic;
using TMPro;

public class PopUpViewFacility : TechTreeBase
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

        // ��� ������ ����
        _nodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // ���� ��� Ȱ��ȭ
        foreach (TechTrees.Node.SubNode nextNodes in NextNodes[CurrentNode])
        {
            switch (nextNodes.Type)
            {
                case TechTreeType.Facility:
                    if (EnableCheck(nextNodes))
                    {
                        byte index = NodeIndex[nextNodes.NodeName];
                        _enabled[index] = true;
                        NodeBtnObjects[index].SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }


    protected override void OnFail()
    {

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



    /* ==================== Private Methods ==================== */

    protected override bool EnableCheck(TechTrees.Node.SubNode nextNode)
    {
        // ���� ���� ������ ��
        TechTrees.Node.SubNode[] requiredNodes = NodeData[NodeIndex[nextNode.NodeName]].Requirments;
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
            NodeBtnObjects[i].SetActive(_enabled[i]);

            // ���� �Ϸ�, �̿Ϸ�
            if (_adopted[i])
            {
                _nodeIcons[i].text = Constants.FACILITY_ADOPTED;
            }
            else
            {
                _nodeIcons[i].text = Constants.FACILITY_UNADOPTED;
            }
        }
    }
}
