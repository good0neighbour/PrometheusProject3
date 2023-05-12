using UnityEngine;

public class JsonCreator : MonoBehaviour
{
    public void LanSave()
    {
        Language.Instance.LanguageSave();
    }

    public void OhterLanSave()
    {
        Language.Instance.SaveOtherLanguages();
    }
}
