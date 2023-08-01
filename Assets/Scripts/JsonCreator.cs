#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class JsonCreator : MonoBehaviour
{
    public void LanSave()
    {
        Language.Instance.LanguageSave();
        AssetDatabase.Refresh();
    }

    public void OhterLanSave()
    {
        Language.Instance.SaveOtherLanguages();
        AssetDatabase.Refresh();
    }
}
#endif
