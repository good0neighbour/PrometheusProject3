using UnityEngine.SceneManagement;

public class ScreenStart : TitleMenuScreenBase
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    public void BtnStartPlay()
    {
        SceneManager.LoadScene(1);
    }



    /* ==================== Private Methods ==================== */
}
