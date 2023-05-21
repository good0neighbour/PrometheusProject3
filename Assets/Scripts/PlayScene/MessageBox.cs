using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class MessageBox : MonoBehaviour
{
    /* ==================== Variables ==================== */

    [SerializeField] private float _messageGap = 2.0f;
    [SerializeField] private GameObject _messageBoxObject = null;
    [SerializeField]  private TMP_Text _messageText = null;

    private Queue<string> _messageQueue = new Queue<string>();
    private float _timer = 0.0f;

    public static MessageBox Instance
    {
        get;
        private set;
    }



    /* ==================== Public Methods ==================== */

    /// <summary>
    /// 메세지 큐에 추가
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
        // 소리 재생
        AudioManager.Instance.PlayAuido(AudioType.Touch);

        // 창 닫는다.
        _messageBoxObject.SetActive(false);
    }



    /* ==================== Private Methods ==================== */

    /// <summary>
    /// 날짜 추가
    /// </summary>
    private string AddDate(string message)
    {
        return $"{PlayManager.Instance[VariableUshort.Year].ToString()}{Language.Instance["년"]} {PlayManager.Instance[VariableByte.Month].ToString()}{Language.Instance["월"]}\n{message}";
    }


    /// <summary>
    /// 단어 대체 후 날짜 추가
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
        // 유니티 식 싱글턴패턴
        Instance = this;

        // 처음 타이머는 미리 맞춰놓는다.
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
                // 소리 재생
                AudioManager.Instance.PlayAuido(AudioType.Alert);

                // 메세지 표시
                _messageText.text = $"{PlayManager.Instance[VariableUshort.Year].ToString()}{Language.Instance["년"]} {PlayManager.Instance[VariableByte.Month].ToString()}{Language.Instance["월"]}\n{_messageQueue.Dequeue()}";
                _messageBoxObject.SetActive(true);
                _timer = 0.0f;
                return;
        }
    }
}
