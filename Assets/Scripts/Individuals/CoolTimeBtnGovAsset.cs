using UnityEngine;

public class CoolTimeBtnGovAsset : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] private byte _adoptIndexNumber = 0;

    protected override void OnAdopt()
    {
        if (1.0f > PlayManager.Instance[VariableFloat.GovAsset])
        {
            PlayManager.Instance[VariableFloat.GovAsset] += (1.0f - PlayManager.Instance[VariableFloat.GovAsset]) * Constants.GOV_ASSET_MULTIPLY;
            ScreenMediaCulture.Instance.OnAdoptAnimation(_adoptIndexNumber);
        }
    }
}
