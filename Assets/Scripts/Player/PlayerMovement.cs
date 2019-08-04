using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : Movement {

	private KeyCode dashKey = KeyCode.Space;

	private float dashSpeed;

	private bool isDashing = false;
	private float dashDuration = 0.1f;

	private void Start() {
		base.MovementSetup();
		dashSpeed = moveSpeed * 3;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Weapon")) {
            if (other.GetComponentInParent<Weapon>() is Weapon w && w.holder.tag != "Player" && w.isAttacking)
            {
                if (iFrames <= 0)
                {
                    Hurt();
                }
            }
        }
        if (other.CompareTag("Arrow"))
        {
            if (other.GetComponentInParent<Arrow>() is Arrow a && a.holder.tag != "Player")
            {
                if (iFrames <= 0)
                {
                    Hurt();
                }
            }
        }
    }

    void Update() {

        if (Time.timeScale == 0) {
            return;
        }

		if (isDashing) {

            return;
		}

		float horizontalMovement = Input.GetAxisRaw("Horizontal");
		float verticalMovement = Input.GetAxisRaw("Vertical");

		if (Input.GetKeyDown(dashKey)) {
			isDashing = true;
            StartCoroutine(Dash(horizontalMovement, verticalMovement));
			return;
		}

		Move(horizontalMovement, verticalMovement);
	}

	private IEnumerator Dash(float horizontalMovement, float verticalMovement) {

		float dashEnd = Time.time + dashDuration;

        if (Abs(horizontalMovement) + Abs(verticalMovement) > 1)
        {
            horizontalMovement *= 0.7f;
            verticalMovement *= 0.7f;
        }

        isDashing = true;
		animator.SetBool("IsDashing", isDashing);

		while (Time.time < dashEnd) {

			Vector2 movePosition = transform.position;

            if (horizontalMovement == 0 && verticalMovement == 0)
            {

                if (isFacingRight)
                {
                    movePosition.x += dashSpeed * Time.deltaTime;
                }
                else if (isFacingDown)
                {
                    movePosition.y += dashSpeed * Time.deltaTime;
                }
                else if (isFacingLeft)
                {
                    movePosition.x -= dashSpeed * Time.deltaTime;
                }
                else if (isFacingUp)
                {
                    movePosition.y += dashSpeed * Time.deltaTime;
                }

            } else {
                movePosition.x += dashSpeed * Time.deltaTime * horizontalMovement;
                movePosition.y += dashSpeed * Time.deltaTime * verticalMovement;
            }

			body.MovePosition(movePosition);

			yield return null;
		}

		isDashing = false;
		animator.SetBool("IsDashing", isDashing);
	}
}
