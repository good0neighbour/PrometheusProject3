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

            // 적대자 증가
            ScreenDiplomacy.CurrentForce.Hostile = (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.HOSTILE_INCREASEMENT_BY_DIPLOMACY2;

            // 우호도 증가
            ScreenDiplomacy.CurrentForce.Friendly += (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.FRIENDLY_INCREASEMENT_BY_DIPLOMACY2 * amount;

            if (0.3f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["우호도 매우 증가"], Constants.WHITE);
            }
            else if (0.15f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["우호도 증가"], Constants.WHITE);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["우호도 약간 증가"], Constants.WHITE);
            }
        }
        else
        {
            // 소리 재생
            AudioManager.Instance.PlayAuido(AudioType.Failed);

            // 적대자 증가
            amount = 1.0f - amount;
            ScreenDiplomacy.CurrentForce.Hostile = (1.0f - ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile) * Constants.HOSTILE_INCREASEMENT_BY_DIPLOMACY2 * amount;

            if (1.4f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["적대자 매우 증가"], Constants.FAIL_TEXT);
            }
            else if (1.2f < amount)
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["적대자 증가"], Constants.FAIL_TEXT);
            }
            else
            {
                PopUpScreenDiplomacy.Instance.SetStatusText(Language.Instance["적대자 약간 증가"], Constants.FAIL_TEXT);
            }
        }

        // UI 업데이트
        PopUpScreenDiplomacy.Instance.FillSlot(Name, out SlotNumber);
    }
}
