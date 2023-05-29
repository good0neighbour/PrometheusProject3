using UnityEngine;

public class CoolTimeBtnPopulation : CoolTimeBtnMediaCultureSemiBase
{
    [SerializeField] bool _increase = false;

    protected override void OnAdopt()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Select);

        if (_increase)
        {
            PlayManager.Instance[VariableFloat.PopulationAdjustment] *= Constants.POPULATION_ADJUSTMENT_INCREASE;
        }
        else
        {
            PlayManager.Instance[VariableFloat.PopulationAdjustment] *= Constants.POPULATION_ADJUSTMENT_DECREASE;
        }
    }
}
