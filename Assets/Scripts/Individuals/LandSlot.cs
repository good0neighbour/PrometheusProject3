using UnityEngine;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private ushort _slotNum = 0;



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ���� ��ȣ ����
        ScreenExplore.Instance.CurrentSlot = _slotNum;

        // ���� ȭ�� Ȱ��ȭ
        ScreenExplore.Instance.OpenLandScreen(true);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        _slotNum = PlayManager.Instance[VariableUshort.LandNum];
    }
}
