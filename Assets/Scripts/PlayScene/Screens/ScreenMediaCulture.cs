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

    public static ScreenMediaCulture Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void OnAdoptAnimation(byte index)
    {
        switch (index)
        {
            case 0:
                // �ڻ�ȭ �ִϸ��̼�
                IncreaseAnimation(_assetImage, PlayManager.Instance[VariableFloat.GovAsset]);
                return;
            case 1:
                // ����� �ִϸ��̼�
                IncreaseAnimation(_affectionImage, PlayManager.Instance[VariableFloat.GovAffection]);
                return;
            default:
                Debug.LogError("�߸��� �ε��� - ScreenMediaCulture");
                return;
        }
    }



    /* ==================== Private Methods ==================== */


    private void IncreaseAnimation(Image target, float goal)
    {
        _target = target;
        _goal = goal;
        _animationAmount = _goal - _target.fillAmount;
        _animationProceed = true;
    }


    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // �ʱ� ����
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
