﻿using System;
using System.Collections.Generic;
using DG.Tweening;
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
        public  DialogueController         DialogueController;
        public  int                        ConversationID;          //对话的ID
        private int                        _currentDialogueID = -1; //对话中句子的ID
        public  ConversationData           ConversationData;
        public  ConversationState          State;
        public  DialogueStandardUI         DialogueUI;
        public  PBTypeWritter              TypeWriter = null;
        public  Image                      SpeakerAvatar;
        private Dictionary<int, Texture2D> SpeakerAvatars    = new Dictionary<int, Texture2D>();
        private Dictionary<int, Texture2D> SpeakerNameImages = new Dictionary<int, Texture2D>();
        private Dictionary<int, Texture2D> FullBodyImages    = new Dictionary<int, Texture2D>();
        private Dictionary<int, AudioClip> AudioClips        = new Dictionary<int, AudioClip>();

        public bool _isBlackBG = false;

        public static Action<DialogueData> OnDialogueBegin;

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
                DialogueUI.GetComponentInChildren<CanvasGroup>().DOFade(1, 1);
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
            OnDialogueBegin?.Invoke(data); //对话开始事件

            DialogueActor actor = DialogueController.GetDialogueActor(data.SpeakerID);
            UpdateAvatar(data, actor);
            UpdateFullBodyImage(data);

            if (data.BGMID < 0)
            {
                DialogueUI.GetComponent<AudioSource>().Stop();
                DialogueController.audioController.FadefInBGM();
            }
            else if (data.BGMID > 0)
            {
                if (!AudioClips.ContainsKey(data.BGMID))
                {
                    //使用代码根据路径加载图片
                    AudioClips.Add(data.BGMID,
                                   Resources.Load<AudioClip>(DialogueController.GetDialogueAudioPath(data.BGMID)));
                }

                DialogueUI.GetComponent<AudioSource>().clip
                    = AudioClips[data.BGMID] == null ? null : AudioClips[data.BGMID];
                DialogueUI.GetComponent<AudioSource>().Play();
                DialogueController.audioController.FadefOutBGM();
            }


            if (ConversationID == 1103 && _currentDialogueID == 19)
            {
                DialogueUI.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                DialogueUI.gameObject.GetComponent<Image>().DOFade(1, 1);
                DialogueUI.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Texture/GameCG");
            }
            else if ((ConversationID == 1103 && _currentDialogueID < 19) || ConversationID != 1103)
            {
                if ((data.BlackBG == 1) && !_isBlackBG) //当前是0,现在要是1
                {
                    if (_currentDialogueID == 1)
                    {
                        DialogueUI.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 1);
                    }
                    else
                    {
                        DialogueUI.gameObject.GetComponent<Image>().DOFade(1, 1);
                    }

                    _isBlackBG = true;
                }
                else if ((data.BlackBG == 0) && _isBlackBG)
                {
                    DialogueUI.gameObject.GetComponent<Image>().DOFade(0, 1);
                    _isBlackBG = false;
                }
            }

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
                    var Texture2D = Resources.Load<Texture2D>(actor1?.ActorFullBodyImagePath);
                    //使用代码根据路径加载图片
                    if (Texture2D != null)
                    {
                        FullBodyImages.Add(data.FullBodySlot1, Texture2D);
                    }
                }

                if (FullBodyImages.ContainsKey(data.FullBodySlot1))
                {
                    DialogueUI.FullBodyImage1.sprite = Sprite.Create(FullBodyImages[data.FullBodySlot1],
                                                                     new Rect(
                                                                         0, 0, FullBodyImages[data.FullBodySlot1].width,
                                                                         FullBodyImages[data.FullBodySlot1].height),
                                                                     new Vector2(0.5f, 0.5f));
                    DialogueUI.FullBodyImage1.SetNativeSize();
                    DialogueUI.FullBodyImage1.gameObject.SetActive(true);
                }
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
                    var Texture2D = Resources.Load<Texture2D>(actor1?.ActorFullBodyImagePath);
                    //使用代码根据路径加载图片
                    if (Texture2D != null)
                    {
                        FullBodyImages.Add(data.FullBodySlot2, Texture2D);
                    }
                }

                if (FullBodyImages.ContainsKey(data.FullBodySlot2))
                {
                    DialogueUI.FullBodyImage2.sprite = Sprite.Create(FullBodyImages[data.FullBodySlot2],
                                                                     new Rect(
                                                                         0, 0, FullBodyImages[data.FullBodySlot2].width,
                                                                         FullBodyImages[data.FullBodySlot2].height),
                                                                     new Vector2(0.5f, 0.5f));
                    DialogueUI.FullBodyImage2.SetNativeSize();
                    DialogueUI.FullBodyImage2.gameObject.SetActive(true);
                }
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
                    var Texture2D = Resources.Load<Texture2D>(actor1?.ActorFullBodyImagePath);
                    //使用代码根据路径加载图片
                    if (Texture2D != null)
                    {
                        FullBodyImages.Add(data.FullBodySlot3, Texture2D);
                    }
                }

                if (FullBodyImages.ContainsKey(data.FullBodySlot3))
                {
                    var sprite = Sprite.Create(FullBodyImages[data.FullBodySlot3],
                                               new Rect(0, 0, FullBodyImages[data.FullBodySlot3].width,
                                                        FullBodyImages[data.FullBodySlot3].height),
                                               new Vector2(0.5f, 0.5f));
                    if (sprite != null)
                    {
                        DialogueUI.FullBodyImage3.sprite = sprite;
                        DialogueUI.FullBodyImage3.SetNativeSize();
                        DialogueUI.FullBodyImage3.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                DialogueUI.FullBodyImage3.gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///     尝试获取对话者的名字图片
        /// </summary>
        public bool TryGetNameImage(int speakerID, out Texture2D texture2D)
        {
            if (SpeakerNameImages.TryGetValue(speakerID, out texture2D))
            {
                return true;
            }

            DialogueActor actor = DialogueController.GetDialogueActor(speakerID);
            if (actor == null)
            {
                return false;
            }

            texture2D = Resources.Load<Texture2D>(actor.NameImg);
            if (!texture2D)
            {
                return false;
            }

            SpeakerNameImages.Add(speakerID, texture2D);
            return true;
        }

        private void UpdateAvatar(DialogueData data, DialogueActor actor)
        {
            //设置对话者头像

            if (!SpeakerNameImages.ContainsKey(data.SpeakerID))
            {
                //使用代码根据路径加载图片
                SpeakerNameImages.Add(data.SpeakerID,
                                      Resources.Load<Texture2D>(actor?.NameImg));
            }

            DialogueUI.SpeakerNameImg.sprite = SpeakerNameImages[data.SpeakerID] == null
                ? null
                : Sprite.Create(SpeakerNameImages[data.SpeakerID],
                                new Rect(0, 0, SpeakerNameImages[data.SpeakerID].width,
                                         SpeakerNameImages[data.SpeakerID].height),
                                new Vector2(0.5f, 0.5f));
            DialogueUI.SpeakerNameImg.SetNativeSize();
            DialogueUI.SpeakerNameImg.gameObject.SetActive(SpeakerNameImages[data.SpeakerID] != null);

            if (!SpeakerAvatars.ContainsKey(data.SpeakerID))
            {
                //使用代码根据路径加载图片
                SpeakerAvatars.Add(data.SpeakerID,
                                   Resources.Load<Texture2D>(actor?.ActorAvatarImagePath));
            }

            SpeakerAvatar.sprite = SpeakerAvatars[data.SpeakerID] == null
                ? null
                : Sprite.Create(SpeakerAvatars[data.SpeakerID],
                                new Rect(0, 0, SpeakerAvatars[data.SpeakerID].width,
                                         SpeakerAvatars[data.SpeakerID].height),
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