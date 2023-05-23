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
                // 자산화
                if (1.0f > PlayManager.Instance[VariableFloat.GovAsset])
                {
                    PlayManager.Instance[VariableFloat.GovAsset] += (1.0f - PlayManager.Instance[VariableFloat.GovAsset]) * Constants.GOV_ASSET_MULTIPLY;
                    IncreaseAnimation(_assetImage, PlayManager.Instance[VariableFloat.GovAsset]);
                }
                return;
            case 1:
                // 영향력
                if (PlayManager.Instance[VariableFloat.GovAsset] > PlayManager.Instance[VariableFloat.GovAffection])
                {
                    PlayManager.Instance[VariableFloat.GovAffection] += (PlayManager.Instance[VariableFloat.GovAsset] - PlayManager.Instance[VariableFloat.GovAffection]) * Constants.GOV_AFFECTION_MULTIPLY;
                    IncreaseAnimation(_affectionImage, PlayManager.Instance[VariableFloat.GovAffection]);
                }
                return;
            case 2:
                // 인구 증가
                return;
            case 3:
                // 인구 감소
                return;
            case 4:
                // 시설 지지율
                SupportRateIncrease(VariableFloat.FacilitySupportRate);
                PlayManager.Instance.FacilitySupportGoal = PlayManager.Instance[VariableFloat.FacilitySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.FacilitySupport);
                return;
            case 5:
                // 연구 지지율
                SupportRateIncrease(VariableFloat.ResearchSupportRate);
                PlayManager.Instance.ResearchSupportGoal = PlayManager.Instance[VariableFloat.ResearchSupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.ResearchSupport);
                return;
            case 6:
                // 사회 지지율
                SupportRateIncrease(VariableFloat.SocietySupportRate);
                PlayManager.Instance.SocietySupportGoal = PlayManager.Instance[VariableFloat.SocietySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.SocietySupport);
                return;
            case 7:
                // 외교 지지율
                SupportRateIncrease(VariableFloat.DiplomacySupportRate);
                PlayManager.Instance.DiplomacySupportGoal = PlayManager.Instance[VariableFloat.DiplomacySupportRate];
                BottomBarRight.Instance.SpendAnimation(BottomBarRight.Displays.DiplomacySupport);
                return;
            default:
                Debug.LogError("잘못된 인덱스 - ScreenMediaCulture");
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

        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex - 1);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
#endif
        // PC, 모바일 공용
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}
