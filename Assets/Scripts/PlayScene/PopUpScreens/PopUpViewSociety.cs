using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpViewSociety : MonoBehaviour, IState, IActivateFirst
{
    /* ==================== Variables ==================== */

    [SerializeField] private NodeSociety[] _nodes = null;
    [SerializeField] private TMP_Text _adoptBtnText = null;
    [SerializeField] private TMP_Text _backBtnText = null;
    [SerializeField] private TMP_Text _descriptionText = null;
    [SerializeField] private TMP_Text _costText = null;
    [SerializeField] private TMP_Text _gainText = null;

    private List<NodeElementSociety> _elements = new List<NodeElementSociety>();
    Dictionary<string, byte> _nodeIndex = null;
    private Image[] _nodeImages = null;
    private Image[] _elementImages = null;
    private float[][] _adopted = null;
    private float[] _elementProgression = null;
    private short _currentNode = 0;
    private short _currentElement = 0;
    private bool _isAdoptAvailable = false;

    public static PopUpViewSociety Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnAdopt()
    {

    }


    public void ChangeState()
    {
        // 이 창 비활성화
        gameObject.SetActive(false);

        // 처음 상태로 되돌린다.
        _adoptBtnText.text = null;
        _isAdoptAvailable = false;
        _descriptionText.text = null;
        _costText.text = null;
        _gainText.text = null;

        // 이전 노드는 비활성화
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_UNSELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_UNSELECTED;
        }
    }
    

    public void Execute()
    {
        // 이 창 활성화
        gameObject.SetActive(true);
    }


    public void Activate()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 참조
        _adopted = PlayManager.Instance.GetAdoptedData();
        _elementProgression = PlayManager.Instance.GetSocietyElementProgression();
        TechTrees techTreeData = PlayManager.Instance.GetTechTreeData();
        _nodeIndex = techTreeData.GetIndexDictionary();

        // 배열 생성
        byte length = (byte)_nodes.Length;
        _adopted[(int)TechTreeType.Society] = new float[length];
        _nodeImages = new Image[length];

        for (byte i = 0; i < length; ++i)
        {
            // 노드 초기화
            _nodes[i].SetNode(i, _elements, techTreeData, _nodeIndex);

            // 노드 배경 가져온다.
            _nodeImages[i] = _nodes[i].GetComponent<Image>();
        }

        // 하위 요소 노드 배경 참조
        _elementImages = new Image[_elements.Count];
        for (byte i = 0; i < _elements.Count; ++i)
        {
            _elementImages[i] = _elements[i].GetComponent<Image>();
        }

        // 하위 요소 진행도 배열 생성
        _elementProgression = new float[_elements.Count];
    }


    public virtual void NodeSelect(short currentNode, short currentElement, string description, bool isAvailable)
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 이전 노드는 비활성화
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_UNSELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_UNSELECTED;
        }

        // 현재 노드 정보
        _currentNode = currentNode;
        _currentElement = currentElement;

        // 현재 노드 활성화
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_SELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_SELECTED;
        }

        // 텍스트 업데이트
        _descriptionText.text = description;

        // 사용 가능 여부
        _isAdoptAvailable = isAvailable;
        if (_isAdoptAvailable)
        {
            _adoptBtnText.color = Constants.WHITE;
        }
        else
        {
            _adoptBtnText.color = Constants.TEXT_BUTTON_DISABLE;
        }
    }



    /* ==================== Private Methods ==================== */

    private void OnEnable()
    {
        for (byte i = 0; i < _nodes.Length; ++i)
        {
            _nodes[i].CheckEnable(_adopted, _nodeIndex);
        }
    }
}
