using UnityEngine;

public class CoolTimeBtnPopulation : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] bool _increase = false;

    protected override void OnAdopt()
    {
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Select);
    }
}
