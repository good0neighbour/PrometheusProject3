using UnityEngine;

public class CoolTimeBtnSupportRate : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] byte _whichOne = 0;

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        switch (_whichOne)
        {
            case 0:
                // 시설 지지율
                SupportRateIncrease(VariableFloat.FacilitySupportRate);
                PlayManager.Instance.FacilitySupportGoal = PlayManager.Instance[VariableFloat.FacilitySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);
                return;
            case 1:
                // 연구 지지율
                SupportRateIncrease(VariableFloat.ResearchSupportRate);
                PlayManager.Instance.ResearchSupportGoal = PlayManager.Instance[VariableFloat.ResearchSupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);
                return;
            case 2:
                // 사회 지지율
                SupportRateIncrease(VariableFloat.SocietySupportRate);
                PlayManager.Instance.SocietySupportGoal = PlayManager.Instance[VariableFloat.SocietySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);
                return;
            case 3:
                // 외교 지지율
                SupportRateIncrease(VariableFloat.DiplomacySupportRate);
                PlayManager.Instance.DiplomacySupportGoal = PlayManager.Instance[VariableFloat.DiplomacySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.DiplomacySupport);
                return;
            default:
                break;
        }
    }
}
