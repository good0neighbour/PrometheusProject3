using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScreenTrade : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text[] _amountText = null;
    [SerializeField] private TMP_Text[] _signText = null;
    [SerializeField] private GameObject[] _amountUpBtn = null;
    [SerializeField] private GameObject[] _amountDownBtn = null;
    [SerializeField] private GameObject[] _yearUpDownBtn = null;
    [SerializeField] private TMP_Text _forceName = null;
    [SerializeField] private TMP_Text _yearText = null;
    [SerializeField] private TMP_Text _tradeBtnText = null;
    [SerializeField] private TMP_Text _totalIncomeText = null;
    [SerializeField] private Image _friendlyImage = null;
    [SerializeField] private Image _hostileImage = null;
    [SerializeField] private GameObject _previousScreen = null;

    private short[] _amount = null;
    private short[,] _price = null;
    private byte _year = 0;
    private int _totalIncome = 0;
    private bool _isTradeAvailable = false;



    /* ==================== Public Methods ==================== */

    public void BtnAmountUp(int resourceIndex)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �� ���� ������Ʈ
        if (0 > _amount[resourceIndex])
        {
            _totalIncome += _price[1, resourceIndex];
        }
        else
        {
            _totalIncome += _price[0, resourceIndex];
        }
        _totalIncomeText.text = _totalIncome.ToString();

        // ���� ����
        ++_amount[resourceIndex];
        AmountTextUpdate((byte)resourceIndex);
        _amountDownBtn[resourceIndex].SetActive(true);

        // �� ��ư ��Ȱ��ȭ ����
        switch (resourceIndex)
        {
            case 0:
                if (_amount[0] >= PlayManager.Instance[VariableUshort.CurrentIron])
                {
                    _amountUpBtn[0].SetActive(false);
                }
                return;
            case 1:
                if (_amount[1] >= PlayManager.Instance[VariableUshort.CurrentNuke])
                {
                    _amountUpBtn[1].SetActive(false);
                }
                return;
            case 2:
                if (_amount[2] >= PlayManager.Instance[VariableUshort.CurrentJewel])
                {
                    _amountUpBtn[2].SetActive(false);
                }
                return;
        }

        // �ŷ� ���� Ȯ��
        SetTradeBtn();
    }


    public void BtnAmountDown(int resourceIndex)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �� ���� ������Ʈ
        if (0 < _amount[resourceIndex])
        {
            _totalIncome -= _price[0, resourceIndex];
        }
        else
        {
            _totalIncome -= _price[1, resourceIndex];
        }
        _totalIncomeText.text = _totalIncome.ToString();

        // ���� ����
        --_amount[resourceIndex];
        AmountTextUpdate((byte)resourceIndex);
        _amountUpBtn[resourceIndex].SetActive(true);

        // �Ʒ� ��ư Ȱ��ȭ ����
        if (byte.MaxValue  <= -_amount[resourceIndex])
        {
            _amountDownBtn[resourceIndex].SetActive(false);
        }

        // �ŷ� ���� Ȯ��
        SetTradeBtn();
    }


    public void BtnYearUpDown(bool isUp)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �� ����
        if (isUp)
        {
            ++_year;
            _yearUpDownBtn[1].SetActive(true);
            if (byte.MaxValue <= _year)
            {
                _yearUpDownBtn[0].SetActive(false);
            }
        }
        else
        {
            --_year;
            _yearUpDownBtn[0].SetActive(true);
            if (0 >= _year)
            {
                _yearUpDownBtn[1].SetActive(false);
            }
        }
        _yearText.text = $"{_year.ToString()}{Language.Instance["��"]}";

        // �ŷ� ���� Ȯ��
        SetTradeBtn();
    }


    public void BtnBack()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }


    public void BtnTrade()
    {
        // ��� �Ұ�
        if (!_isTradeAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // �ŷ� ����
        PlayManager.Instance.AddTrade(new Trade(ScreenDiplomacy.CurrentForce.ForcrNum, (ushort)(_year * 12), _amount[0], _amount[1], _amount[2], _totalIncome));

        // �ŷ� ��� ����
        PlayManager.Instance.AddTradeSlot(PlayManager.Instance[VariableUshort.TradeNum], true);

        // �ŷ� �� ����
        ++PlayManager.Instance[VariableUshort.TradeNum];

        // ȭ�� ��ȯ
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // �޼���
        MessageBox.Instance.EnqueueMessage(Language.Instance[
            "{����}(��)���� �ŷ��� �����մϴ�."
            ], Language.Instance[ScreenDiplomacy.CurrentForce.ForceName]);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// �ŷ� �� ǥ�� ������Ʈ
    /// </summary>
    private void AmountTextUpdate(byte index)
    {
        if (0 < _amount[index])
        {
            _amountText[index].text = (_amount[index]).ToString();
            _signText[index].text = Language.Instance["����"];
        }
        else if (0 > _amount[index])
        {
            _amountText[index].text = (-_amount[index]).ToString();
            _signText[index].text = Language.Instance["����"];
        }
        else
        {
            _amountText[index].text = "0";
            _signText[index].text = null;
        }
    }


    /// <summary>
    /// �ŷ� ���� Ȯ��
    /// </summary>
    private bool TradeAvailableCheck()
    {
        // �� ���� �� �� ���
        if (0 >= _year)
        {
            return false;
        }

        // �ŷ� ���� �� ���
        for (byte i = 0; i < _amount.Length; ++i)
        {
            if (0 != _amount[i])
            {
                return true;
            }
        }

        // �ŷ� ���� �� �� ���
        return false;
    }


    /// <summary>
    /// �ŷ� ��ư Ȱ��ȭ
    /// </summary>
    private void SetTradeBtn()
    {
        _isTradeAvailable = TradeAvailableCheck();
        if (_isTradeAvailable)
        {
            _tradeBtnText.color = Constants.WHITE;
        }
        else
        {
            _tradeBtnText.color = Constants.TEXT_BUTTON_DISABLE;
        }
    }


    private void Awake()
    {
        _amount = new short[_amountText.Length];
    }


    private void OnEnable()
    {
        // ���� �̸�
        _forceName.text = Language.Instance[ScreenDiplomacy.CurrentForce.ForceName];

        // ��ȣ�� ����
        _friendlyImage.fillAmount = ScreenDiplomacy.CurrentForce.Friendly;
        _hostileImage.fillAmount = ScreenDiplomacy.CurrentForce.Hostile;

        // ����, ���� ���� ��ȭ
        float importMultiply = 1.0f - ScreenDiplomacy.CurrentForce.Friendly + ScreenDiplomacy.CurrentForce.Hostile * Constants.TRADE_HOSTILE_AFFECTION;
        float exportMultiply = 1.0f + ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile * Constants.TRADE_HOSTILE_AFFECTION;

        // �迭 ����
        byte length = (byte)_amountText.Length;
        _price = new short[2, length];

        // ó�� ����
        for (byte i = 0; i < length; ++i)
        {
            _amountText[i].text = "0";
            _signText[i].text = null;
            _amount[i] = 0;
            _price[0, i] = (short)Mathf.Round(Constants.RESOURCES_BASE_PRICE[i] * exportMultiply);
            _price[1, i] = (short)Mathf.Round(Constants.RESOURCES_BASE_PRICE[i] * importMultiply);
            _amountDownBtn[i].SetActive(true);
        }

        // �� ��ư Ȱ��ȭ ����
        _amountUpBtn[0].SetActive(-_amount[0] < PlayManager.Instance[VariableUshort.CurrentIron]);
        _amountUpBtn[1].SetActive(-_amount[1] < PlayManager.Instance[VariableUshort.CurrentNuke]);
        _amountUpBtn[2].SetActive(-_amount[2] < PlayManager.Instance[VariableUshort.CurrentJewel]);

        // �� �ʱ�ȭ
        _year = 0;
        _yearText.text = $"0{Language.Instance["��"]}";
        _yearUpDownBtn[0].SetActive(true);
        _yearUpDownBtn[1].SetActive(false);

        // �� ���� �ʱ�ȭ
        _totalIncome = 0;
        _totalIncomeText.text = "0";

        // ó�� ����
        _isTradeAvailable = false;
        _tradeBtnText.color = Constants.TEXT_BUTTON_DISABLE;
    }


    private void Update()
    {
        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BtnBack();
        }
    }
}
