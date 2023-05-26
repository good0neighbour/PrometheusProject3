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
    /// ���� �ʱ�ȭ
    /// </summary>
    public void SlotInitialize(ushort tradeNum)
    {
        // �ŷ� ����
        _trade = PlayManager.Instance.GetTrade(tradeNum);
        _force = _trade.CurrentForce;
        _friendlyMultiply = _trade.AnnualIncome * Constants.TRADE_FRIENDLY_INCREASEMENT;

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

        // ��ȣ�� ��ȭ
        _force.Friendly += (1.0f - _force.Friendly - _force.Hostile) * _friendlyMultiply;

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

            // �޼���
            MessageBox.Instance.EnqueueMessage(Language.Instance[
                "{����}(��)���� �ŷ��� ����ƽ��ϴ�."
                ], Language.Instance[_trade.CurrentForce.ForceName]);
        }
    }


    private void OnLanguageChange()
    {
        TMP_FontAsset font = Language.Instance.GetFontAsset();

        // ���� �̸�
        _name.text = Language.Instance[_trade.CurrentForce.ForceName];
        _name.font = font;

        // ���� ����
        if (0 < _trade.AnnualIncome)
        {
            _incomeText.text = $"{Language.Instance["���� ����"]} {_trade.AnnualIncome.ToString()}\n";
        }
        else
        {
            _incomeText.text = $"{Language.Instance["���� ����"]} {(-_trade.AnnualIncome).ToString()}\n";
        }
        _incomeText.font = font;

        // ���� ���� ��
        _timeRemainText.text = $"{_trade.TimeRemain.ToString()}{Language.Instance["����"]}";
        _timeRemainText.font = font;

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

        // �ŷ� ǥ��. ������ \n�� �����Ѵ�.
        _tradeText.text = tradeText.Remove(tradeText.Length - 1, 1).ToString();
        _tradeText.font = font;
    }
}
