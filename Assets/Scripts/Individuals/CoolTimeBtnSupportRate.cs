using UnityEngine;

public class CoolTimeBtnSupportRate : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] byte _whichOne = 0;

    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        switch (_whichOne)
        {
            case 0:
                // �ü� ������
                SupportRateIncrease(VariableFloat.FacilitySupportRate);
                PlayManager.Instance.FacilitySupportGoal = PlayManager.Instance[VariableFloat.FacilitySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);
                return;
            case 1:
                // ���� ������
                SupportRateIncrease(VariableFloat.ResearchSupportRate);
                PlayManager.Instance.ResearchSupportGoal = PlayManager.Instance[VariableFloat.ResearchSupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);
                return;
            case 2:
                // ��ȸ ������
                SupportRateIncrease(VariableFloat.SocietySupportRate);
                PlayManager.Instance.SocietySupportGoal = PlayManager.Instance[VariableFloat.SocietySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);
                return;
            case 3:
                // �ܱ� ������
                SupportRateIncrease(VariableFloat.DiplomacySupportRate);
                PlayManager.Instance.DiplomacySupportGoal = PlayManager.Instance[VariableFloat.DiplomacySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.DiplomacySupport);
                return;
            default:
                break;
        }
    }
}
