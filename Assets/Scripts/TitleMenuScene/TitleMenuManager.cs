using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _audioMamagerPrefab = null;



    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����Ƽ�� �̱��������� �����ϴµ�, �� ȭ������ ���� ������ AudioManager�� �����ϰ� �ı��ϴ� ���� �����Ѵ�.
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioMamagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }

        // ���� ������ ��� AutoTranslation�� ã�´�.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>();

        // ��� AutoTranslation�� �غ��Ų��.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }
    }
}
