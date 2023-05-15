using System.Text;
using TMPro;
using UnityEngine;

public class CitySlot : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _populationText = null;

    private City _city = null;
    private ushort _slotNum = 0;
    private ushort _population = 0;

    public string PopulationText
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ���� ����
        ScreenCity.Instance.CurrentCity = PlayManager.Instance.GetCity(_slotNum);

        // ���� ���� ����
        ScreenCity.Instance.CurrentSlot = this;
    }


    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void SlotInitialize(ushort cityNum)
    {
        // ���� ���� ����
        _slotNum = cityNum;

        // ���� ���� ����
        _city = PlayManager.Instance.GetCity(_slotNum);

        // ���� �̸� ǥ��
        _name.text = _city.CityName;

        // �α� ǥ��
        PopulationText = Constants.INITIAL_POPULATION.ToString();
        _populationText.text = PopulationText;

        // �α��� ����
        _population = Constants.INITIAL_POPULATION;

        // �븮�� ���
        PlayManager.OMC += OnMonthChange;
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        if (gameObject.activeSelf)
        {
            ushort newPopulation = (ushort)Mathf.RoundToInt(_city.Population);

            // �α��� ��ȭ���� ���� ����
            if (_population != newPopulation)
            {
                _population = newPopulation;
                PopulationText = _population.ToString("0");
                _populationText.text = PopulationText;
            }
        }
    }


    private void OnEnable()
    {
        // ��Ȱ��ȭ�� ���ȿ��� ������Ʈ�� �� �����Ƿ�
        OnMonthChange();
    }
}