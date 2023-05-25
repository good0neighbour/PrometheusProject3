public class CoolTimeBtnConquestCategory0 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        ScreenDiplomacy.CurrentForce.Conquest += Constants.CONQUEST_MOVEMENT * ScreenDiplomacy.CurrentForce.Friendly;
        if (0.5f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["È¥¶õ ¸Å¿ì Áõ°¡"], Constants.WHITE);
        }
        else if (0.25f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["È¥¶õ Áõ°¡"], Constants.WHITE);
        }
        else
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["È¥¶õ ¾à°£ Áõ°¡"], Constants.WHITE);
        }

        PopUpScreenConquest.Instance.FillSlot(Name, out CurrentForce, out SlotNumber);
    }
}
