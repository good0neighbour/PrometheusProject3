using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 현재 씬에서 모든 AutoTranslation을 찾는다.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>();

        // 모든 AutoTranslation을 준비시킨다.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }
    }
}
