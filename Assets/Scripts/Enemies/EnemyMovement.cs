using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement {

	private Vector2 patrolPosition1;
	private Vector2 patrolPosition2;

	private int patrollingToPosition = 2;
	private float stopDuration = 1f;

	private bool isMovingHorizontally;
	private bool startWithPositiveMovement;

	float horizontalMovement;
	float verticalMovement;

	private void Start() {
		base.MovementSetup();
		base.moveSpeed = 8;
		patrolPosition1 = transform.position;
		patrolPosition2 = GetSecondPatrolPosition();
		StartCoroutine(Patrol());
	}

	private IEnumerator Patrol() {
		while (true) {
			horizontalMovement = 0;
			verticalMovement = 0;
			
			if (patrollingToPosition == 2) {
				if (isMovingHorizontally) {
					horizontalMovement = moveSpeed * Time.deltaTime;
					if (!startWithPositiveMovement) {
						horizontalMovement *= -1;
					}
				}
				else {
					verticalMovement = moveSpeed * Time.deltaTime;
					if (!startWithPositiveMovement) {
						verticalMovement *= -1;
					}
				}

				if (Vector2.Distance(transform.position, patrolPosition2) < 0.1f) {
					patrollingToPosition = 0;

					animator.SetInteger("HorizontalSpeed", 0);
					animator.SetInteger("VerticalSpeed", 0);

					yield return new WaitForSeconds(stopDuration);
					patrollingToPosition = 1;
				}

			} else if (patrollingToPosition == 1) {

				if (isMovingHorizontally) {
					horizontalMovement = -moveSpeed * Time.deltaTime;
					if (!startWithPositiveMovement) {
						horizontalMovement *= -1;
					}
				}
				else {
					verticalMovement = -moveSpeed * Time.deltaTime;
					if (!startWithPositiveMovement) {
						verticalMovement *= -1;
					}
				}

				if (Vector2.Distance(transform.position, patrolPosition1) < 0.1f) {
					patrollingToPosition = 0;

					animator.SetInteger("HorizontalSpeed", 0);
					animator.SetInteger("VerticalSpeed", 0);

					yield return new WaitForSeconds(stopDuration);
					patrollingToPosition = 2;
				}
			}

			Move(horizontalMovement, verticalMovement);
			
			yield return null;
		}
	}

	private Vector2 GetSecondPatrolPosition() {

		int wallLayer = LayerMask.GetMask("Wall");

		Vector2[] directions = {
			Vector2.up,
			Vector2.right,
			Vector2.down,
			Vector2.left
		};

		RaycastHit2D[] rayHits = {
			Physics2D.Raycast(transform.position, directions[0], 100, wallLayer),
			Physics2D.Raycast(transform.position, directions[1], 100, wallLayer),
			Physics2D.Raycast(transform.position, directions[2], 100, wallLayer),
			Physics2D.Raycast(transform.position, directions[3], 100, wallLayer),
		};

		Vector2 farthestHit = Vector2.zero;
		float farthestHitDistance = 0;

		for (int i = 0; i < rayHits.Length; i++) {
			if (rayHits[i].distance > farthestHitDistance) {
				farthestHit = rayHits[i].point - directions[i] * 1.5f;
				farthestHitDistance = rayHits[i].distance;

				// Even i-s mean vertical movement and odd i-s horizontal
				isMovingHorizontally = i % 2 != 0;

				// If the enemy goes up or righ initially(has a positive horizontal/vertical movement)
				startWithPositiveMovement = i < 2;
			}
		}

		return farthestHit;
	}
}
