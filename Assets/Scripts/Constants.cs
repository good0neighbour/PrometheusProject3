using UnityEngine;

public static class Constants
{
    // 수학 값
    public const float PI = Mathf.PI;
    public const float HALF_PI = Mathf.PI * 0.5f;
    public const float DOUBLE_PI = Mathf.PI * 2.0f;
    public const float TRIPLE_PI = Mathf.PI * 3.0f;

    // UI관련 값
    public const float CANVAS_HEIGHT = 1080.0f;
    public const float CANVAS_WIDTH = 1920.0f;
    public const float MASSAGEBOX_HEIGHT_RATIO = 0.15f;
    public const float SPACE_IMAGE_TARGET_POSITION = 192.0f;

    // 수정이 필요 없는 값
    public const float HALF_CANVAS_HEIGHT = CANVAS_HEIGHT * 0.5f;
    public const float HALF_CANVAS_WIDTH = CANVAS_WIDTH * 0.5f;
}
