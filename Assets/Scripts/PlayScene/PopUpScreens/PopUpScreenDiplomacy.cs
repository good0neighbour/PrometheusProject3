using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScreenDiplomacy : MonoBehaviour, IPopUpScreen, ICoolTimeScreen
{
    /* ==================== Variables ==================== */

    [SerializeField] private ButtonCoolTime[] _buttons = null;
    [SerializeField] private string[] _buttonDescriptions = null;
    [SerializeField] private TMP_Text _dexcriptionText = null;
    [SerializeField] private TMP_Text _statusText = null;
    [SerializeField] private TMP_Text _backBtnText = null;
    [SerializeField] private Image _progressionImage = null;
    [SerializeField] private GameObject _previousScreen = null;

    private byte _currentBtn = 0;
    private float _supportRate = 0.0f;
    private bool _isAdoptAvailable = false;
    private bool _isBackBtnAvailable = true;
    private bool _adoptAnimationProceed = false;



    /* ==================== Public Methods ==================== */

    public void BtnBack()
    {
        gameObject.SetActive(false);
        _previousScreen.SetActive(true);

        _dexcriptionText.text = null;
    }


    public void BtnAdopt()
    {
        // 사용 불가
        if (!_isAdoptAvailable)
        {
            return;
        }

        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 승인 버튼 사용 불가
        _isAdoptAvailable = false;

        // 뒤로가기 금지
        _isBackBtnAvailable = false;
        _backBtnText.color = Constants.TEXT_BUTTON_DISABLE;

        // 애니메이션 실행
        AdoptAnimation(PlayManager.Instance[VariableFloat.SocietySupportRate]);
    }


    public void BtnTouch(int index)
    {
        _currentBtn = (byte)index;
        _dexcriptionText.text = _buttonDescriptions[_currentBtn];
    }


    public void OnAdopt(byte index)
    {
        switch (index)
        {
            case 0:
                return;
        }
    }



    /* ==================== Private Methods ==================== */

    private void AdoptAnimation(float supportRate)
    {
        _adoptAnimationProceed = true;
        _supportRate = supportRate;
    }
}
