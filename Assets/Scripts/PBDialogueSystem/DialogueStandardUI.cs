using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PBDialogueSystem
{
    public class DialogueStandardUI : MonoBehaviour
    {

        public PBTypeWritter TextWriter;
        
        public TextMeshProUGUI SpeakerNameTextMesh;
        public Image SpeakerAvatar;
        public string SpeakerName { get; set; }
        public string DialogueText { get; set; }
        public Button NextButton { get; set; }
        public Button SkipButton { get; set; }
        
        public Image FullBodyImage1;
        public Image FullBodyImage2;
        public Image FullBodyImage3;
        
        
        
    }
}