public class CoolTimeBtnConquestCategory1 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ���� ��� ���� ����
        ScreenDiplomacy.CurrentForce.Chaos += (1.0f - ScreenDiplomacy.CurrentForce.Chaos) * ScreenDiplomacy.CurrentForce.Friendly * 2.0f;

        // ȥ�� �ִ�ġ
        if (Constants.MAX_CHAOS < ScreenDiplomacy.CurrentForce.Chaos)
        {
            ScreenDiplomacy.CurrentForce.Chaos = Constants.MAX_CHAOS;
        }

        // ���� ǥ��
        if (0.5f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["���� ��� �ſ� ����"], Constants.WHITE);
        }
        else if (0.25f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["���� ��� ����"], Constants.WHITE);
        }
        else
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["���� ��� �ణ ����"], Constants.WHITE);
        }

        PopUpScreenConquest.Instance.FillSlot(Name, out CurrentForce, out SlotNumber);
    }

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenConquest.Instance.EmptySlot(CurrentForce, SlotNumber);
    }
}
