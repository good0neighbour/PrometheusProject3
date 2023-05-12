using UnityEngine;
using TMPro;

public class InputFieldFontChange : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private TMP_InputField _inputField = null;



    /* ==================== Public Methods ==================== */

    public void GetReady()
    {
        // ������Ʈ �����´�
        _inputField = GetComponent<TMP_InputField>();

        // ã�� �� ������
        if (null == _inputField)
        {
            Debug.LogError($"InputFieldFontChange �߸��� ��ġ�� ���� - {name}");
            return;
        }

        // ��� �븮�ڿ� ����Ѵ�.
        Language.OLC += OnLanguageChange;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� ���� �� ����
    /// </summary>
    private void OnLanguageChange()
    {
        // ��Ʈ ����.
        _inputField.fontAsset = Language.Instance.GetFontAsset();
    }
}
