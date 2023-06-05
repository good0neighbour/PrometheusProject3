using System;
using Random = UnityEngine.Random;

[Serializable]
public class Force
{
    private enum MessageType
    {
        Friendly,
        Hostile,
        Conquest,
        ConquestComplete,
        EndType
    }



    /* ==================== Variables ==================== */

    public string[] DiplomacySlots = new string[Constants.NUMBER_OF_SLOTS];
    public string[] ConquestSlots = new string[Constants.NUMBER_OF_SLOTS];
    public bool[] Messages = new bool[(int)MessageType.EndType];
    public string ForceName = null;
    public byte ForcrNum = 0;
    public byte DiplomacySlotUsage = 0;
    public byte ConquestSlotUsage = 0;
    public ushort Culture = 0;
    public float Friendly = 0.0f;
    public float Hostile = 0.0f;
    public float Conquest = 0.0f;
    public float Defence = 0.0f;
    public bool Info = false;



    /* ==================== Public Methods ==================== */

    public Force(string nationName, byte forcrNum)
    {
        ForceName = nationName;
        ForcrNum = forcrNum;

        for (byte i = 0; i < Messages.Length; ++i)
        {
            Messages[i] = true;
        }

        // 세력 활성화
        BeginForceRunning();
        ForcrNum = forcrNum;
    }


    /// <summary>
    /// 세력 동작 가동
    /// </summary>
    public void BeginForceRunning()
    {
        // 외교 슬롯은 다 비운다.
        for (byte i = 0; i < Constants.NUMBER_OF_SLOTS; ++i)
        {
            DiplomacySlots[i] = null;
            ConquestSlots[i] = null;
        }
        DiplomacySlotUsage = 0;
        ConquestSlotUsage = 0;

        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
        PlayManager.OnYearChange += OnYearChange;
    }


    /// <summary>
    /// 공공 외교 사용 가능 여부
    /// </summary>
    public bool IsDiplomacySlotAvailable()
    {
        return Constants.NUMBER_OF_SLOTS != DiplomacySlotUsage;
    }


    /// <summary>
    /// 공작 활동 사용 가능 여부
    /// </summary>
    public bool IsConquestSlotAvailable()
    {
        return Constants.NUMBER_OF_SLOTS != ConquestSlotUsage;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 메세지 생성
    /// </summary>
    private void MessageEnqueue()
    {
        #region 우호도
        if (Messages[(int)MessageType.Friendly])
        {
            if (0.5f < Friendly)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "다수의 {세력} 시민들이 플레이어 국가에 우호적입니다. 해당 국가의 시민들은 플레이어의 행동을 쉽게 받아들일 것입니다."
                    ], ForceName);
                Messages[(int)MessageType.Friendly] = false;
            }
        }
        else
        {
            if (0.2f > Friendly)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "플레이어 국가에 대한 {세력} 시민들의 우호도가 낮아지고 있습니다."
                    ], ForceName);
                Messages[(int)MessageType.Friendly] = true;
            }
        }
        #endregion

        #region 적대자
        if (Messages[(int)MessageType.Hostile])
        {
            if (0.5f < Hostile)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "다수의 {세력} 시민들이 플레이어 국가에 적대적입니다. 해당 국가와의 교역에서 거래 가격이 플레이어에게 불리해집니다."
                    ], ForceName);
                Messages[(int)MessageType.Hostile] = false;
            }
        }
        else
        {
            if (0.2f > Hostile)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "플레이어 국가에 대한 {세력} 시민들의 적대감이 낮아지고 있습니다."
                    ], ForceName);
                Messages[(int)MessageType.Hostile] = true;
            }
        }
        #endregion

        #region 속국화
        // 속국화 진행
        if (Messages[(int)MessageType.Conquest])
        {
            if (0.5f < Conquest)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{세력}(이)가 혼란 상태입니다. 끊임없는 내부갈등과 무능한 정부의 모습에 일부 시민들은 새로운 지배자를 원합니다."
                    ], ForceName);
                Messages[(int)MessageType.Conquest] = false;
            }
        }
        else
        {
            if (0.4f > Conquest)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{세력}(이)가 혼란을 극복하고 있습니다."
                    ], ForceName);
                Messages[(int)MessageType.Conquest] = true;
            }
        }

        // 속국화 완료
        if (Messages[(int)MessageType.ConquestComplete])
        {
            if (1.0f <= Conquest)
            {
                ++PlayManager.Instance[VariableByte.Conquested];
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{세력}(이)가 플레이어 국가의 속국이 되었습니다. 많은 시민들은 절망하였지만 플레이어에게 우호적인 세력은 기꺼이 당신에게 나라는 바칩니다."
                    ], ForceName);
                Messages[(int)MessageType.ConquestComplete] = false;
            }
        }
        #endregion
    }


    private void OnMonthChange()
    {
        // 우호도, 적대자 감소
        Friendly -= Constants.GENERAL_DIPLOMACY_DECREASEMENT;
        Hostile -= Constants.GENERAL_DIPLOMACY_DECREASEMENT;
        if (0.0f > Friendly)
        {
            Friendly = 0.0f;
        }
        if (0.0f > Hostile)
        {
            Hostile = 0.0f;
        }

        // 속국화 방어
        if (1.0f > Conquest)
        {
            Conquest -= Defence;
            if (0.0f > Conquest)
            {
                Conquest = 0.0f;
            }
        }

        // 메세지 생성
        MessageEnqueue();
    }


    private void OnYearChange()
    {
        // 문화 무작위 증가
        Culture += (ushort)Random.Range(0, 3);

        // 공작 방어력 증가
        Defence += Constants.GENERAL_DIPLOMACY_DECREASEMENT * 0.1f;
    }
}
