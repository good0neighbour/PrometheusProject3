#region ���
/// <summary>
/// ��� ������ ��� ����
/// </summary>
public enum LanguageType
{
    Korean,
    English,
    German,
    French,
    Taiwanese,
    Japanese,
    End
}
#endregion

#region ����
/// <summary>
/// JsonData�� ByteArray �ε��� ������ ���� ������
/// </summary>
public enum VariableByte
{
    Month,
    ExploreDevice,
    AirPressureInfra,
    TemperatureInfra,
    WaterInfra,
    CarbonInfra,
    Era,
    EndByte
}

/// <summary>
/// JsonData�� ShortArray �ε��� ������ ���� ������
/// </summary>
public enum VariableShort
{
    AirMassMovement,
    TemperatureMovement,
    WaterMovement,
    CarbonMovement,
    EndShort
}

/// <summary>
/// JsonData�� UshortArray �ε��� ������ ���� ������
/// </summary>
public enum VariableUshort
{
    Year,
    LandNum,
    CityNum,
    TradeNum,
    CurrentIron,
    CurrentNuke,
    CurrentJewel,
    TotalIron,
    TotalNuke,
    TotalJewel,
    IronUsage,
    NukeUsage,
    JewelUsage,
    AnnualResearch,
    AnnualCulture,
    EndUshort
}

/// <summary>
/// JsonData�� IntArray �ε��� ������ ���� ������
/// </summary>
public enum VariableInt
{
    AnnualFund,
    TradeIncome,
    EndInt
}

/// <summary>
/// JsonData�� IntArray �ε��� ������ ���� ������
/// </summary>
public enum VariableUint
{
    Research,
    Culture,
    Maintenance,
    EndUint
}

/// <summary>
/// JsonData�� LongArray �ε��� ������ ���� ������
/// </summary>
public enum VariableLong
{
    Funds,
    EndLong
}

/// <summary>
/// JsonData�� FloatArray �ε��� ������ ���� ������
/// </summary>
public enum VariableFloat
{
    TotalAirPressure_hPa,
    TotalAirMass_Tt,
    EtcAirMass_Tt,
    TotalTemperature_C,
    IncomeEnergy,
    AbsorbEnergy,
    TotalReflection,
    GroundReflection,
    WaterReflection,
    IceReflection,
    CloudReflection,
    WaterGreenHouse_C,
    CarbonGreenHouse_C,
    EtcGreenHouse_C,
    TotalWater_PL,
    WaterGas_PL,
    WaterLiquid_PL,
    WaterSolid_PL,
    TotalCarbonRatio_ppm,
    CarbonGasMass_Tt,
    CarbonLiquidMass_Tt,
    CarbonSolidMass_Tt,
    CarbonLifeMass_Tt,
    PhotoLifePosibility,
    BreathLifePosibility,
    PhotoLifeStability,
    BreathLifeStability,
    OxygenRatio,
    GravityAccelation_m_s2,
    PlanetRadius_km,
    PlanetDensity_g_cm3,
    PlanetMass_Tt,
    PlanetDistance_AU,
    PlanetArea_km2,
    ExploreProgress,
    ExploreGoal,
    FacilitySupportRate,
    ResearchSupportRate,
    SocietySupportRate,
    DiplomacySupportRate,
    GovAsset,
    GovAffection,
    PopulationAdjustment,
    EndFloat
}

/// <summary>
/// JsonData�� DoubleArray �ε��� ������ ���� ������
/// </summary>
public enum VariableDouble
{
    EndDouble
}
#endregion

#region �ڿ�
public enum ResourceType
{
    Iron,
    Nuke,
    Jewel,
    End
}
#endregion

#region ��ũƮ��
public enum TechTreeType
{
    Facility,
    Tech,
    Thought,
    Society,
    TechTreeEnd
}
#endregion