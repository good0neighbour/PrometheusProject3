using UnityEngine;

public abstract class CoolTimeBtnDiplomacySemiBase : ButtonCoolTimeBase
{
    [SerializeField] private ushort _fundCost = 0;
    [SerializeField] protected string Name = null;
    [SerializeField] private string _description = null;

    protected byte SlotNumber = 0;

    /// <summary>
    /// ���� �� ��ȯ
    /// </summary>
    public string GetDescription()
    {
        return $"[{Name}]\n{_description}";
    }

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

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenDiplomacy.Instance.EmptySlot(SlotNumber);
    }
}
