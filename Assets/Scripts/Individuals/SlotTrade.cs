using UnityEngine;
using TMPro;

public class SlotTrade : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _tradeText = null;

    private Trade _trade = null;



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    public void SlotInitialize(ushort tradeNum)
    {
        // �ŷ� ����
        _trade = PlayManager.Instance.GetTrade(tradeNum);

        //�ڿ� ��ȭ
        PlayManager.Instance[VariableUshort.TotalIron] = (ushort)(PlayManager.Instance[VariableUshort.TotalIron] + _trade.Iron);
        PlayManager.Instance[VariableUshort.CurrentIron] = (ushort)(PlayManager.Instance[VariableUshort.CurrentIron] + _trade.Iron);
        PlayManager.Instance[VariableUshort.TotalNuke] = (ushort)(PlayManager.Instance[VariableUshort.TotalNuke] + _trade.Nuke);
        PlayManager.Instance[VariableUshort.CurrentNuke] = (ushort)(PlayManager.Instance[VariableUshort.CurrentNuke] + _trade.Nuke);
        PlayManager.Instance[VariableUshort.TotalJewel] = (ushort)(PlayManager.Instance[VariableUshort.TotalJewel] + _trade.Jewel);
        PlayManager.Instance[VariableUshort.CurrentJewel] = (ushort)(PlayManager.Instance[VariableUshort.CurrentJewel] + _trade.Jewel);

        // ���� ����
        PlayManager.Instance[VariableInt.AnnualFund] += _trade.AnnualIncome;
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        --_trade.TimeRemain;
        if (0 >= _trade.TimeRemain)
        {
            PlayManager.Instance.RemoveTrade(_trade);
        }
    }
}
