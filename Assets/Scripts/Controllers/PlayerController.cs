using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        public Collider2D collider2d;
        public bool controlEnabled = true;
        
        Vector2 move;
        SpriteRenderer spriteRenderer;
        
        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //控制玩家移动
        public void Move(Vector2 direction)
        {
            if (controlEnabled)
            {
                move = direction.normalized * maxSpeed;
            }
        }
        
        void Update()
        {
            if (controlEnabled)
            {
                move = Vector2.zero;
                if (Input.GetKey(KeyCode.A))
                {
                    move.x = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    move.x = 1;
                }
                transform.Translate(move * Time.deltaTime);
            }            
        }
        
    }
}