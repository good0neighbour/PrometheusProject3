using UnityEngine;

/// <summary>
/// UI에 어떤 정보를 표시하는데, 같은 정보를 여러 화면에 표시할 경우, 각각 따로 문자열을 생성하는 것보다 한 문자열을 참조하는 것이 더 나을 것으로 판단하여 만든 클래스. 공용 문자열을 관리한다.
/// </summary>
public class UIString
{
    /* ==================== Variables ==================== */

    private static UIString _instance = null;

    private float[] _currentUshortValues = new float[(int)VariableUshort.EndUshort];
    private string[] _ushortStrings = new string[(int)VariableUshort.EndUshort];
    private float[] _currentUintValues = new float[(int)VariableUint.EndUint];
    private string[] _uintStrings = new string[(int)VariableUint.EndUint];
    private float[] _currentFloatValues = new float[(int)VariableFloat.EndFloat];
    private string[] _floatStrings = new string[(int)VariableFloat.EndFloat];
    private string[] _floatUnits = new string[(int)VariableFloat.EndFloat];
    private long[] _currentLongValues = new long[(int)VariableLong.EndLong];
    private string[] _longStrings = new string[(int)VariableLong.EndLong];
    private float[] _techValues = null;
    private string[] _techRemainString = null;
    private float[] _thoughtValues = null;
    private string[] _thoughtRemainString = null;
    private float[][] _adopted = null;
    private byte _month = 0;
    private string _date = null;

    public static UIString Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIString();
            }

            return _instance;
        }
    }

    /// <summary>
    /// 이것이 주 목적이므로 편리한 접근을 위해 만들었다. 문자열을 단위 포함해서 가져온다.
    /// </summary>
    public string this[VariableUshort variable]
    {
        get
        {
            // 값이 바뀌었거나 문자열을 생성한 적 없을 때
            if (_currentUshortValues[(int)variable] != PlayManager.Instance[variable] || null == _ushortStrings[(int)variable])
            {
                // 현재 값 저장
                _currentUshortValues[(int)variable] = PlayManager.Instance[variable];

                // 문자열 저장
                _ushortStrings[(int)variable] = _currentUshortValues[(int)variable].ToString();
            }

            // 반환
            return _ushortStrings[(int)variable];
        }
    }

    /// <summary>
    /// 이것이 주 목적이므로 편리한 접근을 위해 만들었다. 문자열을 단위 포함해서 가져온다.
    /// </summary>
    public string this[VariableUint variable]
    {
        get
        {
            // 값이 바뀌었거나 문자열을 생성한 적 없을 때
            if (_currentUintValues[(int)variable] != PlayManager.Instance[variable] || null == _uintStrings[(int)variable])
            {
                // 현재 값 저장
                _currentUintValues[(int)variable] = PlayManager.Instance[variable];

                // 문자열 저장
                _uintStrings[(int)variable] = _currentUintValues[(int)variable].ToString();
            }

            // 반환
            return _uintStrings[(int)variable];
        }
    }

    /// <summary>
    /// 이것이 주 목적이므로 편리한 접근을 위해 만들었다. 문자열을 단위 포함해서 가져온다.
    /// </summary>
    public string this[VariableFloat variable]
    {
        get
        {
            // 값이 바뀌었거나 문자열을 생성한 적 없을 때
            if (_currentFloatValues[(int)variable] != PlayManager.Instance[variable] || null == _floatStrings[(int)variable])
            {
                // 현재 값 저장
                _currentFloatValues[(int)variable] = PlayManager.Instance[variable];

                // 표시할 단위가 없을 때
                if (null == _floatUnits[(int)variable])
                {
                    _floatStrings[(int)variable] = $"{_currentFloatValues[(int)variable].ToString("F2")}";
                }
                // 표시할 단위가 있을 때
                else
                {
                    _floatStrings[(int)variable] = $"{_currentFloatValues[(int)variable].ToString("F2")}{_floatUnits[(int)variable]}";
                }
            }

            // 반환
            return _floatStrings[(int)variable];
        }
    }

    /// <summary>
    /// 이것이 주 목적이므로 편리한 접근을 위해 만들었다. 문자열을 단위 포함해서 가져온다.
    /// </summary>
    public string this[VariableLong variable]
    {
        get
        {
            // 값이 바뀌었거나 문자열을 생성한 적 없을 때
            if (_currentLongValues[(int)variable] != PlayManager.Instance[variable] || null == _longStrings[(int)variable])
            {
                // 현재 값 저장
                _currentLongValues[(int)variable] = PlayManager.Instance[variable];

                // 문자열 저장
                _longStrings[(int)variable] = _currentLongValues[(int)variable].ToString();
            }

            // 반환
            return _longStrings[(int)variable];
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 날짜 문자열 반환
    /// </summary>
    public string GetDateString()
    {
        if (_month != PlayManager.Instance[VariableByte.Month] || null == _date)
        {
            _month = PlayManager.Instance[VariableByte.Month];
            _date = $"{PlayManager.Instance[VariableUshort.Year].ToString()}{Language.Instance["년"]} {PlayManager.Instance[VariableByte.Month].ToString()}{Language.Instance["월"]}";
        }

        return _date;
    }


    public void TechInitialize(byte length)
    {
        // 배열 길이를 매개변수로 받아서 배열을 생성한다.
        _techValues = new float[length];
        _techRemainString = new string[length];

        // 배열의 모든 요소를 -1로 초기화한다.
        for (byte i = 0; i < length; ++i)
        {
            _techValues[i] = -1.0f;
        }
    }
    

    public void ThoughtInitialize(byte length)
    {
        // 배열 길이를 매개변수로 받아서 배열을 생성한다.
        _thoughtValues = new float[length];
        _thoughtRemainString = new string[length];

        // 배열의 모든 요소를 -1로 초기화한다.
        for (byte i = 0; i < length; ++i)
        {
            _thoughtValues[i] = -1.0f;
        }
    }


    /// <summary>
    /// 테크트리 노드의 남은 시간 문자열 가져온다.
    /// </summary>
    public string GetRemainString(TechTreeType type, byte index, TechTrees.Node[] nodeData)
    {
        switch (type)
        {
            case TechTreeType.Tech:
                if ((_techValues[index] != _adopted[(int)type][index]) || (null != _techRemainString[index]))
                {
                    _techValues[index] = _adopted[(int)type][index];
                    _techRemainString[index] = $"{((1.0f - _techValues[index]) / nodeData[index].ProgressionValue).ToString("0")}{Language.Instance["개월"]}";
                }
                return _techRemainString[index];
            case TechTreeType.Thought:
                if ((_thoughtValues[index] != _adopted[(int)type][index]) || (null != _thoughtRemainString[index]))
                {
                    _thoughtValues[index] = _adopted[(int)type][index];
                    _thoughtRemainString[index] = $"{((1.0f - _thoughtValues[index]) / nodeData[index].ProgressionValue).ToString("0")}{Language.Instance["개월"]}";
                }
                return _thoughtRemainString[index];
            default:
                Debug.LogError("UIString - 잘못된 테크트리");
                return null;
        }
    }



    /* ==================== Private Methods ==================== */

    private UIString()
    {
        // SI 단위
        _floatUnits[(int)VariableFloat.TotalAirPressure_hPa] = "hPa";
        _floatUnits[(int)VariableFloat.TotalAirMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.EtcAirMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.TotalTemperature_C] = "℃";
        _floatUnits[(int)VariableFloat.IncomeEnergy] = "E";
        _floatUnits[(int)VariableFloat.AbsorbEnergy] = "E";
        _floatUnits[(int)VariableFloat.WaterGreenHouse_C] = "℃";
        _floatUnits[(int)VariableFloat.CarbonGreenHouse_C] = "℃";
        _floatUnits[(int)VariableFloat.EtcGreenHouse_C] = "℃";
        _floatUnits[(int)VariableFloat.TotalWater_PL] = "PL";
        _floatUnits[(int)VariableFloat.WaterGas_PL] = "PL";
        _floatUnits[(int)VariableFloat.WaterLiquid_PL] = "PL";
        _floatUnits[(int)VariableFloat.WaterSolid_PL] = "PL";
        _floatUnits[(int)VariableFloat.TotalCarbonRatio_ppm] = "ppm";
        _floatUnits[(int)VariableFloat.CarbonGasMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.CarbonLiquidMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.CarbonSolidMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.CarbonLifeMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.PhotoLifePosibility] = "%";
        _floatUnits[(int)VariableFloat.BreathLifePosibility] = "%";
        _floatUnits[(int)VariableFloat.OxygenRatio] = "%";
        _floatUnits[(int)VariableFloat.GravityAccelation_m_s2] = "m/s²";
        _floatUnits[(int)VariableFloat.PlanetRadius_km] = "km";
        _floatUnits[(int)VariableFloat.PlanetDensity_g_cm3] = "g/cm³";
        _floatUnits[(int)VariableFloat.PlanetMass_Tt] = "Zt";
        _floatUnits[(int)VariableFloat.PlanetDistance_AU] = "AU";
        _floatUnits[(int)VariableFloat.PlanetArea_km2] = "Mm2";

        // 지지율 단위
        _floatUnits[(int)VariableFloat.FacilitySupportRate] = "%";
        _floatUnits[(int)VariableFloat.ResearchSupportRate] = "%";
        _floatUnits[(int)VariableFloat.SocietySupportRate] = "%";
        _floatUnits[(int)VariableFloat.DiplomacySupportRate] = "%";

        _adopted = PlayManager.Instance.GetAdoptedData();
    }
}
