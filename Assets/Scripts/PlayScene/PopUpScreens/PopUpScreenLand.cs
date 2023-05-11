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
        // ����Ű ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ScreenExplore.Instance.OpenLandScreen(false);
        }
    }
}
