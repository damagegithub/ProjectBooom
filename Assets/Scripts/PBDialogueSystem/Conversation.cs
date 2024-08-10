using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PBDialogueSystem
{
    public enum ConversationState
    {
        Typing,
        WaitingForNext,
        Finished
    }

    //一段对话
    public class Conversation
    {
        public DialogueController DialogueController;
        public int ConversationID; //对话的ID
        private int _currentDialogueID = -1; //对话中句子的ID
        public ConversationData ConversationData;
        public List<DialogueActor> Actors = new List<DialogueActor>();
        public ConversationState State;
        public DialogueStandardUI DialogueUI;
        public PBTypeWritter TypeWriter = null;
        public Image SpeakerAvatar;
        private Dictionary<int, Texture2D> SpeakerAvatars = new Dictionary<int, Texture2D>();
        private Dictionary<int, Texture2D> FullBodyImages = new Dictionary<int, Texture2D>();

        public Conversation(int newConversationID, PBTypeWritter InTypeWriter)
        {
            ConversationID = newConversationID;
            TypeWriter = InTypeWriter;
            _currentDialogueID = 0;
            TypeWriter.conversation = this;
        }


        public void NextDialogue()
        {
            if (DialogueUI.GameObject().activeSelf == false)
            {
                DialogueUI.GameObject().SetActive(true);
            }

            _currentDialogueID++;
            TypeWriter.GameObject().SetActive(false);
            DialogueData data = GetDialogueDataByID(_currentDialogueID);
            if (data == null)
            {
                State = ConversationState.Finished;
                DialogueController.OnConversationEnd(ConversationID);
                DialogueUI.GameObject().SetActive(false);
                return;
            }

            DialogueActor actor = DialogueController.GetDialogueActor(data.SpeakerID);
            UpdateAvatar(data, actor);
            UpdateFullBodyImage(data);

            //设置对话内容
            TypeWriter.fullText = data.DialogueText;
            TypeWriter.GameObject().SetActive(true);
            State = ConversationState.Typing;
            TypeWriter.StartTyping();
        }

        private void UpdateFullBodyImage(DialogueData data)
        {
            if (data.FullBodySlot1 > 0)
            {
                DialogueActor actor1 = DialogueController.GetDialogueActor(data.FullBodySlot1);
                if (!FullBodyImages.ContainsKey(data.FullBodySlot1))
                {
                    //使用代码根据路径加载图片
                    FullBodyImages.Add(data.FullBodySlot1,
                        Resources.Load<Texture2D>(actor1?.ActorAvatarImagePath));
                }

                DialogueUI.FullBodyImage1.sprite = Sprite.Create(FullBodyImages[data.FullBodySlot1],
                    new Rect(0, 0, FullBodyImages[data.FullBodySlot1].width, FullBodyImages[data.FullBodySlot1].height),
                    new Vector2(0.5f, 0.5f));
                DialogueUI.FullBodyImage1.SetNativeSize();
                DialogueUI.FullBodyImage1.gameObject.transform.localPosition =
                    new Vector3(data.FullBodySlot1_X, data.FullBodySlot1_Y, 0);
                DialogueUI.FullBodyImage1.gameObject.SetActive(true);
            }
            else
            {
                DialogueUI.FullBodyImage1.gameObject.SetActive(false);
            }

            if (data.FullBodySlot2 > 0)
            {
                DialogueActor actor1 = DialogueController.GetDialogueActor(data.FullBodySlot2);
                if (!FullBodyImages.ContainsKey(data.FullBodySlot2))
                {
                    //使用代码根据路径加载图片
                    FullBodyImages.Add(data.FullBodySlot2,
                        Resources.Load<Texture2D>(actor1?.ActorAvatarImagePath));
                }

                DialogueUI.FullBodyImage2.sprite = Sprite.Create(FullBodyImages[data.FullBodySlot2],
                    new Rect(0, 0, FullBodyImages[data.FullBodySlot2].width, FullBodyImages[data.FullBodySlot2].height),
                    new Vector2(0.5f, 0.5f));
                DialogueUI.FullBodyImage2.SetNativeSize();
                DialogueUI.FullBodyImage2.gameObject.transform.localPosition =
                    new Vector3(data.FullBodySlot2_X, data.FullBodySlot2_Y, 0);
                DialogueUI.FullBodyImage2.gameObject.SetActive(true);
            }
            else
            {
                DialogueUI.FullBodyImage2.gameObject.SetActive(false);
            }


            if (data.FullBodySlot3 > 0)
            {
                DialogueActor actor1 = DialogueController.GetDialogueActor(data.FullBodySlot3);
                if (!FullBodyImages.ContainsKey(data.FullBodySlot3))
                {
                    //使用代码根据路径加载图片
                    FullBodyImages.Add(data.FullBodySlot3,
                        Resources.Load<Texture2D>(actor1?.ActorAvatarImagePath));
                }

                DialogueUI.FullBodyImage3.sprite = Sprite.Create(FullBodyImages[data.FullBodySlot3],
                    new Rect(0, 0, FullBodyImages[data.FullBodySlot3].width, FullBodyImages[data.FullBodySlot3].height),
                    new Vector2(0.5f, 0.5f));
                DialogueUI.FullBodyImage3.SetNativeSize();
                DialogueUI.FullBodyImage3.gameObject.transform.localPosition =
                    new Vector3(data.FullBodySlot3_X, data.FullBodySlot3_Y, 0);
                DialogueUI.FullBodyImage3.gameObject.SetActive(true);
            }
            else
            {
                DialogueUI.FullBodyImage3.gameObject.SetActive(false);
            }
        }

        private void UpdateAvatar(DialogueData data, DialogueActor actor)
        {
            //设置对话者头像

            DialogueUI.SpeakerNameTextMesh.text = actor?.ActorName;
            if (!SpeakerAvatars.ContainsKey(data.SpeakerID))
            {
                //使用代码根据路径加载图片
                SpeakerAvatars.Add(data.SpeakerID,
                    Resources.Load<Texture2D>(actor?.ActorAvatarImagePath));
            }

            SpeakerAvatar.sprite = SpeakerAvatars[data.SpeakerID]==null ? null : Sprite.Create(SpeakerAvatars[data.SpeakerID],
                new Rect(0, 0, SpeakerAvatars[data.SpeakerID].width, SpeakerAvatars[data.SpeakerID].height),
                new Vector2(0.5f, 0.5f));
            SpeakerAvatar.SetNativeSize();
            SpeakerAvatar.gameObject.transform.localPosition = new Vector3(data.AvatarPosX, data.AvatarPosY, 0);
            SpeakerAvatar.gameObject.SetActive(data.bShowAvatar);
            
        }

        private DialogueData GetDialogueDataByID(int dialogueID)
        {
            foreach (DialogueData data in ConversationData.DialogueDatas)
            {
                if (data.DialogueID == dialogueID)
                {
                    return data;
                }
            }

            return null;
        }
    }
}