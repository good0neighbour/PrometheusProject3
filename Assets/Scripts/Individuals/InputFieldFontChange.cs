using UnityEngine;
using TMPro;

public class InputFieldFontChange : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private TMP_InputField _inputField = null;



    /* ==================== Public Methods ==================== */

    public void GetReady()
    {
        // 컴포넌트 가져온다
        _inputField = GetComponent<TMP_InputField>();

        // 찾을 수 없으면
        if (null == _inputField)
        {
            Debug.LogError($"InputFieldFontChange 잘못된 위치에 부착 - {name}");
            return;
        }

        // 언어 대리자에 등록한다.
        Language.OLC += OnLanguageChange;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 언어 변경 시 동작
    /// </summary>
    private void OnLanguageChange()
    {
        // 폰트 변경.
        _inputField.fontAsset = Language.Instance.GetFontAsset();
    }
}
