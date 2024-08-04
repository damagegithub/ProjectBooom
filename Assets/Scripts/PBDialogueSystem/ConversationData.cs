using System.Collections.Generic;

namespace PBDialogueSystem
{
    public class DialogueData
    {
        public int ConversationID;
        public int DialogueID;
        public int SpeakerID;
        public string DialogueText;
        public bool bShowAvatar;
        public int AvatarPosX;
        public int AvatarPosY;
        public int FullBodySlot1;
        public int FullBodySlot2;
        public int FullBodySlot3;
        public int FullBodySlot1_X;
        public int FullBodySlot1_Y;
        public int FullBodySlot2_X;
        public int FullBodySlot2_Y;
        public int FullBodySlot3_X;
        public int FullBodySlot3_Y;
    }

    public class ConversationData
    {
        public List<DialogueData> DialogueDatas;
    }
}