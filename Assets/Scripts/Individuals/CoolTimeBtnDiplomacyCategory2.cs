public class CoolTimeBtnDiplomacyCategory2 : CoolTimeBtnDiplomacySemiBase
{
    protected override void OnAdopt()
    {
        // 우호도 증가는 0.5부터 시작한다.
        float amount = PopUpScreenDiplomacy.Instance.PlayerSoftPower - 0.5f;
        if (0.0f < amount)
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Select);

            // 우호도 증가
            ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT2 * amount;
            ScreenDiplomacy.CurrentForce.Hostile *= Constants.HOSTILE_DECREASEMENT1;

            if (0.3f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("우호도 매우 증가", Constants.WHITE);
            }
            else if (0.15f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("우호도 증가", Constants.WHITE);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("우호도 약간 증가", Constants.WHITE);
            }
        }
        else
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Failed);

            if (0.4f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("적대자 매우 증가", Constants.FAIL_TEXT);
            }
            else if (0.3f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("적대자 증가", Constants.FAIL_TEXT);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText("적대자 매우 증가", Constants.FAIL_TEXT);
            }
        }

        // UI 업데이트
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out SlotNumber);
    }
}
