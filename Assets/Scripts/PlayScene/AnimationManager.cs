using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationManager : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [Header("������")]
    [SerializeField] private float _noiseSpeed = 50.0f;
    [SerializeField] private float _noiseEffectSpeedmult = 2.0f;
    [SerializeField] private float _noiseBrightness = 0.6f;
    [SerializeField] private float _noiseAlpha = 0.6f;

    [Header("�¿� ��ư")]
    [SerializeField] private float _leftRightBtnMinBrightness = 0.5f;

    [Header("����")]
    [SerializeField] private Image _background = null;
    [SerializeField] private TMP_Text[] _leftRightBtns = null;

    private RectTransform _backgroundTransform = null;
    private float _leftRightTimer = 0.0f;
    private float _leftRightAdd = 0.0f;
    private float _leftRightMultiply = 0.0f;

    public static AnimationManager Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// ������ ȿ�� ����
    /// </summary>
    public void NoiseEffect()
    {
        _background.color = new Color(1.0f, 1.0f, 1.0f, _noiseAlpha);
    }



    /* ==================== Private Methods ==================== */

    private void Awake()
    {
        // ����Ƽ�� �̱�������
        Instance = this;

        // ��� rectTransform ����
        _backgroundTransform = _background.rectTransform;

        // �¿� ��ư �ִϸ��̼� ��� ����
        _leftRightMultiply = (1.0f - _leftRightBtnMinBrightness) * 0.5f;
        _leftRightAdd = _leftRightBtnMinBrightness + _leftRightMultiply;
    }


    private void Update()
    {
        // ���� ��������
        float float0;

        #region ������ �ִϸ��̼�
        _backgroundTransform.localPosition += new Vector3(0.0f, _noiseSpeed, 0.0f);
        if (Constants.HALF_CANVAS_HEIGHT < _backgroundTransform.localPosition.y)
        {
            _backgroundTransform.localPosition -= new Vector3(0.0f, Constants.CANVAS_HEIGHT, 0.0f);
        }
        #endregion

        #region ������ ��ο�����
        float0 = _background.color.r + (_noiseBrightness - _background.color.r) * Time.deltaTime * _noiseEffectSpeedmult;
        _background.color = new Color(float0, float0, float0, _noiseAlpha);
        #endregion

        #region �¿� ��ư �ִϸ��̼�
        // ���� ����
        float0 = Mathf.Sin(_leftRightTimer) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[0].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[1].color = new Color(float0, float0, float0, 1.0f);

        float0 = Mathf.Sin(_leftRightTimer + Constants.HALF_PI) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[2].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[3].color = new Color(float0, float0, float0, 1.0f);

        float0 = Mathf.Sin(_leftRightTimer + Constants.PI) * _leftRightMultiply + _leftRightAdd;
        _leftRightBtns[4].color = new Color(float0, float0, float0, 1.0f);
        _leftRightBtns[5].color = new Color(float0, float0, float0, 1.0f);

        // Ÿ�̸�
        _leftRightTimer += Time.deltaTime;
        if (_leftRightTimer > Constants.DOUBLE_PI)
        {
            _leftRightTimer -= Constants.DOUBLE_PI;
        }
        #endregion
    }
}
