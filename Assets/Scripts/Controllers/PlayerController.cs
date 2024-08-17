using System.Collections.Generic;
using PBDialogueSystem;
using Spine;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Controllers
{
    /// <summary>
    ///     This is the main class used to implement control of the player.
    ///     It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private List<DialogueActor> _actors = new();

        [Header("角色最大速度")]
        public float maxSpeed = 7;
        [Header("Spine Idle 动画速度")]
        public float SpineIdleTimeScale = 0.5f;
        [Header("Spine Walk 动画速度")]
        public float SpineWalkTimeScale = 1f;
        [Header("是否可控制")]
        public bool controlEnabled = true;

        public Rigidbody2D RigidBody2D;
        public Collider2D  collider2d;
        public Bounds      Bounds => collider2d.bounds;

        [SpineAnimation] public string            runAnimationName;
        [SpineAnimation] public string            idleAnimationName;
        public                  float             runWalkDuration = 1.5f;
        public                  SkeletonAnimation skeletonAnimation;
        public                  AnimationState    spineAnimationState;
        public                  Skeleton          skeleton;


        private void Awake()
        {
            // PlayerPrefs.DeleteAll();
            collider2d = GetComponent<Collider2D>();
            RigidBody2D = GetComponent<Rigidbody2D>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;

            OldState = AnimationType.Idle;
            spineAnimationState.SetAnimation(0, idleAnimationName, true);
            skeletonAnimation.timeScale = SpineIdleTimeScale;
        }

        private bool LastDirection => transform.localScale.x > 0; // left

        public enum AnimationType
        {
            Idle,
            Run,
        }

        private AnimationType OldState = AnimationType.Idle;

        public bool IsScriptControl;
        public Vector2 ScriptSpeed;

        private void Update()
        {
            Vector2 move = Vector2.zero;
            if (IsScriptControl)
            {
                move = ScriptSpeed;
            }
            else if (controlEnabled)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    move.x -= maxSpeed;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    move.x += maxSpeed;
                }
            }

            if (IsScriptControl || controlEnabled)
            {
                if (!Mathf.Approximately(move.x, 0f))
                {
                    bool direction = move.x < 0;
                    if (direction != LastDirection)
                    {
                        Vector3 localScale = transform.localScale;
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
                
            }
            else
            {
                spineAnimationState.SetAnimation(0, idleAnimationName, true);
            }
                RigidBody2D.velocity = move;

            // 更新 idle 速度
            skeletonAnimation.timeScale = OldState switch
            {
                AnimationType.Idle => SpineIdleTimeScale,
                AnimationType.Run  => SpineWalkTimeScale,
                _                  => skeletonAnimation.timeScale,
            };
        }
    }
}