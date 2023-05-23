using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenDiplomacy : PlayScreenBase
{
    /* ==================== Variables ==================== */

    [SerializeField] private TMP_Text _forceNameText = null;
    [SerializeField] private Image _friendlyImage = null;
    [SerializeField] private Image _hostileImage = null;
    [SerializeField] private Image _conquestImage = null;
    [SerializeField] private GameObject _true = null;
    [SerializeField] private GameObject _false = null;

    private byte _current = 0;

    public Force CurrentForce
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    public void BtnForceSelect(int index)
    {
        // �̹� ���� ��ư�� ������.
        if (_current == index)
        {
            return;
        }

        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // ���� ����
        _current = (byte)index;
        CurrentForce = PlayManager.Instance.GetForce(_current);
        _forceNameText.text = CurrentForce.ForceName;

        // ǥ���� ȭ��
        bool isTrue = CurrentForce.Info;
        _true.SetActive(isTrue);
        _false.SetActive(!isTrue);
    }


    public void BtnTrue()
    {
        CurrentForce.Info = true;
        _true.SetActive(true);
        _false.SetActive(false);
    }


    public void BtnDiplomacy()
    {

    }


    public void BtnTrade()
    {

    }


    public void BtnConquest()
    {

    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        CurrentForce = PlayManager.Instance.GetForce(_current);
    }


    private void Update()
    {
        _friendlyImage.fillAmount = CurrentForce.Friendly;
        _hostileImage.fillAmount = CurrentForce.Hostile;
        _conquestImage.fillAmount = CurrentForce.Conquest;

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
