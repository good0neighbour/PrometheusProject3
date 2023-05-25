using System.Text;
using UnityEngine;
using TMPro;

public class SlotTrade : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _tradeText = null;
    [SerializeField] private TMP_Text _timeRemainText = null;

    private Trade _trade = null;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    public void SlotInitialize(ushort tradeNum)
    {
        // 거래 정보
        _trade = PlayManager.Instance.GetTrade(tradeNum);

        // 텍스트 표시
        OnLanguageChange();

        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
        Language.OnLanguageChange += OnLanguageChange;
    }


    /// <summary>
    /// 1: 거래 개시, -1: 거래 만료
    /// </summary>
    public void RunTrade(short run)
    {
        //자원 변화
        PlayManager.Instance[VariableUshort.TotalIron] = (ushort)(PlayManager.Instance[VariableUshort.TotalIron] - _trade.Iron * run);
        PlayManager.Instance[VariableUshort.CurrentIron] = (ushort)(PlayManager.Instance[VariableUshort.CurrentIron] - _trade.Iron * run);
        PlayManager.Instance[VariableUshort.TotalNuke] = (ushort)(PlayManager.Instance[VariableUshort.TotalNuke] - _trade.Nuke * run);
        PlayManager.Instance[VariableUshort.CurrentNuke] = (ushort)(PlayManager.Instance[VariableUshort.CurrentNuke] - _trade.Nuke * run);
        PlayManager.Instance[VariableUshort.TotalJewel] = (ushort)(PlayManager.Instance[VariableUshort.TotalJewel] - _trade.Jewel * run);
        PlayManager.Instance[VariableUshort.CurrentJewel] = (ushort)(PlayManager.Instance[VariableUshort.CurrentJewel] + _trade.Jewel * run);

        // 연간 수익
        PlayManager.Instance[VariableInt.TradeIncome] += _trade.AnnualIncome * run;
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        // 시간 경과
        --_trade.TimeRemain;

        // 남은 개월 수
        _timeRemainText.text = $"{_trade.TimeRemain.ToString()}{Language.Instance["개월"]}";

        // 시간 종료
        if (0 >= _trade.TimeRemain)
        {
            // 거래 만료
            RunTrade(-1);

            // 대리자에서 제거
            PlayManager.OnMonthChange -= OnMonthChange;
            Language.OnLanguageChange -= OnLanguageChange;

            // 가변배열에서 제거
            PlayManager.Instance.RemoveTrade(_trade);

            // 거래 수 감소
            --PlayManager.Instance[VariableUshort.TradeNum];

            // 슬롯 제거
            Destroy(gameObject);
        }
    }


    private void OnLanguageChange()
    {
        // 세력 이름
        _name.text = Language.Instance[_trade.ForceName];

        // 거래 내역
        StringBuilder tradeText = new StringBuilder();

        // 철
        if (0 < _trade.Iron)
        {
            tradeText.Append($"{Language.Instance["철"]} {Language.Instance["수출"]} {_trade.Iron.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["철"]} {Language.Instance["수입"]} {(-_trade.Iron).ToString()}\n");
        }

        // 핵물질
        if (0 < _trade.Nuke)
        {
            tradeText.Append($"{Language.Instance["핵물질"]} {Language.Instance["수출"]} {_trade.Nuke.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["핵물질"]} {Language.Instance["수입"]} {(-_trade.Nuke).ToString()}\n");
        }

        // 보석
        if (0 < _trade.Jewel)
        {
            tradeText.Append($"{Language.Instance["보석"]} {Language.Instance["수출"]} {_trade.Jewel.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["보석"]} {Language.Instance["수입"]} {(-_trade.Jewel).ToString()}\n");
        }

        // 연간 수익
        if (0 < _trade.AnnualIncome)
        {
            tradeText.Append($"{Language.Instance["연간 수익"]} {_trade.AnnualIncome.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["연간 지출"]} {(-_trade.AnnualIncome).ToString()}\n");
        }

        // 거래 표시. 마지막 \n은 제거한다.
        _tradeText.text = tradeText.Remove(tradeText.Length - 1, 1).ToString();

        // 남은 개월 수
        _timeRemainText.text = $"{_trade.TimeRemain.ToString()}{Language.Instance["개월"]}";
    }
}
