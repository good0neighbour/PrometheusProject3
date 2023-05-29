#region 언어
/// <summary>
/// 사용 가능한 언어 종류
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

#region 변수
/// <summary>
/// JsonData의 ByteArray 인덱스 접근을 위한 열거형
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
/// JsonData의 ShortArray 인덱스 접근을 위한 열거형
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
/// JsonData의 UshortArray 인덱스 접근을 위한 열거형
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
/// JsonData의 IntArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableInt
{
    AnnualFund,
    TradeIncome,
    EndInt
}

/// <summary>
/// JsonData의 IntArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableUint
{
    Research,
    Culture,
    Maintenance,
    EndUint
}

/// <summary>
/// JsonData의 LongArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableLong
{
    Funds,
    EndLong
}

/// <summary>
/// JsonData의 FloatArray 인덱스 접근을 위한 열거형
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
/// JsonData의 DoubleArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableDouble
{
    EndDouble
}
#endregion

#region 자원
public enum ResourceType
{
    Iron,
    Nuke,
    Jewel,
    End
}
#endregion

#region 테크트리
public enum TechTreeType
{
    Facility,
    Tech,
    Thought,
    Society,
    TechTreeEnd
}
#endregion