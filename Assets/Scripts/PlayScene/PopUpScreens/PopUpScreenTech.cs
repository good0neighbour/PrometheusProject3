using System.Collections.Generic;
using UnityEngine;

public class PopUpScreenTech : TechTreeBase
{
    /* ==================== Variables ==================== */

    private List<byte>[] _nextNodes = null;
    private bool[] _unlocked = null;



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
        //AdoptAnimation(PlayManager.Instance[VariableFloat.ResearchSupportRate]);
        AdoptAnimation(75.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // Ȱ��ȭ ���� ����

        // ��� ������ ����
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

        // ���� ��� Ȱ��ȭ
        _unlocked[CurrentNode] = true;
        

        // ���� �޼���
        StatusText.color = Constants.WHITE;
        StatusText.text = Language.Instance["���� �Ϸ�"];

        // ���� ��ư ��� �Ұ�
        AdoptBtn.color = Constants.TEXT_BUTTON_DISABLE;
    }

    protected override void OnFail()
    {
        throw new System.NotImplementedException();
    }



    /* ==================== Private Methods ==================== */

    protected void Awake()
    {
        // ��� ���� ��������
        NodeData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Tech);

        // ���� ��� ���� �迭 ����
        byte length = (byte)NodeData.Length;
        _nextNodes = new List<byte>[length];

        // �θ��� �Լ� ȣ��
        BasicInitialize(length);

        // ���� ��� ���
        for (byte i = 0; i < length; ++i)
        {
            // ���� ��� ����
            TechTrees.Node.RequirmentNode[] requiredNodes = NodeData[i].Requirments;

            // ���� ���� ���
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // �䱸 ������ ����� ���� ������ ��
                switch (requiredNodes[j].Type)
                {
                    case TechTreeType.Tech:
                        byte index = NodeIndex[requiredNodes[j].NodeName];

                        // �����迭 ������ �� ������ ����
                        if (null == _nextNodes[index])
                        {
                            _nextNodes[index] = new List<byte>();
                        }

                        // ���� ���� ������ �Ϳ� ���� ��带 ���� ������ ���
                        _nextNodes[index].Add(i);

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
