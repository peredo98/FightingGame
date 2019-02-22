using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDamageCollider : StateMachineBehaviour{

    StateManager states;

    public HandleDamageColliders.DamageType damageType;
    public HandleDamageColliders.DCtype dCtype;
    public float delay;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
        {
            states = animator.transform.GetComponentInParent<StateManager>();
        }
        states.handleDC.OpenCollider(dCtype, delay, damageType);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
        {
            states = animator.transform.GetComponentInParent<StateManager>();
        }
        states.handleDC.CloseColliders();
    }

}
