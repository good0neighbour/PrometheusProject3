public class CoolTimeBtnConquestCategory1 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        ScreenDiplomacy.CurrentForce.Chaos += (1.0f - ScreenDiplomacy.CurrentForce.Chaos) * ScreenDiplomacy.CurrentForce.Friendly;
        if (0.5f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["공작 방어 매우 감소"], Constants.WHITE);
        }
        else if (0.25f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["공작 방어 감소"], Constants.WHITE);
        }
        else
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["공작 방어 약간 감소"], Constants.WHITE);
        }

        PopUpScreenConquest.Instance.FillSlot(Name, out CurrentForce, out SlotNumber);
    }
}
