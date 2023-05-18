using UnityEngine;

public class TechTreeNode : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private TechTreeViewBase _techTree = null;
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
    public void SetTechTree(TechTreeViewBase current, byte nodeNum)
    {
        _techTree = current;
        _nodeNum = nodeNum;
    }



    /* ==================== Private Methods ==================== */
}
