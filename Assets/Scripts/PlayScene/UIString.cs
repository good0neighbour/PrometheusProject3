/// <summary>
/// UI에 어떤 정보를 표시하는데, 같은 정보를 여러 화면에 표시할 경우, 각각 따로 문자열을 생성하는 것보다 한 문자열을 참조하는 것이 더 나을 것으로 판단하여 만든 클래스. 공용 문자열을 관리한다.
/// </summary>
public class UIString
{
    /* ==================== Variables ==================== */

    private static UIString _instance = null;

    private float[] _currentValues = new float[(int)VariableFloat.EndFloat];
    private string[] _strings = new string[(int)VariableFloat.EndFloat];
    private string[] _units = new string[(int)VariableFloat.EndFloat];

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
    public string this[VariableFloat variable]
    {
        get
        {
            // 값이 바뀌었거나 문자열을 생성한 적 없을 때
            if (_currentValues[(int)variable] != PlayManager.Instance[variable] || null == _strings[(int)variable])
            {
                // 현재 값 저장
                _currentValues[(int)variable] = PlayManager.Instance[variable];

                // 표시할 단위가 있을 때
                if (null == _units[(int)variable])
                {
                    _strings[(int)variable] = $"{_currentValues[(int)variable].ToString("F2")}";
                }
                // 표시할 단위가 없을 때
                else
                {
                    _strings[(int)variable] = $"{_currentValues[(int)variable].ToString("F2")}{_units[(int)variable]}";
                }
            }

            // 반환
            return _strings[(int)variable];
        }
    }



    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private UIString()
    {
        _units[(int)VariableFloat.TotalAirPressure_hPa] = "hPa";
        _units[(int)VariableFloat.TotalAirMass_Tt] = "Tt";
        _units[(int)VariableFloat.EtcAirMass_Tt] = "Tt";
        _units[(int)VariableFloat.TotalTemperature_C] = "℃";
        _units[(int)VariableFloat.IncomeEnergy] = "E";
        _units[(int)VariableFloat.AbsorbEnergy] = "E";
        _units[(int)VariableFloat.WaterGreenHouse_C] = "℃";
        _units[(int)VariableFloat.CarbonGreenHouse_C] = "℃";
        _units[(int)VariableFloat.EtcGreenHouse_C] = "℃";
        _units[(int)VariableFloat.TotalWater_PL] = "PL";
        _units[(int)VariableFloat.WaterGas_PL] = "PL";
        _units[(int)VariableFloat.WaterLiquid_PL] = "PL";
        _units[(int)VariableFloat.WaterSolid_PL] = "PL";
        _units[(int)VariableFloat.CarbonGasMass_Tt] = "Tt";
        _units[(int)VariableFloat.CarbonLiquidMass_Tt] = "Tt";
        _units[(int)VariableFloat.CarbonSolidMass_Tt] = "Tt";
        _units[(int)VariableFloat.GravityAccelation_m_s2] = "m/s²";
        _units[(int)VariableFloat.PlanetRadius_km] = "km";
        _units[(int)VariableFloat.PlanetDensity_g_cm3] = "g/cm³";
        _units[(int)VariableFloat.PlanetMass_Tt] = "Zt";
        _units[(int)VariableFloat.PlanetDistance_AU] = "AU";
        _units[(int)VariableFloat.PlanetArea_km2] = "Mm2";
    }
}
