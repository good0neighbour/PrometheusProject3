using UnityEngine;
using TMPro;

public class ScrollList : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("ũ�� ����")]
    [SerializeField] private Vector2 _anchorMaxCollapsed = new Vector2(0.7f, 0.55f);
    [SerializeField] private Vector2 _anchorMaxExpanded = new Vector2(0.7f, 0.8f);

    [Header("����")]
    [SerializeField] private TMP_Text _listExpandBtn = null;

    private RectTransform _rectTransfrom = null;
    private bool _isExpended = false;



    /* ==================== Public Methods ==================== */

    public void BtnListExpand()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��� ���
        if (_isExpended)
        {
            _rectTransfrom.anchorMax = _anchorMaxCollapsed;
            _listExpandBtn.text = Constants.ON_COLLAPSED;
            _isExpended = false;
        }
        // ��� Ȯ��
        else
        {
            _rectTransfrom.anchorMax = _anchorMaxExpanded;
            _listExpandBtn.text = Constants.ON_EXPANDED;
            _isExpended = true;
        }
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����
        _rectTransfrom = GetComponent<RectTransform>();
    }
}
