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
    ExploreDevice,
    AirPressureInfra,
    TemperatureInfra,
    WaterInfra,
    CarbonInfra,
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
    LandNum,
    CityNum,
    EndUshort
}

/// <summary>
/// JsonData의 IntArray 인덱스 접근을 위한 열거형
/// </summary>
public enum VariableInt
{
    EndInt
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
public enum FaciityTag
{
    Electricity0,
    Electricity1,
    Electricity2,
    Food0,
    Food1,
    Food2,
    Food3,
    Food4,
    Food5,
    Health0,
    Health1,
    Health2,
    Health3,
    Health4,
    Health5,
    Safety0,
    Safety1,
    Safety2,
    Safety3,
    Safety4,
    Police0,
    Police1,
    Police2,
    Police3,
    Police4,
    Police5,
    WaterTreatment0,
    WaterTreatment1,
    WaterTreatment2,
    WaterTreatment3,
    WaterTreatment4,
    WaterTreatment5,
    WaterTreatment6,
    WaterTreatment7,
    Industry0,
    Industry1,
    Industry2,
    Industry3,
    Industry4,
    Industry5,
    Education0,
    Education1,
    Education2,
    Education3,
    Education4,
    Education5,
    FacilityEnd
}

public enum TechTag
{
    Tech0_0,
    Tech0_1,
    Tech0_2,
    Tech0_3,
    Tech0_4,
    Tech0_5,
    Tech0_6,
    Tech0_7,
    Tech1_0,
    Tech1_1,
    Tech1_2,
    Tech1_3,
    Tech2_0,
    Tech2_1,
    Tech2_2,
    Tech2_3,
    Tech2_4,
    Tech2_5,
    Tech2_6,
    TechEnd
}

public enum ThoughtTag
{
    Thought0_1,
    Thought0_2,
    Thought1_0,
    Thought2_0,
    Thought2_1,
    Thought2_2,
    Thought2_3,
    Thought3_0,
    Thought3_1,
    Thought3_2,
    Thought4_0,
    Thought4_1,
    Thought4_2,
    Thought4_3,
    Thought4_4,
    Thought5_0,
    Thought5_1,
    Thought5_2,
    Thought5_3,
    Thought6_0,
    Thought6_1,
    Thought6_2,
    Thought6_3,
    ThoughtEnd
}

public enum SocietyTag
{
    Society0_0,
    Society1_0,
    Society1_1,
    Society2_0,
    Society2_1,
    Society2_2,
    Society3_0,
    Society3_1,
    Society3_2,
    Society3_3,
    Society4_0,
    Society4_1,
    Society4_2,
    Society4_3,
    Society4_4,
    SocietyEnd
}
#endregion