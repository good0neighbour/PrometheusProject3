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
    [SerializeField] private Image _adoptProgressionImage = null;
    [SerializeField] private ScreenSociety _societyScreen = null;
    [SerializeField] private GameObject _previousScreen = null;

    private List<NodeElementSociety> _elements = new List<NodeElementSociety>();
    Dictionary<string, byte> _nodeIndex = null;
    private Image[] _nodeImages = null;
    private Image[] _elementImages = null;
    private float[][] _adopted = null;
    private float[] _elementProgression = null;
    private short _currentNode = 0;
    private short _currentElement = 0;
    private float _supportRate = 0.0f;
    private float _timer = 0.0f;
    private bool _isAdoptAvailable = false;
    private bool _adoptAnimationProceed = false;
    private bool _isBackBtnAvailable = true;

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

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��ư ��� �Ұ�
        _isAdoptAvailable = false;

        // �ڷΰ��� ����
        _isBackBtnAvailable = false;
        _backBtnText.color = Constants.TEXT_BUTTON_DISABLE;

        // ���� ��ư Ŭ�� �� ����
        if (-1 < _currentNode)
        {
            _nodes[_currentNode].BtnAdopt();
        }
        else
        {
            _elements[_currentElement].BtnAdopt();
        }

        // �ִϸ��̼� ����
        AdoptAnimation(PlayManager.Instance[VariableFloat.SocietySupportRate]);
    }


    public void ChangeState()
    {
        // ��� �Ұ�
        if (!_isBackBtnAvailable)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // �� â ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ó�� ���·� �ǵ�����.
        SetAdoptAvailable(false);
        _adoptBtnText.text = Language.Instance["����"];
        _descriptionText.text = null;
        _costText.text = null;
        _gainText.text = null;

        // ���� ������ ��Ȱ��ȭ
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

        // ���� â Ȱ��ȭ
        _previousScreen.SetActive(true);
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

        // ���� ��� �迭 ����
        byte elementLength = (byte)_elements.Count;
        _elementProgression = PlayManager.Instance.GetSocietyElementProgression(elementLength);

        // ���� ��� ��� ��� ����
        _elementImages = new Image[elementLength];
        for (byte i = 0; i < elementLength; ++i)
        {
            _elementImages[i] = _elements[i].GetComponent<Image>();
        }

        // ���� �Ұ� ���·� �����Ѵ�.
        SetAdoptAvailable(false);

        // ��� ���� Ȯ��
        for (byte i = 0; i < _adopted[(int)TechTreeType.Society].Length; ++i)
        {
            if (1.0f <= _adopted[(int)TechTreeType.Society][i])
            {
                _nodes[i].AlreadyAdopted(_nodes);
            }
        }
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
            // ��� ����
            _nodeImages[_currentNode].color = Constants.BUTTON_SELECTED;

            // ���� ��ư �ؽ�Ʈ ������Ʈ
            if (1.0f <= _adopted[(int)TechTreeType.Society][_currentNode])
            {
                _adoptBtnText.text = Language.Instance["���� �Ϸ�"];
                SetAdoptAvailable(false);
            }
            else
            {
                if (!_nodes[_currentNode].IsAvailable)
                {
                    _adoptBtnText.text = Language.Instance["���� �Ұ�"];
                    SetAdoptAvailable(false);
                }
                else
                {
                    _adoptBtnText.text = Language.Instance["����"];
                    SetAdoptAvailable(true);
                }
            }
        }
        else
        {
            // ��� ����
            _elementImages[_currentElement].color = Constants.BUTTON_SELECTED;

            // ���� ��ư �ؽ�Ʈ ������Ʈ
            if (1.0f <= _elementProgression[_currentElement])
            {
                _adoptBtnText.text = Language.Instance["���� �Ϸ�"];
                SetAdoptAvailable(false);
            }
            else
            {
                if (!_elements[_currentElement].IsAvailable)
                {
                    _adoptBtnText.text = Language.Instance["���� �Ұ�"];
                    SetAdoptAvailable(false);
                }
                else
                {
                    _adoptBtnText.text = Language.Instance["����"];
                    SetAdoptAvailable(true);
                }
            }
        }

        // �ؽ�Ʈ ������Ʈ
        _descriptionText.text = description;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� Ȯ��
    /// </summary>
    private bool CostAvailable()
    {
        if (-1 < _currentNode)
        {
            return _nodes[_currentNode].CostAvailable();
        }
        else
        {
            return _elements[_currentElement].CostAvailable();
        }
    }


    private void SetAdoptAvailable(bool available)
    {
        _isAdoptAvailable = available;
        if (_isAdoptAvailable)
        {
            _adoptBtnText.color = Constants.WHITE;
            _isAdoptAvailable = true;
        }
        else
        {
            _adoptBtnText.color = Constants.TEXT_BUTTON_DISABLE;
            _isAdoptAvailable = false;
        }
    }


    /// <summary>
    /// ���� ���ϸ��̼� ����
    /// </summary>
    private void AdoptAnimation(float supportRate)
    {
        _adoptAnimationProceed = true;
        _supportRate = supportRate;
    }


    private void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ������ ���
        PlayManager.Instance[VariableFloat.SocietySupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (100.0f < PlayManager.Instance[VariableFloat.SocietySupportRate])
        {
            PlayManager.Instance[VariableFloat.SocietySupportRate] = 100.0f;
        }

        // ������ ��� �̳����̼�
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);

        // ���� �� ����
        if (-1 < _currentNode)
        {
            _nodes[_currentNode].OnAdopt(_adopted[(int)TechTreeType.Society], _nodes);
            _adoptBtnText.text = Language.Instance["���� �Ϸ�"];
            SetAdoptAvailable(false);

            // ��ȸ ȭ�� ��ư �̹��� ������Ʈ
            _societyScreen.SocietyImageUpdate();
        }
        else
        {
            _elements[_currentElement].OnAdopt(_elementProgression);

            if (1.0f <= _elementProgression[_currentElement])
            {
                _adoptBtnText.text = Language.Instance["���� �Ϸ�"];
                SetAdoptAvailable(false);
            }
        }
    }


    private void OnFail()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Failed);

        // ������ ����
        PlayManager.Instance[VariableFloat.SocietySupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (0.0f > PlayManager.Instance[VariableFloat.SocietySupportRate])
        {
            PlayManager.Instance[VariableFloat.SocietySupportRate] = 0.0f;
        }

        // ������ ��� �̳����̼�
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);
    }


    private void OnEnable()
    {
        for (byte i = 0; i < _nodes.Length; ++i)
        {
            _nodes[i].CheckEnable(_adopted, _nodeIndex);
        }
    }


    private void Update()
    {
        if (_adoptAnimationProceed)
        {
            _timer += Time.deltaTime;
            _adoptProgressionImage.fillAmount = _timer;
            if (1.0f <= _timer)
            {
                if (_supportRate >= Random.Range(0.0f, Constants.MAX_SUPPORT_RATE_ADOPTION))
                {
                    OnAdopt();
                }
                else
                {
                    OnFail();
                }

                // ��� Ȯ�� �� ���� ��ư Ȱ��ȭ
                SetAdoptAvailable(CostAvailable());

                // �ڷΰ��� ����
                _isBackBtnAvailable = true;
                _backBtnText.color = Constants.WHITE;

                _adoptAnimationProceed = false;
                _adoptProgressionImage.fillAmount = 0.0f;
                _timer = 0.0f;
            }
        }
    }
}
