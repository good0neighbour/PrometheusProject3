using UnityEngine;

public abstract class CoolTimeBtnDiplomacySemiBase : ButtonCoolTimeBase
{
    [SerializeField] ushort _fundCost = 0;

    public abstract void OnFail();

    protected override void Cost()
    {
        PlayManager.Instance[VariableLong.Funds] -= _fundCost;
    }

    protected override bool CostAvailable()
    {
        if (_fundCost > PlayManager.Instance[VariableLong.Funds])
        {
            return false;
        }

        return true;
    }
}
