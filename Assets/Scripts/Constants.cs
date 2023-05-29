using UnityEngine;

public static class Constants
{
    // 수학 값
    public const float PI = Mathf.PI;
    public const float HALF_PI = Mathf.PI * 0.5f;
    public const float DOUBLE_PI = Mathf.PI * 2.0f;
    public const float TRIPLE_PI = Mathf.PI * 3.0f;
    public const float YEAR_TO_MONTH = 1.0f / 12.0f;

    // 단위 조정용 값
    public const float E_2 = 0.01f;
    public const float E_3 = 0.001f;
    public const double E3 = 100.0d;
    public const double E7 = 10000000.0d;

    // 물리 값
    public const float MIN_KELVIN = -273.0f;
    public const float MAX_ICE_TEMP = 20.0f;
    public const float EARTH_AIR_PRESSURE = 1013.25f;
    public const float EARTH_AIR_MASS_Tt = 5134.58f;
    public const float EARTH_TEMPERATURE = 15.0f;
    public const float EARTH_WATER_VOLUME_PL = 1408718.0f;
    public const float EARTH_WATER_LIQUID = 1379705.3f;
    public const float EARTH_RADIUS_km = 6378.14f;
    public const float EARTH_DENSITY_g_cm3 = 5.51f;
    public const float EARTH_CARBON_RATIO_ppm = 480.0f;
    public const double GRAVITY_COEFICIENT = 0.0000000000667428d;
    public const double PLANET_GRAVITY_ADJUST = 1000517274389.2647640484152314488d;
    public const double PLANET_MASS_ADJUST = 0.00000099760221803384511479248479118797d;
    public const double PLANET_AREA_ADJUST = 0.99783185418734494087135323844218d;

    // UI관련 값
    public const float CANVAS_HEIGHT = 1080.0f;
    public const float CANVAS_WIDTH = 1920.0f;
    public const float MASSAGEBOX_HEIGHT_RATIO = 0.15f;
    public const float METERIMAGE_WIDTH_RATIO = 0.3f;
    public const float SPACE_IMAGE_TARGET_POSITION = 192.0f;

    // 소리 관련 값
    public const float THEME_VOLUME = 0.3f;
    public const float THEME_FADE_OUT_SPEEDMULT = 0.2f;

    // 주메뉴 관련 값
    public const float TEXT_SCREEN_SPEEDMULT = 0.1f;
    public const float START_SCREEN_SPEEDMULT = 2.0f;

    // 게임 플레이 관련 값, 일반
    public const byte MEET_OPORTUNITY = 1;
    public const byte NUMBER_OF_FORCES = 4;
    public const byte NUMBER_OF_SLOTS = 5;
    public const byte START_FUND = 120;
    public const byte FUND_AGENDA = 10;
    public const ushort INITIAL_POPULATION = 10;
    public const float INITIAL_POPULATION_MOVEMENT = 0.0001f;
    public const float MONTH_TIMER = 2.0f;
    public const float GAME_RESUME = 1.0f;
    public const float GAME_PAUSE = 0.0f;
    public const float AIRMASS_MOVEMENT = 1.0f;
    public const float TEMPERATURE_MOVEMENT = 0.2f;
    public const float WATER_VOLUME_MOVEMENT = 5.0f;
    public const float CARBON_RATIO_MOVEMENT = 0.2f;
    public const float LIFE_STABILITY_SPEEDMULT = 0.05f;
    public const float EXPLORE_SPEEDMULT = 0.001f;
    public const float INITIAL_EXPLORE_GOAL = 0.01f;
    public const float EXPLORE_GOAL_INCREASEMENT = 3.0f;
    public const float TECHTREE_AREA_WIDTH_CENTER = 0.375f;
    public const float TECHTREE_AREA_HEIGHT_CENTER = 0.375f;
    public const float TECHTREE_AREA_WIDTH = 1536.0f;
    public const float TECHTREE_AREA_HEIGHT = 864.0f;
    public const float SPEND_ANIMATION_DURATION = 0.2f;
    public const float SUPPORT_RATE_DECREASEMENT = 1.2f;
    public const float SUPPORT_RATE_INCREASEMENT = 50.0f;
    public const float SUPPORT_RATE_CHANGE_BY_ADOPTION = 1.0f;
    public const float SUPPORT_RATE_SPEEDMULT = 0.05f;
    public const float MAX_SUPPORT_RATE_ADOPTION = 90.0f;
    public const float GOV_ASSET_MULTIPLY = 0.02f;
    public const float GOV_AFFECTION_MULTIPLY = 0.1f;
    public const float POPULATION_ADJUSTMENT_INCREASE = 1.1f;
    public const float POPULATION_ADJUSTMENT_DECREASE = 0.9f;
    public const float CRIME_RATE_SPEEDMULT = 0.05f;
    public const float DEATH_RATE_SPEEDMULT = 0.02f;
    public const float MIN_EVENT_POPULATION = 100.0f;
    public const float CRIME_POSIBILITY = 0.4f;
    public const float DISEASE_POSIBILITY = 0.8f;
    public const float TAX_PER_POPULATION = 0.025f;
    public const float FRIENDLY_INCREASEMENT_BY_DIPLOMACY0 = 0.01f;
    public const float FRIENDLY_INCREASEMENT_BY_DIPLOMACY1 = 0.05f;
    public const float FRIENDLY_INCREASEMENT_BY_DIPLOMACY2 = 0.1f;
    public const float HOSTILE_INCREASEMENT_BY_DIPLOMACY1 = 0.01f;
    public const float HOSTILE_INCREASEMENT_BY_DIPLOMACY2 = 0.03f;
    public const float CONQUEST_MOVEMENT = 0.1f;
    public const float GENERAL_DIPLOMACY_DECREASEMENT_MULTIPLY = 0.98f;
    public const float TRADE_HOSTILE_AFFECTION = 0.5f;
    public const float TRADE_FRIENDLY_INCREASEMENT = 0.0001f;
    public const string ON_EXPANDED = ">";
    public const string ON_COLLAPSED = ">";
    public const string FACILITY_UNADOPTED = "○";
    public const string FACILITY_ADOPTED = "●";
    public const string GAME_SPEED_PHASE_1 = "▶";
    public const string GAME_SPEED_PHASE_2 = "▶▶";
    public const string GAME_SPEED_PHASE_3 = "▶▶▶";
    public const string GAME_SPEED_PHASE_4 = "▶▶▶▶";

    // 게임 플레이 관련 값, 자원
    public static readonly short[,] RESOURCE_MIN_MAX = new short[(int)ResourceType.End, 2]
    {
        { -2, 5 },  // 철
        { -4, 2 },  // 핵물질
        { -5, 3 },  // 보석
    };
    public static readonly ushort[] RESOURCES_BASE_PRICE = new ushort[(int)ResourceType.End]
    {
        5,  // 철
        7,  // 핵물질
        10  // 보석
    };

    // 색상
    public static readonly Color WHITE = new Color(1.0f, 1.0f, 1.0f);
    public static readonly Color TEXT_BUTTON_DISABLE = new Color(0.5f, 0.5f, 0.5f);
    public static readonly Color BUTTON_UNSELECTED = new Color(50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f);
    public static readonly Color BUTTON_SELECTED = new Color(150.0f / 255.0f, 150.0f / 255.0f, 150.0f / 255.0f);
    public static readonly Color FAIL_TEXT = new Color(1.0f, 100.0f / 255.0f, 0.0f);
    public static readonly Color BOTTOM_TEXT_DEFAULT = new Color(220.0f / 255.0f, 220.0f / 255.0f, 220.0f / 255.0f);
    public static readonly Color SLOT_DISABLED = new Color(25.0f / 255.0f, 25.0f / 255.0f, 25.0f / 255.0f);
    public static readonly Color SLOT_ENABLED = new Color(50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f);

    // 수정이 필요 없는 값
    public const float MONTHLY_MULTIPLY = 1.0f / MONTH_TIMER;
    public const float HALF_CANVAS_HEIGHT = CANVAS_HEIGHT * 0.5f;
    public const float QUARTER_CANVAS_WIDTH = CANVAS_WIDTH * 0.25f;
    public const float HALF_CANVAS_WIDTH = CANVAS_WIDTH * 0.5f;
    public const float HALF_METAIMAGE_WIDTH = CANVAS_WIDTH * METERIMAGE_WIDTH_RATIO * 0.5f;
    public const double MAX_ICE_TEMP_LOG = MAX_ICE_TEMP + 1.0d;
}
