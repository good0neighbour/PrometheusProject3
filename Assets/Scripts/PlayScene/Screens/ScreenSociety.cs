using UnityEngine;

public class ScreenSociety : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private PopUpViewSociety _societyView = null;



    /* ==================== Public Methods ==================== */

    public void Activate()
    {
        _societyView.Activate();
    }



    /* ==================== Private Methods ==================== */
}