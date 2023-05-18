using UnityEngine;
using TMPro;
using System.Text;

public class SlotLand : MonoBehaviour
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
    public void SlotInitialize(ushort landNum)
    {
        // ���� ���� ����
        SlotNum = landNum;

        // ���� �Ǽ� ���� Ȯ��. ����� ������ �ҷ��� ��� �ʿ�.
        _isCityExists = (null != PlayManager.Instance.GetLand(SlotNum).CityName);

        // �ؽ�Ʈ ���� ������Ʈ
        OnLanguageChange();

        // �븮�ڿ� ���
        Language.OnLanguageChange += OnLanguageChange;
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
        // ���� ������ ���� �̸����� ������Ʈ
        else
        {
            _name.text = PlayManager.Instance.GetLand(SlotNum).CityName;
        }

        // �ڿ� ��� ������Ʈ�� ���� �غ�
        StringBuilder resourceString = new StringBuilder(null);
        byte[] land = PlayManager.Instance.GetLand(SlotNum).Resources;

        // ��� ������ �÷��� �� �� �Ͼ�� ���� ���̱� ������ �ڿ� ���� ��������� �������� �ʰ� �׶��׶� �����ؿ´�.
        for (ResourceType i = 0; i < ResourceType.End; ++i)
        {
            // ���ڿ� ���� ��������
            string text;
            switch (i)
            {
                case ResourceType.Iron:
                    text = Language.Instance["ö"];
                    break;
                case ResourceType.Nuke:
                    text = Language.Instance["�ٹ���"];
                    break;
                case ResourceType.Jewel:
                    text = Language.Instance["����"];
                    break;
                default:
                    Debug.LogError($"LandSlot - ResourceType ���� ���. OnLanguageChange()");
                    return;
            }

            // �� �ڿ��� 0���� Ŭ ���� ǥ���Ѵ�.
            if (0 < land[(int)i])
            {
                // ������ ǥ���� �ڿ� ������ ������ ������ �ʿ� ����.
                if (!string.IsNullOrEmpty(resourceString.ToString()))
                {
                    resourceString.Append("\n");
                }
                resourceString.Append($"{text} {land[(int)i].ToString()}");
            }
        }

        // �ڿ� ���� ǥ��
        _resourcesField.text = resourceString.ToString();
    }
}
