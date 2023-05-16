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
        PopulationText = Constants.INITIAL_POPULATION.ToString();
        _populationText.text = PopulationText;

        // 인구수 저장
        _population = Constants.INITIAL_POPULATION;

        // 대리자 등록
        PlayManager.OnMonthCahnge += OnMonthChange;
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        if (gameObject.activeSelf)
        {
            ushort newPopulation = (ushort)Mathf.RoundToInt(_city.Population);

            // 인구가 변화했을 때만 갱신
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
        // 비활성화된 동안에는 업데이트를 안 했으므로
        OnMonthChange();
    }
}
