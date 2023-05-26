using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _audioMamagerPrefab = null;
    [SerializeField] private GameObject _mainScreen = null;
    [SerializeField] private GameObject _loadingScreen = null;



    /* ==================== Public Methods ==================== */

    public void BtnStartGame()
    {
        GameManager.Instance.IsNewGame = true;
        Language.OnLanguageChange = null;
        _mainScreen.SetActive(false);
        _loadingScreen.SetActive(true);
    }


    public void BtnLoadGame()
    {
        GameManager.Instance.IsNewGame = false;
        Language.OnLanguageChange = null;
        _mainScreen.SetActive(false);
        _loadingScreen.SetActive(true);
    }



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
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>(true);

        // ��� AutoTranslation�� �غ��Ų��.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }
    }
}
