using UnityEngine;
using TMPro;

public class PopUpScreenFacility : TechTreeBase
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
        //AdoptAnimation(PlayManager.Instance[VariableFloat.FacilitySupportRate]);
        AdoptAnimation(50.0f);
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��� ������ ����
        NodeIcons[CurrentNode].text = Constants.FACILITY_ADOPTED;

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

        // ���� ��ư ��� ����
        IsAdoptAvailable = true;
    }



    /* ==================== Private Methods ==================== */
}
