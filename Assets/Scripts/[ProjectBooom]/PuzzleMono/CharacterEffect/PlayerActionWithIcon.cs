using _ProjectBooom_.Input;
using _ProjectBooom_.ObservableData;
using _ProjectBooom_.PuzzleMono.CharacterAction;
using DG.Tweening;
using LYP_Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace _ProjectBooom_.PuzzleMono.CharacterEffect
{
    /// <summary>
    ///     显示在头部的可交互提示图标
    /// </summary>
    public class PlayerActionWithIcon : MonoBehaviour
    {
        [Header("可交互的图标提示")]
        public SpriteRenderer _SR_Icon;

        [Header("初始对象空间位置")]
        public Vector3 SceneIconOriginOS;

        [FormerlySerializedAs("IconMoveLatency")]
        [Header("图标移动时间")]
        public float IconMoveDuration = 0.5f;

        private void Awake()
        {
            SceneIconOriginOS = _SR_Icon.transform.localPosition;
        }

        private void OnEnable()
        {
            RuntimeUnimportantData.ActionObjectChanged += IconChange;
            InputWarp.OnActionKeyDown += PlayerDoAction;
            IconChange();
        }

        private void OnDisable()
        {
            RuntimeUnimportantData.ActionObjectChanged -= IconChange;
            InputWarp.OnActionKeyDown -= PlayerDoAction;
        }

        public void IconChange()
        {
            NearestAction nearestAction = RuntimeUnimportantData.GetNearestActionObject(transform.position);
            if (nearestAction is ItemNearestAction)
            {
                DebugHelper.Log($"nearestAction is ItemNearestAction");
                _SR_Icon.sprite = AssetRef.Instance.ItemTipIcon;
                _SR_Icon.enabled = true;
            }
            else if (nearestAction is SwitchNearestAction)
            {
                DebugHelper.Log($"nearestAction is SwitchNearestAction");
                _SR_Icon.sprite = AssetRef.Instance.SwitchTipIcon;
                _SR_Icon.enabled = true;
            }
            else
            {
                DebugHelper.Log($"nearestAction is null");
                _SR_Icon.enabled = false;
                _SR_Icon.sprite = null;
            }

            IconMove(nearestAction);
        }

        public void IconMove(NearestAction nearestAction)
        {
            if (DOTween.IsTweening(this))
            {
                DOTween.Kill(this);
            }

            if (nearestAction is ItemNearestAction)
            {
                DebugHelper.Log($"nearestAction is ItemNearestAction");
                _SR_Icon.sprite = AssetRef.Instance.ItemTipIcon;
                _SR_Icon.enabled = true;
                DOTween.Sequence()
                       .Append(_SR_Icon.transform.DOMove(nearestAction.transform.position, IconMoveDuration))
                       .Join(_SR_Icon.DOFade(1f, IconMoveDuration))
                       .SetId(this);
            }
            else if (nearestAction is SwitchNearestAction)
            {
                DebugHelper.Log($"nearestAction is SwitchNearestAction");
                _SR_Icon.sprite = AssetRef.Instance.SwitchTipIcon;
                _SR_Icon.enabled = true;
                DOTween.Sequence()
                       .Append(_SR_Icon.transform.DOMove(nearestAction.transform.position, IconMoveDuration))
                       .Join(_SR_Icon.DOFade(1f, IconMoveDuration))
                       .SetId(this);
            }
            else
            {
                DebugHelper.Log($"nearestAction is null");
                DOTween.Sequence()
                       .Append(_SR_Icon.transform.DOLocalMove(SceneIconOriginOS, IconMoveDuration))
                       .Join(_SR_Icon.DOFade(0f, IconMoveDuration))
                       .AppendCallback(() =>
                        {
                            _SR_Icon.enabled = false;
                            _SR_Icon.sprite = null;
                        })
                       .SetId(this);
            }
        }

        public void PlayerDoAction()
        {
            NearestAction nearestAction = RuntimeUnimportantData.GetNearestActionObject(transform.position);
            if (nearestAction)
            {
                nearestAction.DoAction();
            }
        }
    }
}