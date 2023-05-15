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


    protected override void Awake()
    {
        // ��� ���� ��������
        NodeData = PlayManager.Instance.GetTechTreeData().GetTechNodes();

        // �θ��� �Լ� ȣ��
        base.Awake();

        // ���� ��� ���� �迭 ����
        byte length = (byte)NodeData.Length;
        _nextNodes = new List<byte>[length];

        // ���� ��� ���
        for (byte i = 0; i < length; ++i)
        {
            // ���� ��� ����
            FaciityTag[] previousNodes = NodeData[i].PreviousNodes;

            // ���� ���� ���
            for (byte j = 0; j < (byte)previousNodes.Length; ++j)
            {
                // �����迭 ������ �� ������ ����
                if (null == _nextNodes[(int)previousNodes[j]])
                {
                    _nextNodes[(int)previousNodes[j]] = new List<byte>();
                }

                // ���� ���� ������ �Ϳ� ���� ��带 ���� ������ ���
                _nextNodes[(int)previousNodes[j]].Add(i);
            }
        }

        // �迭 ����
        _unlocked = PlayManager.Instance.GetUnlockedTechs();
    }



    /* ==================== Private Methods ==================== */
}
