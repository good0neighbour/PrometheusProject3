public class CoolTimeBtnDiplomacyCategory0 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        //�Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ��ȣ�� ����
        ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT0;

        // UI ������Ʈ
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out SlotNumber);
        PopUpScreenDiplomacy.Instance.SetStatusText("��ȣ�� ����", Constants.WHITE);
    }
}
