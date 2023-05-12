using UnityEngine;
using TMPro;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;

    private bool _isCityExists = false;

    public ushort SlotNum
    {
        get;
        set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ���� ��ȣ ����
        ScreenExplore.Instance.CurrentSlot = this;

        // ���� ȭ�� Ȱ��ȭ
        ScreenExplore.Instance.OpenLandScreen(true);
    }


    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void SlotInitialize()
    {
        SlotNum = PlayManager.Instance[VariableUshort.LandNum];
        _name.text = $"{Language.Instance["����"]}{(SlotNum + 1).ToString()}";

        Language.OLC += OnLanguageChange;
    }


    /// <summary>
    /// ���� �̸� ������Ʈ
    /// </summary>
    public void SlotNameUpdate(string cityName)
    {
        _name.text = cityName;
        _isCityExists = true;
    }



    /* ==================== Private Methods ==================== */

    private void OnLanguageChange()
    {
        // ���ð� ���� ����
        if (!_isCityExists)
        {
            _name.text = $"{Language.Instance["����"]}{(SlotNum + 1).ToString()}";
        }
    }
}
