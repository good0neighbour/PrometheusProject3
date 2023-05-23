using UnityEngine;

public abstract class CoolTimeBtnMediaCultureSemiBase : ButtonCoolTimeBase
{
    /* ==================== Variables ==================== */

    [Header("자식 클래스")]
    [Header("설정")]
    [SerializeField] private byte _cultureCost = 0;



    /* ==================== Protected Methods ==================== */

    protected override void Cost()
    {
        PlayManager.Instance[VariableUint.Culture] -= _cultureCost;
    }


    protected override bool CostAvailable()
    {
        if (_cultureCost > PlayManager.Instance[VariableUint.Culture])
        {
            return false;
        }

        return true;
    }


    protected void SupportRateIncrease(VariableFloat supportRate)
    {
        PlayManager.Instance[supportRate] += Constants.SUPPORT_RATE_INCREASEMENT * PlayManager.Instance[VariableFloat.GovAffection];
        if (100.0f < PlayManager.Instance[supportRate])
        {
            PlayManager.Instance[supportRate] = 100.0f;
        }
    }



    /* ==================== Private Methods ==================== */
}
