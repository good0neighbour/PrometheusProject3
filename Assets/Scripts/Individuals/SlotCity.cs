using TMPro;
using UnityEngine;

public class SlotCity : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _populationText = null;

    private City _city = null;
    private ushort _slotNum = 0;
    private ushort _population = 0;

    public string PopulationString
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
        PopulationString = Constants.INITIAL_POPULATION.ToString();
        _populationText.text = PopulationString;

        // �α��� ����
        _population = Constants.INITIAL_POPULATION;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        ushort newPopulation = (ushort)Mathf.Round(_city.Population);

        // �α��� ��ȭ���� ���� ����
        if (_population != newPopulation)
        {
            _population = newPopulation;
            PopulationString = _population.ToString("0");
            _populationText.text = PopulationString;
        }
    }
}
