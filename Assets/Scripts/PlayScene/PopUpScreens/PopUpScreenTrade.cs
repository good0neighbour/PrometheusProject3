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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 총 수익 업데이트
        if (0 > _amount[resourceIndex])
        {
            _totalIncome += _price[1, resourceIndex];
        }
        else
        {
            _totalIncome += _price[0, resourceIndex];
        }
        _totalIncomeText.text = _totalIncome.ToString();

        // 수출 증가
        ++_amount[resourceIndex];
        AmountTextUpdate((byte)resourceIndex);
        _amountDownBtn[resourceIndex].SetActive(true);

        // 위 버튼 비활성화 여부
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

        // 거래 가능 확인
        SetTradeBtn();
    }


    public void BtnAmountDown(int resourceIndex)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 총 수익 업데이트
        if (0 < _amount[resourceIndex])
        {
            _totalIncome -= _price[0, resourceIndex];
        }
        else
        {
            _totalIncome -= _price[1, resourceIndex];
        }
        _totalIncomeText.text = _totalIncome.ToString();

        // 수입 증가
        --_amount[resourceIndex];
        AmountTextUpdate((byte)resourceIndex);
        _amountUpBtn[resourceIndex].SetActive(true);

        // 아래 버튼 활성화 여부
        if (byte.MaxValue  <= -_amount[resourceIndex])
        {
            _amountDownBtn[resourceIndex].SetActive(false);
        }

        // 거래 가능 확인
        SetTradeBtn();
    }


    public void BtnYearUpDown(bool isUp)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 년 설정
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
        _yearText.text = $"{_year.ToString()}{Language.Instance["년"]}";

        // 거래 가능 확인
        SetTradeBtn();
    }


    public void BtnBack()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 화면 전환
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }


    public void BtnTrade()
    {
        // 사용 불가
        if (!_isTradeAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // 거래 생성
        PlayManager.Instance.AddTrade(new Trade(ScreenDiplomacy.CurrentForce.ForcrNum, (ushort)(_year * 12), _amount[0], _amount[1], _amount[2], _totalIncome));

        // 거래 목록 생성
        PlayManager.Instance.AddTradeSlot(PlayManager.Instance[VariableUshort.TradeNum], true);

        // 거래 수 증가
        ++PlayManager.Instance[VariableUshort.TradeNum];

        // 화면 전환
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // 메세지
        MessageBox.Instance.EnqueueMessage(Language.Instance[
            "{세력}(와)과의 거래를 개시합니다."
            ], Language.Instance[ScreenDiplomacy.CurrentForce.ForceName]);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 거래 량 표시 업데이트
    /// </summary>
    private void AmountTextUpdate(byte index)
    {
        if (0 < _amount[index])
        {
            _amountText[index].text = (_amount[index]).ToString();
            _signText[index].text = Language.Instance["수출"];
        }
        else if (0 > _amount[index])
        {
            _amountText[index].text = (-_amount[index]).ToString();
            _signText[index].text = Language.Instance["수입"];
        }
        else
        {
            _amountText[index].text = "0";
            _signText[index].text = null;
        }
    }


    /// <summary>
    /// 거래 가능 확인
    /// </summary>
    private bool TradeAvailableCheck()
    {
        // 년 설정 안 된 경우
        if (0 >= _year)
        {
            return false;
        }

        // 거래 설정 된 경우
        for (byte i = 0; i < _amount.Length; ++i)
        {
            if (0 != _amount[i])
            {
                return true;
            }
        }

        // 거래 설정 안 된 경우
        return false;
    }


    /// <summary>
    /// 거래 버튼 활성화
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
        // 세력 이름
        _forceName.text = Language.Instance[ScreenDiplomacy.CurrentForce.ForceName];

        // 우호도 정보
        _friendlyImage.fillAmount = ScreenDiplomacy.CurrentForce.Friendly;
        _hostileImage.fillAmount = ScreenDiplomacy.CurrentForce.Hostile;

        // 수입, 수출 가격 변화
        float importMultiply = 1.0f - ScreenDiplomacy.CurrentForce.Friendly + ScreenDiplomacy.CurrentForce.Hostile * Constants.TRADE_HOSTILE_AFFECTION;
        float exportMultiply = 1.0f + ScreenDiplomacy.CurrentForce.Friendly - ScreenDiplomacy.CurrentForce.Hostile * Constants.TRADE_HOSTILE_AFFECTION;

        // 배열 생성
        byte length = (byte)_amountText.Length;
        _price = new short[2, length];

        // 처음 상태
        for (byte i = 0; i < length; ++i)
        {
            _amountText[i].text = "0";
            _signText[i].text = null;
            _amount[i] = 0;
            _price[0, i] = (short)Mathf.Round(Constants.RESOURCES_BASE_PRICE[i] * exportMultiply);
            _price[1, i] = (short)Mathf.Round(Constants.RESOURCES_BASE_PRICE[i] * importMultiply);
            _amountDownBtn[i].SetActive(true);
        }

        // 위 버튼 활성화 여부
        _amountUpBtn[0].SetActive(-_amount[0] < PlayManager.Instance[VariableUshort.CurrentIron]);
        _amountUpBtn[1].SetActive(-_amount[1] < PlayManager.Instance[VariableUshort.CurrentNuke]);
        _amountUpBtn[2].SetActive(-_amount[2] < PlayManager.Instance[VariableUshort.CurrentJewel]);

        // 년 초기화
        _year = 0;
        _yearText.text = $"0{Language.Instance["년"]}";
        _yearUpDownBtn[0].SetActive(true);
        _yearUpDownBtn[1].SetActive(false);

        // 총 수익 초기화
        _totalIncome = 0;
        _totalIncomeText.text = "0";

        // 처음 상태
        _isTradeAvailable = false;
        _tradeBtnText.color = Constants.TEXT_BUTTON_DISABLE;
    }


    private void Update()
    {
        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BtnBack();
        }
    }
}
