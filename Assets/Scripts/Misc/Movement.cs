using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class Movement : MonoBehaviour{

	[SerializeField] protected Animator animator;

	[SerializeField] protected float moveSpeed = 4f;

	protected Rigidbody2D body;

	protected bool isFacingLeft  = false;
	protected bool isFacingDown  = true;
	protected bool isFacingRight = false;
	protected bool isFacingUp    = false;

    [HideInInspector]
    public float iFrames = 0;

	protected bool isStunned = false;

    public void LateUpdate()
    {
        if (iFrames <= 0) return;
        iFrames -= Time.deltaTime;
    }

    public void Hurt(){
        gameObject.SetActive(false);
        if (gameObject.tag == "Player") {
            GameManager.instance.GameOver();
        }
    }

    protected void MovementSetup() {
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
	}

	protected void Move(float horizontalMovement, float verticalMovement) {
		Vector2 movePosition = transform.position;

        if (Abs(horizontalMovement) + Abs(verticalMovement) > 1) {
            horizontalMovement *= 0.7f;
            verticalMovement *= 0.7f;
        }

		if (horizontalMovement != 0) {
			movePosition.x += horizontalMovement * moveSpeed * Time.deltaTime;
			animator.SetInteger("HorizontalSpeed", (int)Sign(horizontalMovement));
			animator.SetInteger("VerticalSpeed", 0);

			if (horizontalMovement > 0) {
				isFacingRight = true;
				isFacingLeft = false;
			} else {
				isFacingLeft = true;
				isFacingRight = false;
			}
			isFacingUp = false;
			isFacingDown = false;
		}
		if (verticalMovement != 0) {
			movePosition.y += verticalMovement * moveSpeed * Time.deltaTime;
			animator.SetInteger("HorizontalSpeed", 0);
			animator.SetInteger("VerticalSpeed", (int)Sign(verticalMovement));

			if (verticalMovement > 0) {
				isFacingUp = true;
				isFacingDown = false;
			}
			else {
				isFacingDown = true;
				isFacingUp = false;
			}
			isFacingLeft = false;
			isFacingRight = false;
		}

		if (horizontalMovement == 0 && verticalMovement == 0) {
			animator.SetInteger("HorizontalSpeed", 0);
			animator.SetInteger("VerticalSpeed", 0);
		}

		body.MovePosition(movePosition);
	}
}
