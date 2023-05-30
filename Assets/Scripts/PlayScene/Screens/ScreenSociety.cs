using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenSociety : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private PopUpScreenElementTechTree _popUpElementTechScreen = null;
    [SerializeField] private PopUpViewSociety _societyView = null;
    [SerializeField] private TMP_Text _eraText = null;
    [SerializeField] private Image _societyBtnImage = null;
    [SerializeField] private Image _supportRateImage = null;
    [SerializeField] private Sprite[] _societySprites = null;



    /* ==================== Public Methods ==================== */

    public void Activate()
    {
        _societyView.Activate();
        SocietyImageUpdate();
    }


    public void BtnSocietyView()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ��ũƮ�� â Ȱ��ȭ
        _popUpElementTechScreen.ActiveThis(0);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;

        // �� â �ݴ´�.
        gameObject.SetActive(false);
    }


    /// <summary>
    /// ��ȸ �̹��� ������Ʈ
    /// </summary>
    public void SocietyImageUpdate()
    {
        _societyBtnImage.sprite = _societySprites[PlayManager.Instance[VariableByte.Era] - 1];
        _eraText.text = UIString.Instance.GetEraString();
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // ������ �̹���
        _supportRateImage.fillAmount = PlayManager.Instance[VariableFloat.SocietySupportRate] * 0.01f;

        // ����Ű ����
#if PLATFORM_STANDALONE_WIN
        // Ű���� ����
        if (Input.GetKeyUp(KeyCode.W))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex - 1);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            GeneralMenuButtons.Instance.BtnScreenIndex(GeneralMenuButtons.Instance.CurrentRightIndex + 1);
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