using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenResearch : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [Header("참조")]
    [SerializeField] private PopUpScreenTechTree _popUpTechScreen = null;
    [SerializeField] private TMP_Text _techResearchTitleText = null;
    [SerializeField] private TMP_Text _techResearchRemainText = null;
    [SerializeField] private Image _techResearchProgreesionImage = null;

    private TechTrees.Node[] _nodeData = null;
    private float[][] _adopted = null;



    /* ==================== Public Methods ==================== */

    public void BtnTechScreen()
    {
        _popUpTechScreen.ActiveThis(TechTreeType.Tech);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }


    public void BtnThoughtScreen()
    {
        _popUpTechScreen.ActiveThis(TechTreeType.Thought);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // 참조
        _adopted = PlayManager.Instance.GetAdoptedData();
        _nodeData = PlayManager.Instance.GetTechTreeData().GetNodes(TechTreeType.Tech);
    }


    private void Update()
    {
        // 실시간 타게팅 변경
        float[] adoped = _adopted[(int)TechTreeType.Tech];
        short index = -1;
        float progression = 0.0f;
        for (byte i = 0; i < adoped.Length; ++i)
        {
            if (progression < adoped[i] && 1.0f > adoped[i])
            {
                progression = adoped[i];
                index = i;
            }
        }

        if (-1 < index)
        {
            _techResearchTitleText.text = _nodeData[index].NodeName;
            _techResearchRemainText.text = UIString.Instance.GetRemainString(TechTreeType.Tech, (byte)index, _nodeData);
            _techResearchProgreesionImage.fillAmount = adoped[index];
        }
        else
        {
            _techResearchTitleText.text = Language.Instance["연구 없음"];
            _techResearchRemainText.text = null;
            _techResearchProgreesionImage.fillAmount = 0.0f;
        }
    }
}
