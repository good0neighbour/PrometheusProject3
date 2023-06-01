public class CoolTimeBtnConquestCategory0 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // �ӱ�ȭ ����
        ScreenDiplomacy.CurrentForce.Conquest += Constants.CONQUEST_MOVEMENT * ScreenDiplomacy.CurrentForce.Friendly;
        if (1.0f < ScreenDiplomacy.CurrentForce.Conquest)
        {
            ScreenDiplomacy.CurrentForce.Conquest = 1.0f;
        }

        // ���� ǥ��
        if (0.5f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["ȥ�� �ſ� ����"], Constants.WHITE);
        }
        else if (0.25f < ScreenDiplomacy.CurrentForce.Friendly)
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["ȥ�� ����"], Constants.WHITE);
        }
        else
        {
            PopUpScreenConquest.Instance.SetStatusText(Language.Instance["ȥ�� �ణ ����"], Constants.WHITE);
        }

        PopUpScreenConquest.Instance.FillSlot(Name, out CurrentForce, out SlotNumber);
    }

    protected override void OnCoolTimeEnd()
    {
        base.OnCoolTimeEnd();
        PopUpScreenConquest.Instance.EmptySlot(CurrentForce, SlotNumber);
    }
}
