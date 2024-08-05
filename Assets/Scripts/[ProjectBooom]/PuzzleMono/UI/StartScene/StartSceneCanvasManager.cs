using UnityEngine;

namespace _ProjectBooom_.PuzzleMono.UI.StartScene
{
    public class StartSceneCanvasManager : MonoBehaviour
    {
        [SerializeField]
        [Header("角色选择面板")]
        public GameObject CharacterPanel;
        [SerializeField]
        [Header("对话系统面板")]
        public GameObject DialogSystemPanel;
        [SerializeField]
        [Header("开始界面面板")]
        public GameObject StartPanel;
    }
}