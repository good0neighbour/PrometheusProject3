using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private GameObject _audioMamagerPrefab = null;



    /* ==================== Public Methods ==================== */

    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 유니티식 싱글턴패턴을 적용하는데, 주 화면으로 들어올 때마다 AudioManager를 생성하고 파괴하는 것을 방지한다.
        if (null == AudioManager.Instance)
        {
            GameObject audioManager = Instantiate(_audioMamagerPrefab);
            AudioManager.Instance = audioManager.GetComponent<AudioManager>();
            DontDestroyOnLoad(audioManager);
        }

        // 현재 씬에서 모든 AutoTranslation을 찾는다.
        AutoTranslation[] autoTranslations = FindObjectsOfType<AutoTranslation>();

        // 모든 AutoTranslation을 준비시킨다.
        for (ushort i = 0; i < autoTranslations.Length; ++i)
        {
            autoTranslations[i].TranslationReady();
        }
    }
}
