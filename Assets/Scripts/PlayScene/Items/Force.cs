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
        EndType
    }



    /* ==================== Variables ==================== */

    public string[] DiplomacySlots = new string[Constants.NUMBER_OF_SLOTS];
    public string[] ConquestSlots = new string[Constants.NUMBER_OF_SLOTS];
    public bool[] Messages = new bool[(int)MessageType.EndType];
    public string ForceName = null;
    public byte ForcrNum = 0;
    public ushort Culture = 0;
    public float Friendly = 0.0f;
    public float Hostile = 0.0f;
    public float Conquest = 0.0f;
    public float Chaos = 0.0f;
    public bool Info = false;
    public bool IsDiplomacySlotAvailable = true;
    public bool IsConquestSlotAvailable = true;



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


    public void BeginForceRunning()
    {
        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
        PlayManager.OnYearChange += OnYearChange;
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
            if (0.2f < Friendly)
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
            if (0.2f < Hostile)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "플레이어 국가에 대한 {세력} 시민들의 적대감이 낮아지고 있습니다."
                    ], ForceName);
                Messages[(int)MessageType.Hostile] = true;
            }
        }
        #endregion

        #region 속국화
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
            if (0.3f < Conquest)
            {
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{세력}(이)가 혼란을 극복하고 있습니다."
                    ], ForceName);
                Messages[(int)MessageType.Conquest] = true;
            }
        }
        #endregion
    }


    private void OnMonthChange()
    {
        // 우호도, 적대자 감소
        Friendly *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY;
        Hostile *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY;
        if (1.0f > Conquest)
        {
            Conquest *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY * Chaos;
            Chaos *= Random.Range(0.8f, 1.0f);
            if (1.0f <= Conquest)
            {
                ++PlayManager.Instance[VariableByte.Conquested];
                MessageBox.Instance.EnqueueMessage(Language.Instance[
                    "{세력}(이)가 플레이어 국가의 속국이 되었습니다. 많은 시민들은 절망하였지만 플레이어에게 우호적인 세력은 기꺼이 당신에게 나라는 바칩니다."
                    ], ForceName);
            }
        }

        // 메세지 생성
        MessageEnqueue();
    }


    private void OnYearChange()
    {
        // 문화 무작위 증가
        Culture += (ushort)Random.Range(0, 3);

        // 혼란 감소
        Chaos *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY;
    }
}
