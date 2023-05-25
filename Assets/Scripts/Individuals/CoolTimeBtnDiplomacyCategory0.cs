public class CoolTimeBtnDiplomacyCategory0 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        //�Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��ȣ�� ����
        ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT_BY_DIPLOMACY0;

        // UI ������Ʈ
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out CurrentForce, out SlotNumber);
        PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["��ȣ�� ����"], Constants.WHITE);
    }
}
