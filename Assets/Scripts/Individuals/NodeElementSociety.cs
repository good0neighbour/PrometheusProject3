using UnityEngine;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;

public class NodeElementSociety : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("기본")]
    [SerializeField] private string _elementName = null;
    [SerializeField] private string _description = null;

    [Header("참조")]
    [SerializeField] private TMP_Text _titleText = null;

    private byte _elementNum = 0;
    private bool _isAvailable = false;



    /* ==================== Public Methods ==================== */

    public void BtnTouch()
    {
        PopUpViewSociety.Instance.NodeSelect(-1, _elementNum, _description, _isAvailable);
    }


    public void SetElement(byte elementNum)
    {
        _elementNum = elementNum;
        _titleText.text = _elementName;
    }


    public void SetAvailable(bool available)
    {
        _isAvailable = available;
    }



    /* ==================== Private Methods ==================== */
}
