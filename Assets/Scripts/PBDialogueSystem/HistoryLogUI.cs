using DG.Tweening;
using UnityEngine;

namespace PBDialogueSystem
{
    /// <summary>
    ///     历史对话记录UI
    /// </summary>
    public class HistoryLogUI : MonoBehaviour
    {
        [Header("历史对话记录UI画布组件")]
        [SerializeField]
        public CanvasGroup HistoryLogCanvasGroup;
        [Header("历史对话记录UI内容节点")]
        [SerializeField]
        public Transform HistoryLogContent;
        [Header("历史对话记录UI项预制体")]
        [SerializeField]
        public GameObject HistoryLogItemPrefab;

        [Header("历史对话记录UI淡入淡出时间")]
        [SerializeField]
        public float FadeDuration = 0.5f;

        public DialogueController DialogueController;

        private RectTransform _historyLogContentRectTrans;
        public bool HasHistoryLogPanel
        {
            get => DialogueController.HasHistoryLogPanel;
            set => DialogueController.HasHistoryLogPanel = value;
        }

        private void OnEnable()
        {
            HideHistoryLog();
            _historyLogContentRectTrans = HistoryLogContent.GetComponent<RectTransform>();
            DialogueController = FindObjectOfType<DialogueController>(true);
            Conversation.OnDialogueBegin += AddHistoryLogItem;
        }

        private void OnDisable()
        {
            Conversation.OnDialogueBegin -= AddHistoryLogItem;
        }

        private void Update()
        {
            if (DOTween.IsTweening(this))
            {
                return;
            }

            if (HasHistoryLogPanel)
            {
                Vector2 anchoredPosition = _historyLogContentRectTrans.anchoredPosition;
                if (Input.GetMouseButtonDown(0)                // 鼠标左键
                 || Input.GetKeyDown(KeyCode.Escape)           // ESC键
                 || Mathf.Approximately(anchoredPosition.y, 0) // 页面滚动到最底部
                   )
                {
                    HideHistoryLog();
                }
            }
            else
            {
                float mouseScrollDelta = Input.mouseScrollDelta.y;
                if (mouseScrollDelta > 0)
                {
                    ShowHistoryLog();
                }
            }
        }

        public void SwitchHistoryLogAction()
        {
            if (DOTween.IsTweening(this))
            {
                return;
            }

            if (HasHistoryLogPanel)
            {
                HideHistoryLog();
            }
            else
            {
                ShowHistoryLog();
            }
        }

        public void AddHistoryLogItem(DialogueData data)
        {
            if (data == null)
            {
                return;
            }

            // 不知道为什么 这个需要过滤掉
            if (data.DialogueText.StartsWith("sedrfsa"))
            {
                return;
            }

            GameObject go = Instantiate(HistoryLogItemPrefab, HistoryLogContent);
            if (go && go.TryGetComponent(out HistoryLogItemUI historyLogItemUI))
            {
                historyLogItemUI.HistoryLogUI = this;
                historyLogItemUI.Bind(data);
            }
        }

        public void ShowHistoryLog()
        {
            if (DOTween.IsTweening(this))
            {
                DOTween.Kill(this);
            }

            Vector2 anchoredPosition = _historyLogContentRectTrans.anchoredPosition;
            anchoredPosition.y = -10;
            _historyLogContentRectTrans.anchoredPosition = anchoredPosition;
            HasHistoryLogPanel = true;
            HistoryLogCanvasGroup.interactable = true;
            HistoryLogCanvasGroup.blocksRaycasts = true;
            HistoryLogCanvasGroup.DOFade(1, FadeDuration)
                                 .SetEase(Ease.Linear)
                                 .SetId(this);
        }

        public void HideHistoryLog()
        {
            if (DOTween.IsTweening(this))
            {
                DOTween.Kill(this);
            }

            HistoryLogCanvasGroup.DOFade(0, FadeDuration)
                                 .OnComplete(() =>
                                  {
                                      HasHistoryLogPanel = false;
                                      HistoryLogCanvasGroup.interactable = false;
                                      HistoryLogCanvasGroup.blocksRaycasts = false;
                                  })
                                 .SetEase(Ease.Linear)
                                 .SetId(this);
        }
    }
}