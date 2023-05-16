using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpViewThought : TechTreeBase, IActivateFirst
{
    /* ==================== Variables ==================== */

    private TMP_Text[] _titleTexts = null;
    private TMP_Text[] _remainTexts = null;
    private Image[] _progreesionImages = null;
    private List<byte> _onProgress = new List<byte>();
    private float _researchSpeedmult = 1.0f / Constants.MONTH_TIMER;



    /* ==================== Public Methods ==================== */

    public override void BtnAdopt()
    {
        // ��� �Ұ�
        if (!IsAdoptAvailable)
        {
            return;
        }

        // �θ� Ŭ������ ���� ����
        base.BtnAdopt();

        // ���� �ִϸ��̼�
        //AdoptAnimation(PlayManager.Instance[VariableFloat.ResearchSupportRate]);
        AdoptAnimation(75.0f);
    }


    public void Activate()
    {
        // ��� ���� ��������
        TechTreeData = PlayManager.Instance.GetTechTreeData();
        NodeData = TechTreeData.GetNodes(TechTreeType.Thought);
        NextNodes = TechTreeData.GetNextNodes(TechTreeType.Thought);

        // �迭 ����
        byte length = (byte)NodeData.Length;
        _titleTexts = new TMP_Text[length];
        _remainTexts = new TMP_Text[length];
        _progreesionImages = new Image[length];

        // UIString ��� ���� ���·� �����.
        UIString.Instance.ThoughtInitialize(length);

        BasicInitialize(length);

        for (byte i = 0; i < length; ++i)
        {
            // ����
            Transform node = NodeBtnObjects[i].transform;
            _titleTexts[i] = node.Find("TextTitle").GetComponent<TMP_Text>();
            _remainTexts[i] = node.transform.Find("TextRemain").GetComponent<TMP_Text>();
            _progreesionImages[i] = node.transform.Find("ImageProgressionBackground").Find("ImageProgression").GetComponent<Image>();

            // ���� ���� ���̾��� �� ���� �迭�� �߰�
            if (0.0f < Adopted[(int)TechTreeType.Thought][i])
            {
                _onProgress.Add(i);
            }
        }

        // ��Ȱ��ȭ�� ��
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // �䱸 ���� Ȯ��
            TechTrees.Node.SubNode[] requiredNodes = NodeData[i].Requirments;
            for (byte j = 0; j < requiredNodes.Length; ++j)
            {
                // �� ���� ���ε����� Ȱ��ȭ
                if (0.0f < Adopted[(int)requiredNodes[j].Type][NodeIndex[requiredNodes[j].NodeName]])
                {
                    NodeBtnObjects[i].SetActive(true);
                    break;
                }
                else
                {
                    // �ϴ� ��Ȱ��ȭ
                    NodeBtnObjects[i].SetActive(false);
                }
            }
        }

        // �ؽ�Ʈ ������Ʈ
        OnLanguageChange();

        // �븮�� ���
        Language.OnLanguageChange += OnLanguageChange;
        PlayManager.OnPlayUpdate += TechResearchProgress;
    }


    /// <summary>
    /// ���� ���� �� ��� �����´�.
    /// </summary>
    public List<byte> GetProgressionList()
    {
        return _onProgress;
    }



    /* ==================== Protected Methods ==================== */

    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        // Ȱ��ȭ ���� ����
        Adopted[(int)TechTreeType.Thought][CurrentNode] = 0.0001f;

        // ���� �������� ����Ʈ�� �߰�
        _onProgress.Add(CurrentNode);
    }


    protected override void OnFail()
    {

    }


    protected override bool IsUnadopted()
    {
        bool result = (0.0f < Adopted[(int)TechTreeType.Thought][NodeIndex[NodeData[CurrentNode].NodeName]]);
        if (result)
        {
            AdoptBtn.text = Language.Instance["���� �Ϸ�"];
        }
        else
        {
            AdoptBtn.text = Language.Instance["����"];
        }
        return !result;
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��� ǥ�� ������Ʈ
    /// </summary>
    private void NodeUpdate(byte index)
    {
        // ���� ����
        _remainTexts[index].text = UIString.Instance.GetRemainString(TechTreeType.Thought, index, NodeData);

        // ���� ���� �̹���
        _progreesionImages[index].fillAmount = Adopted[(int)TechTreeType.Thought][index];
    }


    /// <summary>
    /// ���� ����
    /// </summary>
    private void TechResearchProgress()
    {
        for (byte i = 0; i < _onProgress.Count; ++i)
        {
            // ���� ����
            Adopted[(int)TechTreeType.Thought][_onProgress[i]] += NodeData[_onProgress[i]].ProgressionPerMonth * _researchSpeedmult * PlayManager.Instance.GameSpeed * Time.deltaTime;

            // ���� �Ϸ�
            if (1.0f <= Adopted[(int)TechTreeType.Thought][_onProgress[i]])
            {
                List<TechTrees.Node.SubNode> nextNodes = NextNodes[_onProgress[i]];
                for (byte j = 0; j < nextNodes.Count; ++j)
                {
                    switch (nextNodes[j].Type)
                    {
                        case TechTreeType.Thought:
                            // ���� ����� ���� �Ϸ� �������� ���� ���� ���� Ȱ��ȭ�Ѵ�.
                            byte index = NodeIndex[nextNodes[j].NodeName];
                            NodeBtnObjects[index].SetActive(true);
                            NodeUpdate(index);
                            break;
                        default:
                            // �ٸ� ��ũƮ���� Ȱ��ȭ ���θ� �ش� ��ũƮ���� ������ �����Ѵ�.
                            break;
                    }
                }

                // ���� ���� ���� ����Ʈ���� ����
                _onProgress.Remove(_onProgress[i]);

                // ������ ���� �迭 �ε����� �����.
                --i;
            }
        }
    }


    private void OnLanguageChange()
    {
        for (byte i = 0; i < _titleTexts.Length; ++i)
        {
            // ��� �̸�. 
            _titleTexts[i].text = NodeData[i].NodeName;
        }
    }


    private void OnEnable()
    {
        // Ȱ��ȭ, ��Ȱ��ȭ
        for (byte i = 0; i < NodeBtnObjects.Length; ++i)
        {
            // �̹� Ȱ��ȭ �� �͸�
            if (NodeBtnObjects[i].activeSelf)
            {
                NodeUpdate(i);
            }
        }
    }
}
