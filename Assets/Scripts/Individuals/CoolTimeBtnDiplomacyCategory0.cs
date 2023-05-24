public class CoolTimeBtnDiplomacyCategory0 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        //소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 우호도 증가
        ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT0;

        // UI 업데이트
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out SlotNumber);
        PopUpScreenDiplomacy.Instance.SetStatusText("우호도 증가", Constants.WHITE);
    }
}
