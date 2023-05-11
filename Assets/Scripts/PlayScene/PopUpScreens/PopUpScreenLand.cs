using UnityEngine;

public class PopUpScreenLand : MonoBehaviour, IPopUpScreen
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        ScreenExplore.Instance.OpenLandScreen(false);
    }


    public void BtnBuildCity()
    {

    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // 단축키 동작
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ScreenExplore.Instance.OpenLandScreen(false);
        }
    }
}
