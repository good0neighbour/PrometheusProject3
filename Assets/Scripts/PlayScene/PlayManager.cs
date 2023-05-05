using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    public void BtnLeftRight(bool isLeft)
    {
        if (isLeft)
        {
            AnimationManager.Instance.NoiseEffect();
        }
        else
        {
            AnimationManager.Instance.NoiseEffect();
        }
    }



    /* ==================== Private Methods ==================== */
}
