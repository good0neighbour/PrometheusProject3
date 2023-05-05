using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ���� ������ ��� AutoTranslation�� ã�´�.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>();

        // ��� AutoTranslation�� �غ��Ų��.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }
    }
}
