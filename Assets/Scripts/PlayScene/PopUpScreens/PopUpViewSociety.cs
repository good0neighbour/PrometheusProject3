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
        // 사용 불가
        if (!_isAdoptAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 승인 버튼 사용 불가
        _isAdoptAvailable = false;

        // 뒤로가기 금지
        _isBackBtnAvailable = false;
        _backBtnText.color = Constants.TEXT_BUTTON_DISABLE;

        // 승인 버튼 클릭 시 동작
        if (-1 < _currentNode)
        {
            _nodes[_currentNode].BtnAdopt();
        }
        else
        {
            _elements[_currentElement].BtnAdopt();
        }

        // 애니메이션 실행
        AdoptAnimation(PlayManager.Instance[VariableFloat.SocietySupportRate]);
    }


    public void ChangeState()
    {
        // 사용 불가
        if (!_isBackBtnAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 이 창 비활성화
        gameObject.SetActive(false);

        // 처음 상태로 되돌린다.
        SetAdoptAvailable(false);
        _adoptBtnText.text = Language.Instance["승인"];
        _descriptionText.text = null;
        _costText.text = null;
        _gainText.text = null;

        // 이전 선택은 비활성화
        if (-1 < _currentNode)
        {
            _nodeImages[_currentNode].color = Constants.BUTTON_UNSELECTED;
        }
        else
        {
            _elementImages[_currentElement].color = Constants.BUTTON_UNSELECTED;
        }

        // 게임 재개
        PlayManager.Instance.GameResume = Constants.GAME_RESUME;

        // 이전 창 활성화
        _previousScreen.SetActive(true);
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

        // 하위 요소 배열 생성
        byte elementLength = (byte)_elements.Count;
        _elementProgression = PlayManager.Instance.GetSocietyElementProgression(elementLength);

        // 하위 요소 노드 배경 참조
        _elementImages = new Image[elementLength];
        for (byte i = 0; i < elementLength; ++i)
        {
            _elementImages[i] = _elements[i].GetComponent<Image>();
        }

        // 승인 불가 상태로 시작한다.
        SetAdoptAvailable(false);

        // 사용 가능 확인
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
            // 노드 선택
            _nodeImages[_currentNode].color = Constants.BUTTON_SELECTED;

            // 승인 버튼 텍스트 업데이트
            if (1.0f <= _adopted[(int)TechTreeType.Society][_currentNode])
            {
                _adoptBtnText.text = Language.Instance["승인 완료"];
                SetAdoptAvailable(false);
            }
            else
            {
                if (!_nodes[_currentNode].IsAvailable)
                {
                    _adoptBtnText.text = Language.Instance["승인 불가"];
                    SetAdoptAvailable(false);
                }
                else
                {
                    _adoptBtnText.text = Language.Instance["승인"];
                    SetAdoptAvailable(true);
                }
            }
        }
        else
        {
            // 노드 선택
            _elementImages[_currentElement].color = Constants.BUTTON_SELECTED;

            // 승인 버튼 텍스트 업데이트
            if (1.0f <= _elementProgression[_currentElement])
            {
                _adoptBtnText.text = Language.Instance["승인 완료"];
                SetAdoptAvailable(false);
            }
            else
            {
                if (!_elements[_currentElement].IsAvailable)
                {
                    _adoptBtnText.text = Language.Instance["승인 불가"];
                    SetAdoptAvailable(false);
                }
                else
                {
                    _adoptBtnText.text = Language.Instance["승인"];
                    SetAdoptAvailable(true);
                }
            }
        }

        // 텍스트 업데이트
        _descriptionText.text = description;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 비용 확인
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
    /// 승인 에니메이션 실행
    /// </summary>
    private void AdoptAnimation(float supportRate)
    {
        _adoptAnimationProceed = true;
        _supportRate = supportRate;
    }


    private void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 지지율 상승
        PlayManager.Instance[VariableFloat.SocietySupportRate] += Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (100.0f < PlayManager.Instance[VariableFloat.SocietySupportRate])
        {
            PlayManager.Instance[VariableFloat.SocietySupportRate] = 100.0f;
        }

        // 지지율 상승 이내메이션
        BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);

        // 승인 시 동작
        if (-1 < _currentNode)
        {
            _nodes[_currentNode].OnAdopt(_adopted[(int)TechTreeType.Society], _nodes);
            _adoptBtnText.text = Language.Instance["승인 완료"];
            SetAdoptAvailable(false);

            // 사회 화면 버튼 이미지 업데이트
            _societyScreen.SocietyImageUpdate();
        }
        else
        {
            _elements[_currentElement].OnAdopt(_elementProgression);

            if (1.0f <= _elementProgression[_currentElement])
            {
                _adoptBtnText.text = Language.Instance["승인 완료"];
                SetAdoptAvailable(false);
            }
        }
    }


    private void OnFail()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Failed);

        // 지지율 감소
        PlayManager.Instance[VariableFloat.SocietySupportRate] -= Constants.SUPPORT_RATE_CHANGE_BY_ADOPTION;
        if (0.0f > PlayManager.Instance[VariableFloat.SocietySupportRate])
        {
            PlayManager.Instance[VariableFloat.SocietySupportRate] = 0.0f;
        }

        // 지지율 상승 이내메이션
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

                // 비용 확인 후 승인 버튼 활성화
                SetAdoptAvailable(CostAvailable());

                // 뒤로가기 가능
                _isBackBtnAvailable = true;
                _backBtnText.color = Constants.WHITE;

                _adoptAnimationProceed = false;
                _adoptProgressionImage.fillAmount = 0.0f;
                _timer = 0.0f;
            }
        }
    }
}
