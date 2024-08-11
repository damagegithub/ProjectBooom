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

        public Rigidbody2D RigidBody2D;
        public Collider2D collider2d;
        public bool controlEnabled = true;

        private Vector2 move;
        private SpriteRenderer spriteRenderer;

        public Bounds Bounds => collider2d.bounds;

        private void Awake()
        {
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            RigidBody2D = GetComponent<Rigidbody2D>();
        }

        //控制玩家移动
        public void Move(Vector2 direction)
        {
            if (controlEnabled)
            {
                move = direction.normalized * maxSpeed;
            }
        }
        
        private bool LastDirection => spriteRenderer.flipX;

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
                
                var direction = move.x > 0;
                if (direction != LastDirection)
                {
                    spriteRenderer.flipX = direction;
                }

                // transform.Translate(move * Time.deltaTime);
                RigidBody2D.velocity = move;
            }
        }
    }
}