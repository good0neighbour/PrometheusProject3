using UnityEngine;

public class CoolTimeBtnGovAffection : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] private byte _adoptIndexNumber = 0;

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        if (PlayManager.Instance[VariableFloat.GovAsset] > PlayManager.Instance[VariableFloat.GovAffection])
        {
            PlayManager.Instance[VariableFloat.GovAffection] += (PlayManager.Instance[VariableFloat.GovAsset] - PlayManager.Instance[VariableFloat.GovAffection]) * Constants.GOV_AFFECTION_MULTIPLY;
            ScreenMediaCulture.Instance.OnAdoptAnimation(_adoptIndexNumber);
        }
    }
}
