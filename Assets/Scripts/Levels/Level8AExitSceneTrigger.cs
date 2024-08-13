using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class Level8AExitSceneTrigger : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Level 8A Exit Scene Triggered");
            SceneManager.LoadScene("_8A2");
            SceneManager.UnloadSceneAsync("_8A");
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
