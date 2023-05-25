using System;
using Random = UnityEngine.Random;

[Serializable]
public class Force
{
    /* ==================== Variables ==================== */

    private string[] _diplomacySlots = new string[Constants.NUMBER_OF_SLOTS];
    private string[] _conquestSlots = new string[Constants.NUMBER_OF_SLOTS];
    public string ForceName { get; set; }
    public ushort Culture { get; set; }
    public float Friendly { get; set; }
    public float Hostile { get; set; }
    public float Conquest { get; set; }
    public float Chaos { get; set; }
    public bool Info { get; set; }
    public bool IsDiplomacySlotAvailable { get; set; }
    public bool IsConquestSlotAvailable { get; set; }



    /* ==================== Public Methods ==================== */

    public Force(string nationName)
    {
        ForceName = nationName;
        IsDiplomacySlotAvailable = true;

        // 세력 활성화
        BeginForceRunning();
    }


    public void BeginForceRunning()
    {
        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
        PlayManager.OnYearChange += OnYearChange;
    }


    public string DiplomacySlotText(byte index)
    {
        return _diplomacySlots[index];
    }


    public void DiplomacySlotText(byte index, string text)
    {
        _diplomacySlots[index] = text;
    }


    public string ConquestSlotText(byte index)
    {
        return _conquestSlots[index];
    }


    public void ConquestSlotText(byte index, string text)
    {
        _conquestSlots[index] = text;
    }



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
