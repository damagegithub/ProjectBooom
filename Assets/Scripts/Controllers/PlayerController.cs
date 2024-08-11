using System.Collections;
using System.Collections.Generic;
using PBDialogueSystem;
using Spine.Unity;
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

        [SpineAnimation] public string runAnimationName;
        [SpineAnimation] public string idleAnimationName;
        public float runWalkDuration = 1.5f;
        public SkeletonAnimation skeletonAnimation;
        public Spine.AnimationState spineAnimationState;
        public Spine.Skeleton skeleton;

        private void Awake()
        {
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            RigidBody2D = GetComponent<Rigidbody2D>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;
        }

        //控制玩家移动
        public void Move(Vector2 direction)
        {
            if (controlEnabled)
            {
                move = direction.normalized * maxSpeed;
            }
        }

        private bool LastDirection => transform.localScale.x > 0; // left

        public enum AnimationType
        {
            Idle,
            Run,
        }

        private AnimationType OldState = AnimationType.Idle;

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

                if (!Mathf.Approximately(move.x, 0f))
                {
                    var direction = move.x < 0;
                    if (direction != LastDirection)
                    {
                        var localScale = transform.localScale;
                        localScale.x = direction ? Mathf.Abs(localScale.x) : -Mathf.Abs(localScale.x);
                        transform.localScale = localScale;
                    }
                }

                AnimationType newState = Mathf.Approximately(move.x, 0f) ? AnimationType.Idle : AnimationType.Run;
                if (newState != OldState)
                {
                    OldState = newState;
                    switch (newState)
                    {
                        case AnimationType.Idle:
                            spineAnimationState.SetAnimation(0, idleAnimationName, true);
                            break;
                        case AnimationType.Run:
                            spineAnimationState.SetAnimation(0, runAnimationName, true);
                            break;
                    }
                }


                // transform.Translate(move * Time.deltaTime);
                RigidBody2D.velocity = move;
            }
        }
    }
}