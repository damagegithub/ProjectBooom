using System.Collections.Generic;
using PBDialogueSystem;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    ///     This is the main class used to implement control of the player.
    ///     It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private List<DialogueActor> _actors = new List<DialogueActor>();

        /// <summary>
        ///     Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;

        public Collider2D collider2d;
        public bool controlEnabled = true;

        private Vector2 move;
        private SpriteRenderer spriteRenderer;

        public Bounds Bounds => collider2d.bounds;

        private void Awake()
        {
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // List<DialogueActor> Actors = CSVToJsonUtil.GetJsonData<DialogueActor>("Tables/Actors");
            // foreach (var Actor in Actors)
            // {
            //     _actors.Add(Actor);
            // }
            //
            // int selectedCharacterID = PlayerPrefs.GetInt("SelectedCharacterID", 0);
            // if (selectedCharacterID <= 0)
            // {
            //     Debug.LogWarning("No character selected, defaulting to first character.");
            // }
            // else
            // {
            //     foreach (var actor in _actors)
            //     {
            //         if (actor.ActorID == selectedCharacterID)
            //         {
            //             GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(actor.ActorFullBodyImagePath);
            //         }
            //     }
            // }
        }

        //控制玩家移动
        public void Move(Vector2 direction)
        {
            if (controlEnabled)
            {
                move = direction.normalized * maxSpeed;
            }
        }

        private void Update()
        {
            if (controlEnabled)
            {
                move = Vector2.zero;
                if (Input.GetKey(KeyCode.A))
                {
                    move.x -= maxSpeed;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    move.x += maxSpeed;
                }

                transform.Translate(move * Time.deltaTime);
            }
        }
    }
}