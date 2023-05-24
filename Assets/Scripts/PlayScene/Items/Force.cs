using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Force
{
    private string[] _slots = new string[5];
    private bool[] _isSlotAvailable = new bool[5];
    public string ForceName { get; set; }
    public byte TradeTime { get; set; }
    public ushort JewelTrade { get; set; }
    public ushort Culture { get; set; }
    public float Friendly { get; set; }
    public float Hostile { get; set; }
    public float Conquest { get; set; }
    public float Chaos { get; set; }
    public bool Info { get; set; }

    public Force(string nationName)
    {
        ForceName = nationName;

        // 세력 활성화
        BeginForceRunning();
    }

    public void BeginForceRunning()
    {
        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
        PlayManager.OnYearChange += OnYearChange;
    }

    public string SlotText(byte index)
    {
        return _slots[index];
    }

    public void SlotText(byte index, string text)
    {
        _slots[index] = text;
    }

    public bool IsSlotAvailable(byte index)
    {
        return _isSlotAvailable[index];
    }

    public void IsSlotAvailable(byte index, bool isSlotAvailable)
    {
        _isSlotAvailable[index] = isSlotAvailable;
    }

    private void OnMonthChange()
    {
        // 우호도, 적대자 감소
        Friendly *= Constants.GENERAL_DECREASEMENT_MULTIPLY;
        Hostile *= Constants.GENERAL_DECREASEMENT_MULTIPLY;
        Conquest *= Constants.GENERAL_DECREASEMENT_MULTIPLY * Chaos;
        Chaos *= Random.Range(0.9f, 1.0f);
    }

    private void OnYearChange()
    {
        // 문화 무작위 증가
        Culture += (ushort)Random.Range(0, 3);

        // 혼란 감소
        Chaos *= Constants.GENERAL_DECREASEMENT_MULTIPLY;
    }
}
