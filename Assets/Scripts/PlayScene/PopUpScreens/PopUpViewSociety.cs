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
        // ��� �Ұ�
        if (!_isAdoptAvailable)
        {
            return;
        }

        // ���� �� ����
        if (-1 < _currentNode)
        {
            _nodes[_currentNode].OnAdopt();
        }
        else
        {
            _elements[_currentElement].OnAdopt(_elementProgression);
        }

        Debug.Log("BtnAdopt");
    }


    public void ChangeState()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �� â ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ó�� ���·� �ǵ�����.
        SetAdoptButtonAvailable(false);
        _adoptBtnText.text = Language.Instance["����"];
        _descriptionText.text = null;
        _costText.text = null;
        _gainText.text = null;

        // ���� ���� ��Ȱ��ȭ
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_UNSELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_UNSELECTED;
        }

        // ���� �簳
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;
    }
    

    public void Execute()
    {
        // �� â Ȱ��ȭ
        gameObject.SetActive(true);
    }


    public void Activate()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // ����
        _adopted = PlayManager.Instance.GetAdoptedData();
        _elementProgression = PlayManager.Instance.GetSocietyElementProgression();
        TechTrees techTreeData = PlayManager.Instance.GetTechTreeData();
        _nodeIndex = techTreeData.GetIndexDictionary();

        // �迭 ����
        byte length = (byte)_nodes.Length;
        _adopted[(int)TechTreeType.Society] = new float[length];
        _nodeImages = new Image[length];

        for (byte i = 0; i < length; ++i)
        {
            // ��� �ʱ�ȭ
            _nodes[i].SetNode(i, _elements, techTreeData, _nodeIndex);

            // ��� ��� �����´�.
            _nodeImages[i] = _nodes[i].GetComponent<Image>();
        }

        // ���� ��� ��� ��� ����
        _elementImages = new Image[_elements.Count];
        for (byte i = 0; i < _elements.Count; ++i)
        {
            _elementImages[i] = _elements[i].GetComponent<Image>();
        }

        // ���� ��� ���൵ �迭 ����
        _elementProgression = new float[_elements.Count];

        // ���� �Ұ� ���·� �����Ѵ�.
        SetAdoptButtonAvailable(false);
    }


    public virtual void NodeSelect(short currentNode, short currentElement, string description, bool isAvailable)
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ���� ��Ȱ��ȭ
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_UNSELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_UNSELECTED;
        }

        // ���� ��� ����
        _currentNode = currentNode;
        _currentElement = currentElement;

        // ���� ��� Ȱ��ȭ
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_SELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_SELECTED;
        }

        // �ؽ�Ʈ ������Ʈ
        _descriptionText.text = description;

        // ��� ���� ����
        SetAdoptButtonAvailable(isAvailable);
    }



    /* ==================== Private Methods ==================== */

    private void SetAdoptButtonAvailable(bool isAvailable)
    {
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


    private void OnEnable()
    {
        for (byte i = 0; i < _nodes.Length; ++i)
        {
            _nodes[i].CheckEnable(_adopted, _nodeIndex);
        }
    }
}
