using System;
using Random = UnityEngine.Random;

[Serializable]
public class Force
{
    /* ==================== Variables ==================== */

    public string[] DiplomacySlots = new string[Constants.NUMBER_OF_SLOTS];
    public string[] ConquestSlots = new string[Constants.NUMBER_OF_SLOTS];
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


    //public string DiplomacySlotText(byte index)
    //{
    //    return DiplomacySlots[index];
    //}


    //public void DiplomacySlotText(byte index, string text)
    //{
    //    DiplomacySlots[index] = text;
    //}


    //public string ConquestSlotText(byte index)
    //{
    //    return ConquestSlots[index];
    //}


    //public void ConquestSlotText(byte index, string text)
    //{
    //    ConquestSlots[index] = text;
    //}



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        // 우호도, 적대자 감소
        Friendly *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY;
        Hostile *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY;
        if (1.0f > Conquest)
        {
            Conquest *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY * Chaos;
            Chaos *= Random.Range(0.8f, 1.0f);
        }
    }


    private void OnYearChange()
    {
        // 문화 무작위 증가
        Culture += (ushort)Random.Range(0, 3);

        // 혼란 감소
        Chaos *= Constants.GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY;
    }
}
