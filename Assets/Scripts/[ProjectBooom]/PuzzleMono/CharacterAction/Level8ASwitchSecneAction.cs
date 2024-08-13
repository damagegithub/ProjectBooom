using _ProjectBooom_.PuzzleMono.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _ProjectBooom_.PuzzleMono.CharacterAction
{
    public class Level8ASwitchSecneAction : NearestAction
    {
        public override void DoAction()
        {
            if (!IsTriggered)
            {
                base.DoAction();
                Debug.Log("Level 8A Exit Scene Triggered");
                SceneManager.LoadScene("_8A2");
                SceneManager.UnloadSceneAsync("_8A");
            }
        }

    }
}