using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace PBDialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        private Dictionary<int, ConversationData> _conversationData = new Dictionary<int, ConversationData>();
        private Dictionary<int, DialogueActor> _dialogueActors = new Dictionary<int, DialogueActor>();
        private Dictionary<int, DialogueAudio> _dialogueAudios = new Dictionary<int, DialogueAudio>();
        private ConversationData _currentConversationData = null;
        private Conversation _currentConversation = null;

        public GameObject DialogueUIGO;
        public AudioController audioController;
        private DialogueStandardUI DialogueUI;
        
        public event Action<int> OnOneConversationEnd;

        public void ClearOnOneConversationEnd()
        {
            OnOneConversationEnd = null;
        }

        void Start()
        {
            // 先尝试在场景中找到对话UI
            if (!DialogueUI)
            {
                DialogueUI = FindObjectOfType<DialogueStandardUI>(true);
            }

            // 如果场景中没有对话UI，则实例化一个
            if (!DialogueUI)
            {
                GameObject instance = Instantiate(DialogueUIGO, transform.position, Quaternion.identity);
                DialogueUI = instance.GetComponent<DialogueStandardUI>();
            }
            DialogueUI.gameObject.SetActive(false);
            audioController = FindObjectOfType<AudioController>();
            InitConversationData();
            // StartConversation(101);
        }
        
        void Update()
        {
            // 检测空格键输入
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_currentConversation == null) 
                {
                    return;
                }

                if (_currentConversation.State == ConversationState.WaitingForNext)
                {
                    _currentConversation.NextDialogue();
                }
                else if (_currentConversation.State == ConversationState.Typing)
                {
                    _currentConversation.TypeWriter.ShowFullText();
                }
                    
               
            }
        }

        private void InitConversationData()
        {
            List<DialogueData> dialogueJsonData = CSVToJsonUtil.GetJsonData<DialogueData>("Tables/Dialogue");

            foreach (var dialogueData in dialogueJsonData)
            {
                int conversationID = dialogueData.ConversationID;
                if (_conversationData.ContainsKey(conversationID))
                {
                    _conversationData[conversationID].DialogueDatas.Add(dialogueData);
                }
                else
                {
                    ConversationData conversationData = new ConversationData();
                    conversationData.DialogueDatas= new List<DialogueData>();
                    conversationData.DialogueDatas.Add(dialogueData);
                    _conversationData.Add(conversationID, conversationData);
                }
            }

            List<DialogueActor> dialogueActorsJsonData = CSVToJsonUtil.GetJsonData<DialogueActor>("Tables/Actors");
            foreach (var dialogueActor in dialogueActorsJsonData)
            {
                int actorID = dialogueActor.ActorID;
                _dialogueActors[actorID] = dialogueActor;
            }
            List<DialogueAudio> dialogueAudiosJsonData = CSVToJsonUtil.GetJsonData<DialogueAudio>("Tables/Audio");
            foreach (var dialogueAudio in dialogueAudiosJsonData)
            {
                int actorID = dialogueAudio.AudioID;
                _dialogueAudios[actorID] = dialogueAudio;
            }
        }

        public String GetDialogueAudioPath(int AudioID)
        {
            if (!_dialogueAudios.ContainsKey(AudioID))
            {
                Debug.LogError("Actor ID not found in dialogue Audio! " + AudioID);
                return null;
            }

            return _dialogueAudios[AudioID].AudioPath;
        }

        public void StartConversation(int conversationID)
        {
            if (!_conversationData.ContainsKey(conversationID))
            {
                Debug.LogError("Conversation ID not found in conversation data! "+conversationID);
                return;
            }
            var PlayerControllers = GameObject.FindObjectsOfType<PlayerController>();
            foreach (var playerController in PlayerControllers)
            {
                playerController.DisablePlayerControl();
            }

            _currentConversation = null;
            _currentConversation = new Conversation(conversationID,DialogueUI.TextWriter);
            _currentConversation.DialogueUI = DialogueUI;
            _currentConversation.SpeakerAvatar = DialogueUI.SpeakerAvatar;
            _currentConversation.DialogueController = this;
            _currentConversation.ConversationData = _conversationData[conversationID];
            _currentConversation._isBlackBG = false;
            DialogueUI.gameObject.GetComponent<Image>().color=new Color(0,0,0,0);
            _currentConversation.NextDialogue();
        }

        public DialogueActor GetDialogueActor(int actorID)
        {
            if (!_dialogueActors.ContainsKey(actorID))
            {
                Debug.LogError("Actor ID not found in dialogue actors! " + actorID);
                return null;
            }

            return _dialogueActors[actorID];
        }

        public void OnConversationEnd(int conversationID)
        {
            _currentConversation = null;     
            OnOneConversationEnd?.Invoke(conversationID);
            
            var PlayerControllers = GameObject.FindObjectsOfType<PlayerController>();
            foreach (var playerController in PlayerControllers)
            {
                playerController.controlEnabled = true;
            }
        }
    }
}