﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour {

    StateManager states;

    public HandleDamageColliders.DamageType DamageType;
	// Use this for initialization
	void Start () {
        states = GetComponentInParent<StateManager>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<StateManager>()) {
            StateManager oState = other.GetComponentInParent<StateManager>();

            if(oState != states) {

                if (!oState.currenlyAttackig) {
                    oState.TakeDamage(30, DamageType); 
                }

            }
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}