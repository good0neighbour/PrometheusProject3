using System.Collections.Generic;
using UnityEngine;

public class PopUpScreenTech : TechTreeBase
{
    /* ==================== Variables ==================== */

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
    }


    protected override void OnFail()
    {
        
    }


    protected override bool IsUnadopted()
    {
        bool result = (0.0f >= Adopted[(int)TechTreeType.Tech][NodeIndex[NodeData[CurrentNode].NodeName]]);
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

    protected void Awake()
    {
        // ��� ���� ��������
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Tech);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Tech);

        BasicInitialize();
    }
}
