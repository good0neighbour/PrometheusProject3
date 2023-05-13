using UnityEngine;
using TMPro;

public class ScrollList : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("크기 조정")]
    [SerializeField] private Vector2 _anchorMaxCollapsed = new Vector2(0.7f, 0.55f);
    [SerializeField] private Vector2 _anchorMaxExpanded = new Vector2(0.7f, 0.8f);

    [Header("참조")]
    [SerializeField] private TMP_Text _listExpandBtn = null;

    private RectTransform _rectTransfrom = null;
    private bool _isExpended = false;



    /* ==================== Public Methods ==================== */

    public void BtnListExpand()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 목록 축소
        if (_isExpended)
        {
            _rectTransfrom.anchorMax = _anchorMaxCollapsed;
            _listExpandBtn.text = Constants.ON_COLLAPSED;
            _isExpended = false;
        }
        // 목록 확장
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
        // 참조
        _rectTransfrom = GetComponent<RectTransform>();
    }
}
