using UnityEngine;

public class CoolTimeBtnPopulation : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] bool _increase = false;

    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);
    }
}
