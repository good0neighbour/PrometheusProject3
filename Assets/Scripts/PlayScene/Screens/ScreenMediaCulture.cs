using UnityEngine;
using UnityEngine.UI;

public class ScreenMediaCulture : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private Image _assetImage = null;
    [SerializeField] private Image _affectionImage = null;

    private Image _target = null;
    private float _goal = 0.0f;
    private float _animationAmount = 0.0f;
    private bool _animationProceed = false;



    /* ==================== Public Methods ==================== */

    public void OnAdopt(byte index)
    {
        switch (index)
        {
            case 0:
                // �ڻ�ȭ
                if (1.0f > PlayManager.Instance[VariableFloat.GovAsset])
                {
                    PlayManager.Instance[VariableFloat.GovAsset] += (1.0f - PlayManager.Instance[VariableFloat.GovAsset]) * Constants.GOV_ASSET_MULTIPLY;
                    IncreaseAnimation(_assetImage, PlayManager.Instance[VariableFloat.GovAsset]);
                }
                return;
            case 1:
                // �����
                if (PlayManager.Instance[VariableFloat.GovAsset] > PlayManager.Instance[VariableFloat.GovAffection])
                {
                    PlayManager.Instance[VariableFloat.GovAffection] += (PlayManager.Instance[VariableFloat.GovAsset] - PlayManager.Instance[VariableFloat.GovAffection]) * Constants.GOV_AFFECTION_MULTIPLY;
                    IncreaseAnimation(_affectionImage, PlayManager.Instance[VariableFloat.GovAffection]);
                }
                return;
            case 2:
                // �α� ����
                return;
            case 3:
                // �α� ����
                return;
            case 4:
                // �ü� ������
                SupportRateIncrease(VariableFloat.FacilitySupportRate);
                PlayManager.Instance.FacilitySupportGoal = PlayManager.Instance[VariableFloat.FacilitySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);
                return;
            case 5:
                // ���� ������
                SupportRateIncrease(VariableFloat.ResearchSupportRate);
                PlayManager.Instance.ResearchSupportGoal = PlayManager.Instance[VariableFloat.ResearchSupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);
                return;
            case 6:
                // ��ȸ ������
                SupportRateIncrease(VariableFloat.SocietySupportRate);
                PlayManager.Instance.SocietySupportGoal = PlayManager.Instance[VariableFloat.SocietySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);
                return;
            case 7:
                // �ܱ� ������
                SupportRateIncrease(VariableFloat.DiplomacySupportRate);
                PlayManager.Instance.DiplomacySupportGoal = PlayManager.Instance[VariableFloat.DiplomacySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.DiplomacySupport);
                return;
            default:
                Debug.LogError("�߸��� �ε��� - ScreenMediaCulture");
                return;
        }
    }



    /* ==================== Private Methods ==================== */

    private void SupportRateIncrease(VariableFloat supportRate)
    {
        PlayManager.Instance[supportRate] += Constants.SUPPORT_RATE_INCREASEMENT * PlayManager.Instance[VariableFloat.GovAffection];
        if (100.0f < PlayManager.Instance[supportRate])
        {
            PlayManager.Instance[supportRate] = 100.0f;
        }
    }


    private void IncreaseAnimation(Image target, float goal)
    {
        _target = target;
        _goal = goal;
        _animationAmount = _goal - _target.fillAmount;
        _animationProceed = true;
    }


    private void Awake()
    {
        _assetImage.fillAmount = PlayManager.Instance[VariableFloat.GovAsset];
        _affectionImage.fillAmount = PlayManager.Instance[VariableFloat.GovAffection];
    }


    private void Update()
    {
        if (_animationProceed)
        {
            _target.fillAmount += _animationAmount * Time.deltaTime;
            if (_goal <= _target.fillAmount)
            {
                _target.fillAmount = _goal;
                _animationProceed = false;
            }
        }

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex - 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
#endif
        // PC, ����� ����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}
