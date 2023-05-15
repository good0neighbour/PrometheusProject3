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
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // Ȱ��ȭ ���� ����
        _adopted[CurrentNode] = true;

        // ��� ������ ����
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // ���� ��� Ȱ��ȭ
        List<byte> nextNodes = _nextNodes[CurrentNode];
        for (byte i = 0; i < nextNodes.Count; ++i)
        {
            if (EnableCheck(nextNodes[i]))
            {
                _enabled[nextNodes[i]] = true;
                NodeBtnObjects[nextNodes[i]].SetActive(true);
            }
        }

        // ���� �޼���
        StatusText.color = Constants.WHITE;
        StatusText.text = Language.Instance["���� �Ϸ�"];

        // ���� ��ư ��� �Ұ�
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
    }


    protected override void OnFail()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Failed);

        // ���� �޼���
        StatusText.color = Constants.FAIL_TEXT;
        StatusText.text = Language.Instance["���� ����"];
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ���� ��� Ȱ��ȭ ���� ����
    /// </summary>
    private bool EnableCheck(byte nextNode)
    {
        // ���� ���� ������ ��
        TechTrees.Node.RequirmentNode[] requiredNodes = NodeData[nextNode].Requirments;
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
                    if (!Adopted[(int)requiredNodes[i].Type][NodeIndex[requiredNodes[i].NodeName]])
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
        NodeData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Facility);

        // ���� ��� ���� �迭 ����
        byte length = (byte)NodeData.Length;
        _nextNodes = new List<byte>[length];

        BasicInitialize(length);

        // ���� ��� ���
        for (byte i = 0; i < length; ++i)
        {
            // ���� ��� ����
            TechTrees.Node.RequirmentNode[] requiredNodes = NodeData[i].Requirments;

            // ���� ���� ���
            for (byte j = 0; j < (byte)requiredNodes.Length; ++j)
            {
                // �䱸 ������ �ε��� ��ȣ
                byte index = NodeIndex[requiredNodes[j].NodeName];

                switch (requiredNodes[j].Type)
                {
                    // �䱸 ������ �ü��� ��
                    case TechTreeType.Facility:
                        {
                            // �����迭 ������ �� ������ ����
                            if (null == _nextNodes[index])
                            {
                                _nextNodes[index] = new List<byte>();
                            }

                            // ���� ���� ������ �Ϳ� ���� ��带 ���� ������ ���
                            _nextNodes[index].Add(i);

                            break;
                        }
                    // �䱸 ������ �ü��� �ƴ� ��
                    default:
                        {
                            //Adopted[][]

                            break;
                        }
                }
            }
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
                NodeIcons[i].text = Constants.FACILITY_ADOPTED;
            }
            else
            {
                NodeIcons[i].text = Constants.FACILITY_UNADOPTED;
            }
        }
    }
}
