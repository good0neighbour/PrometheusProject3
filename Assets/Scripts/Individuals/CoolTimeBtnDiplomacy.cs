using UnityEngine;

public class CoolTimeBtnDiplomacy : CoolTimeBtnDiplomacySemiBase
{
    [SerializeField] private string _name = null;

    private byte _slotNumber = 0;

    public override void OnFail()
    {

    }

    protected override void OnAdopt()
    {
        PopUpScreenDiplomacy.Instance.FillSlot(_name, out _slotNumber);
    }

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenDiplomacy.Instance.EmptySlot(_slotNumber);
    }
}
