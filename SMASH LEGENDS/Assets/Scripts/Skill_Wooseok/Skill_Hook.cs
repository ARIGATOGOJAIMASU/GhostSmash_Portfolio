using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class Skill_Hook : Skill_Slash
    {
        public enum STATE { MOVING, HOOKED, ENDED }

        [SerializeField]
        GameObject Hook;
        Transform HookTransform;
        Transform ParentTransform;
        GameObject HookedTarget;
        [SerializeField]
        Junpyo.PlayerController_FSM HookedTargetPCFSM;
        Transform HookedTargetTransform;

        [SerializeField]
        public STATE curState;

        [SerializeField]
        float MaxMovingTime;

        [SerializeField]
        float HookForwardSpeed;

        [SerializeField]
        float HookBackwardSpeed;

        [SerializeField]
        public int HookingSound;

        public Skill_Hook(GameObject ParentPlayer, Skill FollowUp) : base(ParentPlayer, FollowUp)
        {
            return;
        }

        public override void FollowUp()
        {
            ChangeState(STATE.HOOKED);
        }

        protected override void Awake()
        {
            HookTransform = Hook.transform;
        }

        public override void SkillEffectOnEnter(GameObject otherobj)
        {
            if (
                (!ParentPlayer.CompareTag(otherobj.tag))
                && ParentPlayer != otherobj
                && MaxHit > curhit
                && MaxTargetNumber > slappedtarget.Count
                && !GameObjectChecker(slappedtarget, otherobj)
                )
            {
                if (otherobj.layer == LayerMask.NameToLayer("Player"))
                {
                    HitEnemy(otherobj);
                    slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
                    curhit++;
                    DesignateTarget(otherobj);
                    GameManager.Instance.FollowUpSkill((int)Junpyo.SKILLTYPE.ULTIMATE, ParentScript.ID);
                }
                else if (otherobj.layer == LayerMask.NameToLayer("Coffin"))
                {
                    HitCoffin(otherobj);
                    slappedtarget.Add(new Pair<GameObject, int>(otherobj, 1));
                    curhit++;
                    DesignateTarget(otherobj);
                    GameManager.Instance.FollowUpSkill((int)Junpyo.SKILLTYPE.ULTIMATE, ParentScript.ID);
                }
            }
        }

        private void DesignateTarget(GameObject Target)
        {
            if(Target == null)
            {
                HookedTargetTransform = null;
                HookedTarget = null;
                HookedTargetPCFSM = null;
                return;
            }
            HookedTarget = Target;
            HookedTargetTransform = HookedTarget.transform;
            HookedTargetPCFSM = Target.GetComponent<Junpyo.PlayerController_FSM>();
            PlayHitSound();
        }

        protected override void FixedUpdate()
        {
            if (timer < LifeTime)
            {
                timer += Time.fixedDeltaTime;
            }
            if (timer >= MaxMovingTime && curState == STATE.MOVING)
            {
                ChangeState(STATE.ENDED);
            }

            switch (curState)
            {
                case STATE.MOVING:
                    HookTransform.localPosition += Vector3.forward * HookForwardSpeed * Time.fixedDeltaTime;
                    break;
                case STATE.HOOKED:
                    HookTransform.position = Vector3.Lerp(HookTransform.position, ParentTransform.position, HookBackwardSpeed * Time.fixedDeltaTime / Vector3.Distance(HookTransform.position, ParentTransform.position));
                    if(HookedTarget != null)
                    {
                        GameManager.Instance.MovePlayer(HookTransform.position, HookedTargetPCFSM.ID);
                    }

                    if (Vector3.Distance(HookTransform.position, ParentTransform.position) < 0.01f)
                    {
                        ChangeState(STATE.ENDED);
                    }
                    break;
                case STATE.ENDED:
                    HookTransform.localPosition = Vector3.zero;
                    gameObject.SetActive(false);
                    break;
            }
        }

        public override void Restart()
        {
            base.Restart();
            ParentTransform = ParentPlayer.transform;
            HookTransform.localPosition = Vector3.zero;
            ChangeState(STATE.MOVING);
        }

        protected void ChangeState(STATE state)
        {
            switch (state)
            {
                case STATE.MOVING:
                    DesignateTarget(null);
                    break;
                case STATE.HOOKED:
                    break;
                case STATE.ENDED:
                    DesignateTarget(null);
                    break;
            }
            curState = state;
        }
        public override void PlayHitSound()
        {
            base.PlayHitSound();
            if (HookingSound != -1)
            {
                myAudio.Play(Juhyung.SoundManager.Instance.HitSound[HookingSound], Juhyung.AudioManager.DEFINE.REPEAT);
            }
        }
    }
}