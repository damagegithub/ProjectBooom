using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PBDialogueSystem
{
    /// <summary>
    ///     历史对话记录UI项
    /// </summary>
    public class HistoryLogItemUI : MonoBehaviour
    {
        public Image           ImageName;
        public TextMeshProUGUI TMP_Content;
        public HistoryLogUI    HistoryLogUI;

        public void Bind(DialogueData data)
        {
            string dialogueText = data.DialogueText;
            int speakerID = data.SpeakerID;
            if (!HistoryLogUI)
            {
                Debug.LogWarning($"HistoryLogUI 未赋值");
                return;
            }

            if (!HistoryLogUI.DialogueController)
            {
                Debug.LogWarning($"DialogueController 未赋值");
                return;
            }

            DialogueController dc = HistoryLogUI.DialogueController;
            if (dc && dc._currentConversation != null
                   && dc._currentConversation.TryGetNameImage(speakerID, out Texture2D nameImage))
            {
                Bind(nameImage, dialogueText);
            }
            else
            {
                Debug.LogWarning($"对话者ID {speakerID} 不存在于 " +
                                 $"conversatonID: {data.ConversationID} " +
                                 $"dialogueID: {data.DialogueID}");
            }
        }

        public void Bind(Texture2D textureName, string content)
        {
            if (!textureName)
            {
                ImageName.enabled = false;
                ImageName.gameObject.SetActive(false);
            }
            else
            {
                Rect rect = new(0, 0, textureName.width, textureName.height);
                ImageName.sprite = Sprite.Create(textureName, rect, new Vector2(0.5f, 0.5f));
                ImageName.enabled = true;
                ImageName.gameObject.SetActive(true);
            }

            TMP_Content.SetText(content);
            if (string.IsNullOrWhiteSpace(TMP_Content.text))
            {
                TMP_Content.enabled = false;
                TMP_Content.gameObject.SetActive(false);
            }
            else
            {
                TMP_Content.enabled = true;
                TMP_Content.gameObject.SetActive(true);
            }
        }
    }
}