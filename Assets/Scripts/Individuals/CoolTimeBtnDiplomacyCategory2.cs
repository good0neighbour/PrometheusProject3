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

            // ������ ����
            ScreenDiplomacy.CurrentForce.Hostile = (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.HOSTILE_INCREASEMENT_BY_DIPLOMACY2;

            // ��ȣ�� ����
            ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT_BY_DIPLOMACY2 * amount;

            if (0.3f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["��ȣ�� �ſ� ����"], Constants.WHITE);
            }
            else if (0.15f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["��ȣ�� ����"], Constants.WHITE);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["��ȣ�� �ణ ����"], Constants.WHITE);
            }
        }
        else
        {
            // �Ҹ� ���
            AudioManager.Instance.PlayAuido(AudioType.Failed);

            // ������ ����
            amount = 1.0f - amount;
            ScreenDiplomacy.CurrentForce.Hostile = (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.HOSTILE_INCREASEMENT_BY_DIPLOMACY2 * amount;

            if (1.4f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["������ �ſ� ����"], Constants.FAIL_TEXT);
            }
            else if (1.2f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["������ ����"], Constants.FAIL_TEXT);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["������ �ణ ����"], Constants.FAIL_TEXT);
            }
        }

        // UI ������Ʈ
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out SlotNumber);
    }
}
