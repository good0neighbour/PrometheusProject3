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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 도시 정보 전달
        ScreenCity.Instance.CurrentCity = PlayManager.Instance.GetCity(_slotNum);

        // 현재 슬롯 전달
        ScreenCity.Instance.CurrentSlot = this;
    }


    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    public void SlotInitialize(ushort cityNum)
    {
        // 도시 구분 저장
        _slotNum = cityNum;

        // 도시 정보 참조
        _city = PlayManager.Instance.GetCity(_slotNum);

        // 도시 이름 표시
        _name.text = _city.CityName;

        // 인구 표시
        PopulationString = Constants.INITIAL_POPULATION.ToString();
        _populationText.text = PopulationString;

        // 인구수 저장
        _population = Constants.INITIAL_POPULATION;
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        ushort newPopulation = (ushort)Mathf.Round(_city.Population);

        // 인구가 변화했을 때만 갱신
        if (_population != newPopulation)
        {
            _population = newPopulation;
            PopulationString = _population.ToString("0");
            _populationText.text = PopulationString;
        }
    }
}
