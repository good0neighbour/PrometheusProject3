using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenLoading : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private TMP_Text _loadingText = null;

    private float _timer = 0.0f;

    private void Update()
    {
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, _timer);
        _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b, _timer);

        if (1.0 <= _timer)
        {
            SceneManager.LoadScene(1);
        }

        _timer += Time.deltaTime;
    }
}
