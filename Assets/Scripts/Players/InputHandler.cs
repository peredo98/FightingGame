using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public string playerInput;

    float 
    horizontal;
    float vertical;
    bool jump;
    bool attack1;
    bool attack2;
    bool attack3;

    StateManager states;
	// Use this for initialization
	void Start () {
        states = GetComponent<StateManager>();
	}

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal" + playerInput);
        vertical = Input.GetAxis("Vertical" + playerInput);
        attack1 = Input.GetButton("Fire1" + playerInput);
        attack2 = Input.GetButton("Fire2" + playerInput);
        attack3 = Input.GetButton("Fire3" + playerInput);
        jump = Input.GetButton("Jump" + playerInput);

        states.horizontal = horizontal;
        states.vertical = vertical;
        states.attack1 = attack1;
        states.attack2 = attack2;
        states.attack3 = attack3;
        states.jump = jump;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
