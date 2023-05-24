using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BottomBarLeft : MonoBehaviour
{
    public enum Displays
    {
        Fund,
        Research,
        Culture,
        Iron,
        Nuke,
        Jewel,
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

    private List<TMP_Text> _spendAnimationTargets = new List<TMP_Text>();
    private float _timer = 0.0f;
    private bool _spendAnimationProceed = false;

    public static BottomBarLeft Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ���� �ִϸ��̼� ����
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
        }

        // ��� �ִϸ��̼� ����
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
        // ��Ȱ��ȭ�� �� ��� �Ұ�
        if (!gameObject.activeSelf)
        {
            return;
        }

        // ��¥
        _date.text = UIString.Instance.GetDateString();
    }


    private void OnYearChange()
    {
        // ��Ȱ��ȭ�� �� ��� �Ұ�
        if (!gameObject.activeSelf)
        {
            return;
        }

        // ǥ��
        _fundNum.text = UIString.Instance[VariableLong.Funds];
        _researchNum.text = UIString.Instance[VariableUint.Research];
        _cultureNum.text = UIString.Instance[VariableUint.Culture];
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // �븮�� ���
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
        // ���� �ִϸ��̼�
        if (_spendAnimationProceed)
        {
            _timer += Time.deltaTime;
            if (Constants.SPEND_ANIMATION_DURATION <= _timer)
            {
                // ���� �ִϸ��̼� ��
                _spendAnimationProceed = false;
                foreach (TMP_Text text in _spendAnimationTargets)
                {
                    text.fontStyle = FontStyles.Normal;
                    text.color = Constants.BOTTOM_TEXT_DEFAULT;
                }
                _spendAnimationTargets.Clear();
            }
        }
    }
}
