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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 테크트리 창 활성화
        _popUpElementTechScreen.ActiveThis(0);
        PlayManager.Instance.GameResume = Constants.GAME_PAUSE;

        // 이 창 닫는다.
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 사회 이미지 업데이트
    /// </summary>
    public void SocietyImageUpdate()
    {
        _societyBtnImage.sprite = _societySprites[PlayManager.Instance[VariableByte.Era] - 1];
        _eraText.text = UIString.Instance.GetEraString();
    }



    /* ==================== Private Methods ==================== */

    private void Update()
    {
        // 지지율 이미지
        _supportRateImage.fillAmount = PlayManager.Instance[VariableFloat.SocietySupportRate] * 0.01f;

        // 단축키 동작
#if PLATFORM_STANDALONE_WIN
        // 키보드 동작
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
        // PC, 모바일 공용
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GeneralMenuButtons.Instance.BtnLeftRight(true);
        }
    }
}