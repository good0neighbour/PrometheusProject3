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
    /// ���� �ʱ�ȭ
    /// </summary>
    public void SlotInitialize(ushort tradeNum)
    {
        // �ŷ� ����
        _trade = PlayManager.Instance.GetTrade(tradeNum);

        // �ؽ�Ʈ ǥ��
        OnLanguageChange();

        // �븮�� ���
        PlayManager.OnMonthChange += OnMonthChange;
        Language.OnLanguageChange += OnLanguageChange;
    }


    /// <summary>
    /// 1: �ŷ� ����, -1: �ŷ� ����
    /// </summary>
    public void RunTrade(short run)
    {
        //�ڿ� ��ȭ
        PlayManager.Instance[VariableUshort.TotalIron] = (ushort)(PlayManager.Instance[VariableUshort.TotalIron] - _trade.Iron * run);
        PlayManager.Instance[VariableUshort.CurrentIron] = (ushort)(PlayManager.Instance[VariableUshort.CurrentIron] - _trade.Iron * run);
        PlayManager.Instance[VariableUshort.TotalNuke] = (ushort)(PlayManager.Instance[VariableUshort.TotalNuke] - _trade.Nuke * run);
        PlayManager.Instance[VariableUshort.CurrentNuke] = (ushort)(PlayManager.Instance[VariableUshort.CurrentNuke] - _trade.Nuke * run);
        PlayManager.Instance[VariableUshort.TotalJewel] = (ushort)(PlayManager.Instance[VariableUshort.TotalJewel] - _trade.Jewel * run);
        PlayManager.Instance[VariableUshort.CurrentJewel] = (ushort)(PlayManager.Instance[VariableUshort.CurrentJewel] + _trade.Jewel * run);

        // ���� ����
        PlayManager.Instance[VariableInt.TradeIncome] += _trade.AnnualIncome * run;
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        // �ð� ���
        --_trade.TimeRemain;

        // ���� ���� ��
        _timeRemainText.text = $"{_trade.TimeRemain.ToString()}{Language.Instance["����"]}";

        // �ð� ����
        if (0 >= _trade.TimeRemain)
        {
            // �ŷ� ����
            RunTrade(-1);

            // �븮�ڿ��� ����
            PlayManager.OnMonthChange -= OnMonthChange;
            Language.OnLanguageChange -= OnLanguageChange;

            // �����迭���� ����
            PlayManager.Instance.RemoveTrade(_trade);

            // �ŷ� �� ����
            --PlayManager.Instance[VariableUshort.TradeNum];

            // ���� ����
            Destroy(gameObject);
        }
    }


    private void OnLanguageChange()
    {
        // ���� �̸�
        _name.text = Language.Instance[_trade.ForceName];

        // �ŷ� ����
        StringBuilder tradeText = new StringBuilder();

        // ö
        if (0 < _trade.Iron)
        {
            tradeText.Append($"{Language.Instance["ö"]} {Language.Instance["����"]} {_trade.Iron.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["ö"]} {Language.Instance["����"]} {(-_trade.Iron).ToString()}\n");
        }

        // �ٹ���
        if (0 < _trade.Nuke)
        {
            tradeText.Append($"{Language.Instance["�ٹ���"]} {Language.Instance["����"]} {_trade.Nuke.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["�ٹ���"]} {Language.Instance["����"]} {(-_trade.Nuke).ToString()}\n");
        }

        // ����
        if (0 < _trade.Jewel)
        {
            tradeText.Append($"{Language.Instance["����"]} {Language.Instance["����"]} {_trade.Jewel.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["����"]} {Language.Instance["����"]} {(-_trade.Jewel).ToString()}\n");
        }

        // ���� ����
        if (0 < _trade.AnnualIncome)
        {
            tradeText.Append($"{Language.Instance["���� ����"]} {_trade.AnnualIncome.ToString()}\n");
        }
        else
        {
            tradeText.Append($"{Language.Instance["���� ����"]} {(-_trade.AnnualIncome).ToString()}\n");
        }

        // �ŷ� ǥ��. ������ \n�� �����Ѵ�.
        _tradeText.text = tradeText.Remove(tradeText.Length - 1, 1).ToString();

        // ���� ���� ��
        _timeRemainText.text = $"{_trade.TimeRemain.ToString()}{Language.Instance["����"]}";
    }
}
