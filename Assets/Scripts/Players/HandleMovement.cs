using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMovement : MonoBehaviour {
    Rigidbody2D rb;
    StateManager states;
    HandleAnimations anim;

    public float acceleration = 30;
    public float airAcceleartion = 15;
    public float maxSpeed = 20;
    public float jumpSpeed = 8;
    public float jumpDuration = 150;

    float actualSpeed;
    bool justJumped;
    bool canVariableJump;
    float jmpTimer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        states = GetComponent<StateManager>();
        anim = GetComponent<HandleAnimations>();
        rb.freezeRotation = true;
	}

    private void FixedUpdate()
    {
        if (!states.dontMove) {
            HorizontalMovement();
            Jump();
        }
    }

    void HorizontalMovement() {
        actualSpeed = this.maxSpeed;

        if(states.onGround && !states.currenlyAttackig) {
            rb.AddForce(new Vector2((states.horizontal * actualSpeed) - rb.velocity.x * this.acceleration, 0));
        }

        if(states.horizontal == 0 && states.onGround) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Jump() { 
        if(states.vertical > 0 || states.jump)
        {
            if (!justJumped) {
                justJumped = true;

                if (states.onGround) {
                    anim.JumpAnim();
                    rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);
                    jmpTimer = 0;
                    canVariableJump = true;
                }
            }
            else
            {
                if (canVariableJump) {
                    jmpTimer += Time.deltaTime;

                    if (jmpTimer < this.jumpDuration / 1000) {
                        rb.velocity = new Vector3(rb.velocity.x, this.jumpSpeed);

                    }
                }
            }
        }
        else {
            justJumped = false;
        }
    }

    public void AddVelocityOnCharacter(Vector3 direction, float timer) {
        StartCoroutine(AddVelocity(timer, direction));
    }

    IEnumerator AddVelocity(float timer, Vector3 direction) {
        float t = 0;
        while (t < timer) {
            t += Time.deltaTime;

            rb.velocity = direction;
            yield return null;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
