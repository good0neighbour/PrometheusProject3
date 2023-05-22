using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class MessageBox : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private float _messageGap = 2.0f;
    [SerializeField] private GameObject _messageBoxObject = null;
    [SerializeField] private GameObject _logObject = null;
    [SerializeField] private TMP_Text _messageText = null;
    [SerializeField] private Transform _logArea = null;

    private Queue<string> _messageQueue = new Queue<string>();
    private float _timer = 0.0f;

    public static MessageBox Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// �޼��� ť�� �߰�
    /// </summary>
    public void EnqueueMessage(string message, params string[] replacements)
    {
        if (0 < replacements.Length)
        {
            _messageQueue.Enqueue(AddDate(message, replacements));
        }
        else
        {
            _messageQueue.Enqueue(AddDate(message));
        }
    }


    public void BtnClose()
    {
        // �Ҹ� ���
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // â �ݴ´�.
        _messageBoxObject.SetActive(false);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// ��¥ �߰�
    /// </summary>
    private string AddDate(string message)
    {
        return $"{PlayManager.Instance[VariableUshort.Year].ToString()}{Language.Instance["��"]} {PlayManager.Instance[VariableByte.Month].ToString()}{Language.Instance["��"]}\n{message}";
    }


    /// <summary>
    /// �ܾ� ��ü �� ��¥ �߰�
    /// </summary>
    private string AddDate(string message, params string[] replacements)
    {
        StringBuilder sb = new StringBuilder();
        byte replaceIndex = 0;
        bool record = true;
        for (ushort i = 0; i < message.Length; ++i)
        {
            switch (message[i])
            {
                case '{':
                    sb.Append(replacements[replaceIndex]);
                    record = false;
                    break;
                case '}':
                    ++replaceIndex;
                    record = true;
                    break;
                default:
                    if (record)
                    {
                        sb.Append(message[i]);
                    }
                    break;
            }
        }

        return AddDate(sb.ToString());
    }


    private void Awake()
    {
        // ����Ƽ �� �̱�������
        Instance = this;

        // ó�� Ÿ�̸Ӵ� �̸� ������´�.
        _timer = _messageGap;
    }


    private void Update()
    {
        if (_messageGap > _timer)
        {
            _timer += Time.deltaTime;
            return;
        }

        switch (_messageQueue.Count)
        {
            case 0:
                return;
            default:
                // �Ҹ� ���
                AudioManager.Instance.PlayAuido(AudioType.Alert);

                // �޼��� ǥ��
                _messageText.text = _messageQueue.Peek();
                _messageBoxObject.SetActive(true);

                // �α� �߰�
                TMP_Text newLog = Instantiate(_logObject, _logArea).GetComponent<TMP_Text>();
                newLog.font = Language.Instance.GetFontAsset();
                newLog.text = _messageQueue.Dequeue();
                _timer = 0.0f;
                return;
        }
    }
}
