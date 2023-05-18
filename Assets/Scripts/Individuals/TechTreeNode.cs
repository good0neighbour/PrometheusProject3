using UnityEngine;

public class TechTreeNode : MonoBehaviour
{
    /* ==================== Variables ==================== */

    private TechTreeViewBase _techTree = null;
    private byte _nodeNum = 0;



    /* ==================== Public Methods ==================== */

    public void BtnNodeTouch()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 현재 노드 설정
        _techTree.SetCurrentNode(_nodeNum, transform.position);
    }


    /// <summary>
    /// 테크트리 참조하기.
    /// </summary>
    public void SetTechTree(TechTreeViewBase current, byte nodeNum)
    {
        _techTree = current;
        _nodeNum = nodeNum;
    }



    /* ==================== Private Methods ==================== */
}
