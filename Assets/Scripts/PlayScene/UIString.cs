using UnityEngine;

/// <summary>
/// UI�� � ������ ǥ���ϴµ�, ���� ������ ���� ȭ�鿡 ǥ���� ���, ���� ���� ���ڿ��� �����ϴ� �ͺ��� �� ���ڿ��� �����ϴ� ���� �� ���� ������ �Ǵ��Ͽ� ���� Ŭ����. ���� ���ڿ��� �����Ѵ�.
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
    /// �̰��� �� �����̹Ƿ� ���� ������ ���� �������. ���ڿ��� ���� �����ؼ� �����´�.
    /// </summary>
    public string this[VariableUshort variable]
    {
        get
        {
            // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
            if (_currentUshortValues[(int)variable] != PlayManager.Instance[variable] || null == _ushortStrings[(int)variable])
            {
                // ���� �� ����
                _currentUshortValues[(int)variable] = PlayManager.Instance[variable];

                // ���ڿ� ����
                _ushortStrings[(int)variable] = _currentUshortValues[(int)variable].ToString();
            }

            // ��ȯ
            return _ushortStrings[(int)variable];
        }
    }

    /// <summary>
    /// �̰��� �� �����̹Ƿ� ���� ������ ���� �������. ���ڿ��� ���� �����ؼ� �����´�.
    /// </summary>
    public string this[VariableUint variable]
    {
        get
        {
            // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
            if (_currentUintValues[(int)variable] != PlayManager.Instance[variable] || null == _uintStrings[(int)variable])
            {
                // ���� �� ����
                _currentUintValues[(int)variable] = PlayManager.Instance[variable];

                // ���ڿ� ����
                _uintStrings[(int)variable] = _currentUintValues[(int)variable].ToString();
            }

            // ��ȯ
            return _uintStrings[(int)variable];
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
            if (_currentFloatValues[(int)variable] != PlayManager.Instance[variable] || null == _floatStrings[(int)variable])
            {
                // ���� �� ����
                _currentFloatValues[(int)variable] = PlayManager.Instance[variable];

                // ǥ���� ������ ���� ��
                if (null == _floatUnits[(int)variable])
                {
                    _floatStrings[(int)variable] = $"{_currentFloatValues[(int)variable].ToString("F2")}";
                }
                // ǥ���� ������ ���� ��
                else
                {
                    _floatStrings[(int)variable] = $"{_currentFloatValues[(int)variable].ToString("F2")}{_floatUnits[(int)variable]}";
                }
            }

            // ��ȯ
            return _floatStrings[(int)variable];
        }
    }

    /// <summary>
    /// �̰��� �� �����̹Ƿ� ���� ������ ���� �������. ���ڿ��� ���� �����ؼ� �����´�.
    /// </summary>
    public string this[VariableLong variable]
    {
        get
        {
            // ���� �ٲ���ų� ���ڿ��� ������ �� ���� ��
            if (_currentLongValues[(int)variable] != PlayManager.Instance[variable] || null == _longStrings[(int)variable])
            {
                // ���� �� ����
                _currentLongValues[(int)variable] = PlayManager.Instance[variable];

                // ���ڿ� ����
                _longStrings[(int)variable] = _currentLongValues[(int)variable].ToString();
            }

            // ��ȯ
            return _longStrings[(int)variable];
        }
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ��¥ ���ڿ� ��ȯ
    /// </summary>
    public string GetDateString()
    {
        if (_month != PlayManager.Instance[VariableByte.Month] || null == _date)
        {
            _month = PlayManager.Instance[VariableByte.Month];
            _date = $"{PlayManager.Instance[VariableUshort.Year].ToString()}{Language.Instance["��"]} {PlayManager.Instance[VariableByte.Month].ToString()}{Language.Instance["��"]}";
        }

        return _date;
    }


    public void TechInitialize(byte length)
    {
        // �迭 ���̸� �Ű������� �޾Ƽ� �迭�� �����Ѵ�.
        _techValues = new float[length];
        _techRemainString = new string[length];

        // �迭�� ��� ��Ҹ� -1�� �ʱ�ȭ�Ѵ�.
        for (byte i = 0; i < length; ++i)
        {
            _techValues[i] = -1.0f;
        }
    }
    

    public void ThoughtInitialize(byte length)
    {
        // �迭 ���̸� �Ű������� �޾Ƽ� �迭�� �����Ѵ�.
        _thoughtValues = new float[length];
        _thoughtRemainString = new string[length];

        // �迭�� ��� ��Ҹ� -1�� �ʱ�ȭ�Ѵ�.
        for (byte i = 0; i < length; ++i)
        {
            _thoughtValues[i] = -1.0f;
        }
    }


    /// <summary>
    /// ��ũƮ�� ����� ���� �ð� ���ڿ� �����´�.
    /// </summary>
    public string GetRemainString(TechTreeType type, byte index, TechTrees.Node[] nodeData)
    {
        switch (type)
        {
            case TechTreeType.Tech:
                if ((_techValues[index] != _adopted[(int)type][index]) || (null != _techRemainString[index]))
                {
                    _techValues[index] = _adopted[(int)type][index];
                    _techRemainString[index] = $"{((1.0f - _techValues[index]) / nodeData[index].ProgressionValue).ToString("0")}{Language.Instance["����"]}";
                }
                return _techRemainString[index];
            case TechTreeType.Thought:
                if ((_thoughtValues[index] != _adopted[(int)type][index]) || (null != _thoughtRemainString[index]))
                {
                    _thoughtValues[index] = _adopted[(int)type][index];
                    _thoughtRemainString[index] = $"{((1.0f - _thoughtValues[index]) / nodeData[index].ProgressionValue).ToString("0")}{Language.Instance["����"]}";
                }
                return _thoughtRemainString[index];
            default:
                Debug.LogError("UIString - �߸��� ��ũƮ��");
                return null;
        }
    }



    /* ==================== Private Methods ==================== */

    private UIString()
    {
        // SI ����
        _floatUnits[(int)VariableFloat.TotalAirPressure_hPa] = "hPa";
        _floatUnits[(int)VariableFloat.TotalAirMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.EtcAirMass_Tt] = "Tt";
        _floatUnits[(int)VariableFloat.TotalTemperature_C] = "��";
        _floatUnits[(int)VariableFloat.IncomeEnergy] = "E";
        _floatUnits[(int)VariableFloat.AbsorbEnergy] = "E";
        _floatUnits[(int)VariableFloat.WaterGreenHouse_C] = "��";
        _floatUnits[(int)VariableFloat.CarbonGreenHouse_C] = "��";
        _floatUnits[(int)VariableFloat.EtcGreenHouse_C] = "��";
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
        _floatUnits[(int)VariableFloat.GravityAccelation_m_s2] = "m/s��";
        _floatUnits[(int)VariableFloat.PlanetRadius_km] = "km";
        _floatUnits[(int)VariableFloat.PlanetDensity_g_cm3] = "g/cm��";
        _floatUnits[(int)VariableFloat.PlanetMass_Tt] = "Zt";
        _floatUnits[(int)VariableFloat.PlanetDistance_AU] = "AU";
        _floatUnits[(int)VariableFloat.PlanetArea_km2] = "Mm2";

        // ������ ����
        _floatUnits[(int)VariableFloat.FacilitySupportRate] = "%";
        _floatUnits[(int)VariableFloat.ResearchSupportRate] = "%";
        _floatUnits[(int)VariableFloat.SocietySupportRate] = "%";
        _floatUnits[(int)VariableFloat.DiplomacySupportRate] = "%";

        _adopted = PlayManager.Instance.GetAdoptedData();
    }
}
