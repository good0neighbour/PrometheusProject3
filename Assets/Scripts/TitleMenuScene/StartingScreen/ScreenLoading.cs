using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScreenLoading : MonoBehaviour
{
    [SerializeField] private Image _icon = null;
    [SerializeField] private TMP_Text _loadingText = null;

    private float _timer = Constants.PI;

    private void Update()
    {
        float amount = Mathf.Cos(_timer) * 0.5f + 0.5f;
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, amount);
        _loadingText.color = new Color(_loadingText.color.r, _loadingText.color.g, _loadingText.color.b, amount);

        if (Constants.DOUBLE_PI <= _timer)
        {
            SceneManager.LoadScene(1);
        }

        _timer += Time.deltaTime * Constants.STARTING_TEXT_SPEEDMULT;
    }
}
