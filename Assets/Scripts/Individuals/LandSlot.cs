using UnityEngine;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ȭ�� Ȱ��ȭ
        ScreenExplore.Instance.OpenLandScreen(true);
    }



    /* ==================== Private Methods ==================== */
}
