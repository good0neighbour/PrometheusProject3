public class CoolTimeBtnConquestCategory1 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // ���� ��� ���� ����
        ScreenDiplomacy.CurrentForce.Defence -= Constants.GENERAL_DIPLOMACY_DECREASEMENT * ScreenDiplomacy.CurrentForce.Friendly * 2.0f;

        // ���� ���� �ּ�ġ
        if (Constants.MIN_DEFENCE > ScreenDiplomacy.CurrentForce.Defence)
        {
            ScreenDiplomacy.CurrentForce.Defence = Constants.MIN_DEFENCE;
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
