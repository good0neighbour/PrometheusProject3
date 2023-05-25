using UnityEngine;

public abstract class CoolTimeBtnDiplomacySemiBase : ButtonCoolTimeBase
{
    [SerializeField] private ushort _fundCost = 0;
    [SerializeField] protected string Name = null;
    [SerializeField] private string _description = null;

    protected Force CurrentForce = null;
    protected byte SlotNumber = 0;

    /// <summary>
    /// 설명 글 반환
    /// </summary>
    public string GetDescription()
    {
        return $"[{Language.Instance[Name]}]\n{Language.Instance[_description]}";
    }

    protected override void Cost()
    {
        if (0 < _fundCost)
        {
            PlayManager.Instance[VariableLong.Funds] -= _fundCost;
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Fund);
        }
    }

    protected override bool CostAvailable()
    {
        if (_fundCost > PlayManager.Instance[VariableLong.Funds])
        {
            return false;
        }

        return true;
    }

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenDiplomacy.Instance.EmptySlot(CurrentForce, SlotNumber);
    }
}
