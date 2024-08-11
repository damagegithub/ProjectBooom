using System;
using System.Collections.Generic;
using UnityEngine;

namespace PBDialogueSystem
{
    public class DialogueController : MonoBehaviour
    {
        private Dictionary<int, ConversationData> _conversationData = new Dictionary<int, ConversationData>();
        private Dictionary<int, DialogueActor> _dialogueActors = new Dictionary<int, DialogueActor>();
        private ConversationData _currentConversationData = null;
        private Conversation _currentConversation = null;

        public GameObject DialogueUIGO;
        private DialogueStandardUI DialogueUI;
        
        public event Action<int> OnOneConversationEnd;

        void Start()
        {
            GameObject instance = Instantiate(DialogueUIGO, transform.position, Quaternion.identity);
            DialogueUI = instance.GetComponent<DialogueStandardUI>();
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
        }

        public void StartConversation(int conversationID)
        {
            if (!_conversationData.ContainsKey(conversationID))
            {
                Debug.LogError("Conversation ID not found in conversation data!");
                return;
            }

            _currentConversation = null;
            _currentConversation = new Conversation(conversationID,DialogueUI.TextWriter);
            _currentConversation.DialogueUI = DialogueUI;
            _currentConversation.SpeakerAvatar = DialogueUI.SpeakerAvatar;
            _currentConversation.DialogueController = this;
            _currentConversation.ConversationData = _conversationData[conversationID];
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
        }
    }
}