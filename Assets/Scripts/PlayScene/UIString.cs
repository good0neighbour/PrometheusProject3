/// <summary>
/// UI�� � ������ ǥ���ϴµ�, ���� ������ ���� ȭ�鿡 ǥ���� ���, ���� ���� ���ڿ��� �����ϴ� �ͺ��� �� ���ڿ��� �����ϴ� ���� �� ���� ������ �Ǵ��Ͽ� ���� Ŭ����. ���� ���ڿ��� �����Ѵ�.
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
    /// �̰��� �� �����̹Ƿ� ���� ������ ���� �������. ���ڿ��� ���� �����ؼ� �����´�.
    /// </summary>
    public string this[VariableFloat variable]
    {
        get
        {
            // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
            if (_currentValues[(int)variable] != PlayManager.Instance[variable] || null == _strings[(int)variable])
            {
                // ���� �� ����
                _currentValues[(int)variable] = PlayManager.Instance[variable];

                // ǥ���� ������ ���� ��
                if (null == _units[(int)variable])
                {
                    _strings[(int)variable] = $"{_currentValues[(int)variable].ToString("F2")}";
                }
                // ǥ���� ������ ���� ��
                else
                {
                    _strings[(int)variable] = $"{_currentValues[(int)variable].ToString("F2")}{_units[(int)variable]}";
                }
            }

            // ��ȯ
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
        _units[(int)VariableFloat.TotalTemperature_C] = "��";
        _units[(int)VariableFloat.IncomeEnergy] = "E";
        _units[(int)VariableFloat.AbsorbEnergy] = "E";
        _units[(int)VariableFloat.WaterGreenHouse_C] = "��";
        _units[(int)VariableFloat.CarbonGreenHouse_C] = "��";
        _units[(int)VariableFloat.EtcGreenHouse_C] = "��";
        _units[(int)VariableFloat.TotalWater_PL] = "PL";
        _units[(int)VariableFloat.WaterGas_PL] = "PL";
        _units[(int)VariableFloat.WaterLiquid_PL] = "PL";
        _units[(int)VariableFloat.WaterSolid_PL] = "PL";
        _units[(int)VariableFloat.CarbonGasMass_Tt] = "Tt";
        _units[(int)VariableFloat.CarbonLiquidMass_Tt] = "Tt";
        _units[(int)VariableFloat.CarbonSolidMass_Tt] = "Tt";
        _units[(int)VariableFloat.GravityAccelation_m_s2] = "m/s��";
        _units[(int)VariableFloat.PlanetRadius_km] = "km";
        _units[(int)VariableFloat.PlanetDensity_g_cm3] = "g/cm��";
        _units[(int)VariableFloat.PlanetMass_Tt] = "Zt";
        _units[(int)VariableFloat.PlanetDistance_AU] = "AU";
        _units[(int)VariableFloat.PlanetArea_km2] = "Mm2";
    }
}
