using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpViewSociety : MonoBehaviour, IState, IActivateFirst
{
    /* ==================== Variables ==================== */

    [SerializeField] private NodeSociety[] _nodes = null;
    [SerializeField] private NodeElementSociety[] _elements = null;
    [SerializeField] private TMP_Text _adoptBtnText = null;
    [SerializeField] private TMP_Text _backBtnText = null;
    [SerializeField] private TMP_Text _descriptionText = null;
    [SerializeField] private TMP_Text _costText = null;
    [SerializeField] private TMP_Text _gainText = null;

    private Image[] _nodeImages = null;
    private Image[] _elementImages = null;
    private byte _currentNode = 0;
    private byte _currentElement = 0;
    private bool _isAdoptAvailable = false;



    /* ==================== Public Methods ==================== */

    public void BtnAdopt()
    {

    }


    public void ChangeState()
    {
        // 이 창 비활성화
        gameObject.SetActive(false);

        // 처음 상태로 되돌린다.
        _adoptBtnText = null;
        _isAdoptAvailable = false;
        _descriptionText = null;
        _costText = null;
        _gainText = null;
    }
    

    public void Execute()
    {
        // 이 창 활성화
        gameObject.SetActive(true);
    }


    public void Activate()
    {
        byte length = (byte)_nodes.Length;
        _nodeImages = new Image[length];

        for (byte i = 0; i < length; ++i)
        {
            // 노드 초기화
            _nodes[i].SetNode(i, TechTreeType.Society, this);

            // 노드 배경 가져온다.
            _nodeImages[i] = _nodes[i].GetComponent<Image>();
        }
    }


    public virtual void NodeSelect(byte currentNode, byte currentElement)
    {
        _nodeImages[_currentNode].color = Constants.BUTTON_UNSELECTED;
        _elementImages[_currentElement].color = Constants.BUTTON_UNSELECTED;
        _currentNode = currentNode;
        _currentElement = currentElement;
        _nodeImages[_currentNode].color = Constants.BUTTON_SELECTED;
        _elementImages[_currentElement].color = Constants.BUTTON_SELECTED;
    }



    /* ==================== Private Methods ==================== */
}
