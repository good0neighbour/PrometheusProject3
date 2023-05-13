using UnityEngine;

public class TechTreeNode : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private TechTreeBase _techTree = null;
    private byte _nodeNum = 0;



    /* ==================== Public Methods ==================== */

    public void BtnNodeTouch()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ��� ����
        _techTree.SetCurrentNode(_nodeNum, transform.position);
    }


    /// <summary>
    /// ��ũƮ�� �����ϱ�.
    /// </summary>
    public void SetTechTree(TechTreeBase current, byte nodeNum)
    {
        _techTree = current;
        _nodeNum = nodeNum;
    }



    /* ==================== Private Methods ==================== */
}
