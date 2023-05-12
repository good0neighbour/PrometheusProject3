using UnityEngine;
using TMPro;
using System.Text;

public class LandSlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _resourcesField = null;

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
        // ���� ���� ����
        SlotNum = PlayManager.Instance[VariableUshort.LandNum];

        // �ؽ�Ʈ ���� ������Ʈ
        OnLanguageChange();

        // �븮�ڿ� ���
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

        // �ڿ� ��� ������Ʈ�� ���� �غ�
        StringBuilder resourceString = new StringBuilder(null);
        Land land = PlayManager.Instance.GetLand(SlotNum);

        // �� �ڿ��� 0���� Ŭ ���� ǥ���Ѵ�.
        // ��� ������ �÷��� �� �� �Ͼ�� ���� ���̱� ������ �ڿ� ���� ��������� �������� �ʰ� �׶��׶� �����ؿ´�.
        if (0 < land.Resources.Iron)
        {
            resourceString.Append($"{Language.Instance["ö"]} {land.Resources.Iron.ToString()}");
        }
        if (0 < land.Resources.Nuke)
        {
            // ������ ǥ���� �ڿ� ������ ������ ������ �ʿ� ����.
            if (string.IsNullOrEmpty(resourceString.ToString()))
            {
                resourceString.Append("\n");
            }
            resourceString.Append($"{Language.Instance["�ٹ���"]} {land.Resources.Nuke.ToString()}");
        }

        // �ڿ� ���� ǥ��
        _resourcesField.text = resourceString.ToString();
    }
}
