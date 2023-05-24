public class CoolTimeBtnDiplomacyCategory2 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // ��ȣ�� ������ 0.5���� �����Ѵ�.
        float amount = PopUpScreenDiplomacy.Instance.PlayerSoftPower - 0.5f;
        if (0.0f < amount)
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Select);

            // ��ȣ�� ����
            ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT2 * amount;
            ScreenDiplomacy.CurrentForce.Hostile *= Constants.HOSTILE_DECREASEMENT1;

            if (0.3f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("��ȣ�� �ſ� ����", Constants.WHITE);
            }
            else if (0.15f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("��ȣ�� ����", Constants.WHITE);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("��ȣ�� �ణ ����", Constants.WHITE);
            }
        }
        else
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Failed);

            if (0.4f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("������ �ſ� ����", Constants.FAIL_TEXT);
            }
            else if (0.3f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("������ ����", Constants.FAIL_TEXT);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("������ �ſ� ����", Constants.FAIL_TEXT);
            }
        }

        // UI ������Ʈ
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out SlotNumber);
    }
}
