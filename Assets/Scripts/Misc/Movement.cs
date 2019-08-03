using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class Movement : MonoBehaviour{

	[SerializeField] protected Animator animator;

	[SerializeField] protected float moveSpeed = 5f;

	[SerializeField] private Transform weaponAnchor;

	private Rigidbody2D body;

	//private float originalScaleX;

	protected void MovementSetup() {
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
		//originalScaleX = transform.localScale.x;
	}

	protected void Move(float horizontalMovement, float verticalMovement) {
		Vector2 movePosition = transform.position;

		if (horizontalMovement != 0) {
			movePosition.x += horizontalMovement * moveSpeed * Time.deltaTime;
			animator.SetInteger("HorizontalSpeed", (int)Sign(horizontalMovement));
			animator.SetInteger("VerticalSpeed", 0);
			//Vector3 newScale = transform.localScale;
			//if (horizontalMovement > 0) {
			//	newScale.x = originalScaleX;
			//} else {
			//	newScale.x = -originalScaleX;
			//}
			//transform.localScale = newScale;

		}
		else if (verticalMovement != 0) {
			movePosition.y += verticalMovement * moveSpeed * Time.deltaTime;
			animator.SetInteger("HorizontalSpeed", 0);
			animator.SetInteger("VerticalSpeed", (int)Sign(verticalMovement));
		}
		else {
			animator.SetInteger("HorizontalSpeed", 0);
			animator.SetInteger("VerticalSpeed", 0);
		}
		body.MovePosition(movePosition);

		if (weaponAnchor != null) {
			weaponAnchor.position = transform.position;
		}
	}
}
