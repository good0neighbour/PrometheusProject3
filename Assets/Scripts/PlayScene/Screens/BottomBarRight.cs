using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BottomBarRight : MonoBehaviour
{
    public enum Displays
    {
        Fund,
        Research,
        Culture,
        Iron,
        Nuke,
        Jewel,
        FacilitySupport,
        ResearchSupport,
        SocietySupport,
        DiplomacySupport
    }



    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _date = null;
    [SerializeField] private TMP_Text _fundText = null;
    [SerializeField] private TMP_Text _fundNum = null;
    [SerializeField] private TMP_Text _researchText = null;
    [SerializeField] private TMP_Text _researchNum = null;
    [SerializeField] private TMP_Text _cultureText = null;
    [SerializeField] private TMP_Text _cultureNum = null;
    [SerializeField] private TMP_Text _ironText = null;
    [SerializeField] private TMP_Text _ironNum = null;
    [SerializeField] private TMP_Text _nukeText = null;
    [SerializeField] private TMP_Text _nukeNum = null;
    [SerializeField] private TMP_Text _jewelText = null;
    [SerializeField] private TMP_Text _jewelNum = null;
    [SerializeField] private TMP_Text _facilitySupportText = null;
    [SerializeField] private TMP_Text _facilitySupportNum = null;
    [SerializeField] private TMP_Text _researchSupportText = null;
    [SerializeField] private TMP_Text _researchSupportNum = null;
    [SerializeField] private TMP_Text _societySupportText = null;
    [SerializeField] private TMP_Text _societySupportNum = null;
    [SerializeField] private TMP_Text _diplomacySupportText = null;
    [SerializeField] private TMP_Text _diplomacySupportNum = null;

    private List<TMP_Text> _spendAnimationTargets = new List<TMP_Text>();
    private float _timer = 0.0f;
    private bool _spendAnimationProceed = false;

    public static BottomBarRight Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 지출 애니메이션 시작
    /// </summary>
    public void SpendAnimation(Displays target)
    {
        switch (target)
        {
            case Displays.Fund:
                _spendAnimationTargets.Add(_fundText);
                _spendAnimationTargets.Add(_fundNum);
                _fundNum.text = UIString.Instance[VariableLong.Funds];
                break;
            case Displays.Research:
                _spendAnimationTargets.Add(_researchText);
                _spendAnimationTargets.Add(_researchNum);
                _researchNum.text = UIString.Instance[VariableUint.Research];
                break;
            case Displays.Culture:
                _spendAnimationTargets.Add(_cultureText);
                _spendAnimationTargets.Add(_cultureNum);
                _cultureNum.text = UIString.Instance[VariableUint.Culture];
                break;
            case Displays.Iron:
                _spendAnimationTargets.Add(_ironText);
                _spendAnimationTargets.Add(_ironNum);
                _ironNum.text = UIString.Instance[VariableUshort.CurrentIron];
                break;
            case Displays.Nuke:
                _spendAnimationTargets.Add(_nukeText);
                _spendAnimationTargets.Add(_nukeNum);
                _nukeNum.text = UIString.Instance[VariableUshort.CurrentNuke];
                break;
            case Displays.Jewel:
                _spendAnimationTargets.Add(_jewelText);
                _spendAnimationTargets.Add(_jewelNum);
                _jewelNum.text = UIString.Instance[VariableUshort.CurrentJewel];
                break;
            case Displays.FacilitySupport:
                _spendAnimationTargets.Add(_facilitySupportText);
                _spendAnimationTargets.Add(_facilitySupportNum);
                break;
            case Displays.ResearchSupport:
                _spendAnimationTargets.Add(_researchSupportText);
                _spendAnimationTargets.Add(_researchSupportNum);
                break;
            case Displays.SocietySupport:
                _spendAnimationTargets.Add(_societySupportText);
                _spendAnimationTargets.Add(_societySupportNum);
                break;
            case Displays.DiplomacySupport:
                _spendAnimationTargets.Add(_diplomacySupportText);
                _spendAnimationTargets.Add(_diplomacySupportNum);
                break;
        }

        // 비용 애니메이션 시작
        _spendAnimationProceed = true;
        _timer = 0.0f;
        foreach (TMP_Text text in _spendAnimationTargets)
        {
            text.fontStyle = FontStyles.Bold;
            text.color = Constants.WHITE;
        }
    }



    /* ==================== Private Methods ==================== */

    private void OnMonthChange()
    {
        // 비활성화일 때 사용 불가
        if (!gameObject.activeSelf)
        {
            return;
        }

        // 날짜
        _date.text = UIString.Instance.GetDateString();
    }


    private void OnYearChange()
    {
        // 비활성화일 때 사용 불가
        if (!gameObject.activeSelf)
        {
            return;
        }

        // 표시
        _fundNum.text = UIString.Instance[VariableLong.Funds];
        _researchNum.text = UIString.Instance[VariableUint.Research];
        _cultureNum.text = UIString.Instance[VariableUint.Culture];
    }


    private void Awake()
    {
        // 유니티식 싱글턴패턴
        Instance = this;

        // 대리자 등록
        PlayManager.OnMonthChange += OnMonthChange;
        PlayManager.OnYearChange += OnYearChange;
    }


    private void OnEnable()
    {
        OnMonthChange();
        OnYearChange();
        _researchNum.text = UIString.Instance[VariableUint.Research];
        _cultureNum.text = UIString.Instance[VariableUint.Culture];
        _ironNum.text = UIString.Instance[VariableUshort.CurrentIron];
        _nukeNum.text = UIString.Instance[VariableUshort.CurrentNuke];
        _jewelNum.text = UIString.Instance[VariableUshort.CurrentJewel];
    }


    private void Update()
    {
        // 지출 애니메이션
        if (_spendAnimationProceed)
        {
            _timer += Time.deltaTime;
            if (Constants.SPEND_ANIMATION_DURATION <= _timer)
            {
                // 지출 애니메이션 끝
                _spendAnimationProceed = false;
                foreach (TMP_Text text in _spendAnimationTargets)
                {
                    text.fontStyle = FontStyles.Normal;
                    text.color = Constants.BOTTOM_TEXT_DEFAULT;
                }
                _spendAnimationTargets.Clear();
            }
        }

        // 지지율 변화
        _facilitySupportNum.text = UIString.Instance[VariableFloat.FacilitySupportRate];
        _researchSupportNum.text = UIString.Instance[VariableFloat.ResearchSupportRate];
        _societySupportNum.text = UIString.Instance[VariableFloat.SocietySupportRate];
        _diplomacySupportNum.text = UIString.Instance[VariableFloat.DiplomacySupportRate];
    }
}
