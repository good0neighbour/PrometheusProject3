public class CoolTimeBtnConquestCategory0 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 속국화 정도
        ScreenDiplomacy.CurrentForce.Conquest += Constants.CONQUEST_MOVEMENT * ScreenDiplomacy.CurrentForce.Friendly;
        if (1.0f < ScreenDiplomacy.CurrentForce.Conquest)
        {
            ScreenDiplomacy.CurrentForce.Conquest = 1.0f;
        }

        // 상태 표시
        if (0.5f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["혼란 매우 증가"], Constants.WHITE);
        }
        else if (0.25f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["혼란 증가"], Constants.WHITE);
        }
        else
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["혼란 약간 증가"], Constants.WHITE);
        }

        PopUpScreenConquest.Instance.FillSlot(Name, out CurrentForce, out SlotNumber);
    }

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenConquest.Instance.EmptySlot(CurrentForce, SlotNumber);
    }
}
