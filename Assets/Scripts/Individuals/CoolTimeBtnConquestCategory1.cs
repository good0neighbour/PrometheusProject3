public class CoolTimeBtnConquestCategory1 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 공작 방어 감소 정도
        ScreenDiplomacy.CurrentForce.Chaos += (1.0f - ScreenDiplomacy.CurrentForce.Chaos) * ScreenDiplomacy.CurrentForce.Friendly * 2.0f;

        // 혼란 최대치
        if (Constants.MAX_CHAOS < ScreenDiplomacy.CurrentForce.Chaos)
        {
            ScreenDiplomacy.CurrentForce.Chaos = Constants.MAX_CHAOS;
        }

        // 상태 표시
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

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenConquest.Instance.EmptySlot(CurrentForce, SlotNumber);
    }
}
