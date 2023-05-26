using System.Text;
using UnityEngine;
using TMPro;

public class SlotTrade : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _tradeText = null;
    [SerializeField] private TMP_Text _incomeText = null;
    [SerializeField] private TMP_Text _timeRemainText = null;

    private Trade _trade = null;
    private Force _force = null;
    private float _friendlyMultiply = 0.0f;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 슬롯 초기화
    /// </summary>
    public void SlotInitialize(ushort tradeNum)
    {
        // 거래 정보
        _trade = PlayManager.Instance.GetTrade(tradeNum);
        _force = _trade.CurrentForce;
        _friendlyMultiply = _trade.AnnualIncome * Constants.TRADE_FRIENDLY_INCREASEMENT;

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
        if (0 != _trade.Iron)
        {
            PlayManager.Instance[VariableUshort.CurrentIron] = (ushort)(PlayManager.Instance[VariableUshort.CurrentIron] - _trade.Iron * run);
            PlayManager.Instance[VariableUshort.IronUsage] = (ushort)(PlayManager.Instance[VariableUshort.IronUsage] + _trade.Iron * run);
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Iron);
            BottomBarLeft.Instance.SpendAnimation(BottomBarLeft.Displays.Iron);
        }
        if (0 != _trade.Nuke)
        {
            PlayManager.Instance[VariableUshort.CurrentNuke] = (ushort)(PlayManager.Instance[VariableUshort.CurrentNuke] - _trade.Nuke * run);
            PlayManager.Instance[VariableUshort.NukeUsage] = (ushort)(PlayManager.Instance[VariableUshort.NukeUsage] + _trade.Nuke * run);
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Nuke);
            BottomBarLeft.Instance.SpendAnimation(BottomBarLeft.Displays.Nuke);
        }
        if (0 != _trade.Jewel)
        {
            PlayManager.Instance[VariableUshort.CurrentJewel] = (ushort)(PlayManager.Instance[VariableUshort.CurrentJewel] - _trade.Jewel * run);
            PlayManager.Instance[VariableUshort.JewelUsage] = (ushort)(PlayManager.Instance[VariableUshort.JewelUsage] + _trade.Jewel * run);
            BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.Jewel);
            BottomBarLeft.Instance.SpendAnimation(BottomBarLeft.Displays.Jewel);
        }

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

        // 우호도 변화
        _force.Friendly += (1.0f - _force.Friendly - _force.Hostile) * _friendlyMultiply;

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

            // 메세지
            MessageBox.Instance.EnqueueMessage(Language.Instance[
                "{세력}(와)과의 거래가 만료됐습니다."
                ], Language.Instance[_trade.CurrentForce.ForceName]);
        }
    }


    private void OnLanguageChange()
    {
        TMP_FontAsset font = Language.Instance.GetFontAsset();

        // 세력 이름
        _name.text = Language.Instance[_trade.CurrentForce.ForceName];
        _name.font = font;

        // 연간 수익
        if (0 < _trade.AnnualIncome)
        {
            _incomeText.text = $"{Language.Instance["연간 수익"]} {_trade.AnnualIncome.ToString()}\n";
        }
        else
        {
            _incomeText.text = $"{Language.Instance["연간 지출"]} {(-_trade.AnnualIncome).ToString()}\n";
        }
        _incomeText.font = font;

        // 남은 개월 수
        _timeRemainText.text = $"{_trade.TimeRemain.ToString()}{Language.Instance["개월"]}";
        _timeRemainText.font = font;

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

        // 거래 표시. 마지막 \n은 제거한다.
        _tradeText.text = tradeText.Remove(tradeText.Length - 1, 1).ToString();
        _tradeText.font = font;
    }
}
