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
    [SerializeField] private PopUpViewTech _techView = null;
    [SerializeField] private PopUpViewThought _thoughtView = null;

    private List<byte> _techOnProgress = new List<byte>();
    private List<byte> _thoughtOnProgress = new List<byte>();
    private TechTrees.Node[] _techData = null;
    private TechTrees.Node[] _thoughtData = null;
    private float[][] _adopted = null;



    /* ==================== Public Methods ==================== */

    public void BtnTechScreen()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpTechScreen.ActiveThis(TechTreeType.Tech);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnThoughtScreen()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpTechScreen.ActiveThis(TechTreeType.Thought);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
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
        _adopted = PlayManager.Instance.GetAdoptedData();
        _techData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Tech);
        _thoughtData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Thought);
        _techOnProgress = _techView.GetProgressionList();
        _thoughtOnProgress = _thoughtView.GetProgressionList();
    }


    private void Update()
    {
        // �ǽð� Ÿ���� ����
        float[] adoped = _adopted[(int)TechTreeType.Tech];
        short index = -1;
        float progression = 0.0f;
        foreach (byte i in _techOnProgress)
        {
            if (progression < adoped[i] && 1.0f > adoped[i])
            {
                progression = adoped[i];
                index = i;
            }
        }

        if (-1 < index)
        {
            _techResearchTitleText.text = _techData[index].NodeName;
            _techResearchRemainText.text = UIString.Instance.GetRemainString(TechTreeType.Tech, (byte)index, _techData);
            _techResearchProgreesionImage.fillAmount = adoped[index];
        }
        else
        {
            _techResearchTitleText.text = Language.Instance["���� ����"];
            _techResearchRemainText.text = null;
            _techResearchProgreesionImage.fillAmount = 0.0f;
        }

        // �ǽð� Ÿ���� ����
        adoped = _adopted[(int)TechTreeType.Thought];
        index = -1;
        progression = 0.0f;
        foreach (byte i in _thoughtOnProgress)
        {
            if (progression < adoped[i] && 1.0f > adoped[i])
            {
                progression = adoped[i];
                index = i;
            }
        }

        if (-1 < index)
        {
            _thoughtResearchTitleText.text = _techData[index].NodeName;
            _thoughtResearchRemainText.text = UIString.Instance.GetRemainString(TechTreeType.Thought, (byte)index, _thoughtData);
            _thoughtResearchProgreesionImage.fillAmount = adoped[index];
        }
        else
        {
            _thoughtResearchTitleText.text = Language.Instance["���� ����"];
            _thoughtResearchRemainText.text = null;
            _thoughtResearchProgreesionImage.fillAmount = 0.0f;
        }
    }
}
