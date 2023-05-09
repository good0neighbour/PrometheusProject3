using System;
using UnityEngine;

public static class Constants
{
    // 수학 값
    public const float PI = Mathf.PI;
    public const float HALF_PI = Mathf.PI * 0.5f;
    public const float DOUBLE_PI = Mathf.PI * 2.0f;
    public const float TRIPLE_PI = Mathf.PI * 3.0f;

    // 단위 조정용 값
    public const double E3 = 100.0d;
    public const double E7 = 10000000.0d;

    // 물리 값
    public const float MIN_KELVIN = -273.0f;
    public const float MAX_ICE_TEMP = 20.0f;
    public const double GRAVITY_COEFICIENT = 0.0000000000667428d;
    public const double PLANET_GRAVITY_ADJUST = 1000517274389.2647640484152314488d;
    public const double PLANET_MASS_ADJUST = 0.00000099760221803384511479248479118797d;
    public const double PLANET_AREA_ADJUST = 0.99783185418734494087135323844218d;

    // UI관련 값
    public const float CANVAS_HEIGHT = 1080.0f;
    public const float CANVAS_WIDTH = 1920.0f;
    public const float MASSAGEBOX_HEIGHT_RATIO = 0.15f;
    public const float SPACE_IMAGE_TARGET_POSITION = 192.0f;

    // 게임 플레이 관련 값
    public const float AIRMASS_MOVEMENT = 5.0f;
    public const float MONTH_TIMER = 1.0f;

    // 색상
    public static readonly Color WHITE = new Color(1.0f, 1.0f, 1.0f);
    public static readonly Color TEXT_BUTTON_DISABLE = new Color(0.5f, 0.5f, 0.5f);

    // 수정이 필요 없는 값
    public const float HALF_CANVAS_HEIGHT = CANVAS_HEIGHT * 0.5f;
    public const float QUARTER_CANVAS_HEIGHT = CANVAS_HEIGHT * 0.25f;
    public const float HALF_CANVAS_WIDTH = CANVAS_WIDTH * 0.5f;
    public const double MAX_ICE_TEMP_LOG = MAX_ICE_TEMP + 1.0d;
}
