using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ScreenResearch : PlayScreenBase, IActivateFirst
{
    /* ==================== Variables ==================== */

    [Header("����")]
    [SerializeField] private PopUpScreenTechTree _popUpTechScreen = null;
    [SerializeField] private TMP_Text _techResearchTitleText = null;
    [SerializeField] private TMP_Text _techResearchRemainText = null;
    [SerializeField] private Image _techResearchProgreesionImage = null;
    [SerializeField] private TMP_Text _thoughtResearchTitleText = null;
    [SerializeField] private TMP_Text _thoughtResearchRemainText = null;
    [SerializeField] private Image _thoughtResearchProgreesionImage = null;
    [SerializeField] private Image _supportRateImage = null;
    [SerializeField] private PopUpViewTech _techView = null;
    [SerializeField] private PopUpViewThought _thoughtView = null;

    private List<byte> _techOnProgress = new List<byte>();
    private List<byte> _thoughtOnProgress = new List<byte>();
    private TechTrees.Node[] _techData = null;
    private TechTrees.Node[] _thoughtData = null;
    private float[] _adoptedTech = null;
    private float[] _adoptedThought = null;



    /* ==================== Public Methods ==================== */

    public void BtnTechScreen()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpTechScreen.ActiveThis(TechTreeType.Tech);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;

        // �� â �ݴ´�.
        gameObject.SetActive(false);
    }


    public void BtnThoughtScreen()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpTechScreen.ActiveThis(TechTreeType.Thought);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;

        // �� â �ݴ´�.
        gameObject.SetActive(false);
    }


    public void Activate()
    {
        _techView.Activate();
        _thoughtView.Activate();
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����
        _adoptedTech = PlayManager.Instance.GetAdoptedData()[(int)TechTreeType.Tech];
        _adoptedThought = PlayManager.Instance.GetAdoptedData()[(int)TechTreeType.Thought];
        _techData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Tech);
        _thoughtData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Thought);
        _techOnProgress = _techView.GetProgressionList();
        _thoughtOnProgress = _thoughtView.GetProgressionList();
    }


    private void Update()
    {
        // �ǽð� Ÿ���� ����
        short index = -1;
        float progression = 0.0f;
        foreach (byte i in _techOnProgress)
        {
            if (progression < _adoptedTech[i] && 1.0f > _adoptedTech[i])
            {
                progression = _adoptedTech[i];
                index = i;
            }
        }

        if (-1 < index)
        {
            _techResearchTitleText.text = _techData[index].NodeName;
            _techResearchRemainText.text = UIString.Instance.GetRemainString(TechTreeType.Tech, (byte)index, _techData);
            _techResearchProgreesionImage.fillAmount = _adoptedTech[index];
        }
        else if (null != _techResearchRemainText.text)
        {
            _techResearchTitleText.text = Language.Instance["���ȭ ���� ����"];
            _techResearchRemainText.text = null;
            _techResearchProgreesionImage.fillAmount = 0.0f;
        }

        // �ǽð� Ÿ���� ����
        index = -1;
        progression = 0.0f;
        foreach (byte i in _thoughtOnProgress)
        {
            if (progression < _adoptedThought[i] && 1.0f > _adoptedThought[i])
            {
                progression = _adoptedThought[i];
                index = i;
            }
        }

        if (-1 < index)
        {
            _thoughtResearchTitleText.text = _thoughtData[index].NodeName;
            _thoughtResearchRemainText.text = UIString.Instance.GetRemainString(TechTreeType.Thought, (byte)index, _thoughtData);
            _thoughtResearchProgreesionImage.fillAmount = _adoptedThought[index];
        }
        else if (null != _thoughtResearchRemainText.text)
        {
            _thoughtResearchTitleText.text = Language.Instance["��� ���� ����"];
            _thoughtResearchRemainText.text = null;
            _thoughtResearchProgreesionImage.fillAmount = 0.0f;
        }

        // ������ �̹���
        _supportRateImage.fillAmount = PlayManager.Instance[VariableFloat.ResearchSupportRate] * 0.01f;

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex - 1);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
#endif
        // PC, ����� ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}
