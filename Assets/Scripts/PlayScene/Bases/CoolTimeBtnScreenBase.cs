using UnityEngine;

public abstract class CoolTimeBtnScreenBase : MonoBehaviour
{
    /// <summary>
    /// 쿨타임 버튼 눌렀을 때 동작
    /// </summary>
    public abstract void OnAdopt(byte index);
}
