using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class SkeletonMovement : EnemyShell {

	private Vector2 patrolPosition1;
	private Vector2 patrolPosition2;

	public Vector2 LookDirection { get; private set; }

	private int patrollingToPosition = 2;
	private float stopDuration = 1f;

	private bool isMovingHorizontally;
	private bool startWithPositiveMovement;

	float horizontalMovement;
	float verticalMovement;

	private Transform player;

	public bool isPatrolling { get; set; } = true;
	private int playerLayerMask;

	[SerializeField] private float speedUpOnPlayerFound = 4f;

	private void Start() {
		base.MovementSetup();
        base.Start();
		patrolPosition1 = transform.position;
		patrolPosition2 = GetSecondPatrolPosition();
		LookDirection = patrolPosition2 - (Vector2)transform.position;
		playerLayerMask = LayerMask.GetMask("Player");
		StartCoroutine(Patrol());
	}

	void Update()
    {
        if (!active || !isPatrolling || isStunned)
        {
            return;
        }
        RaycastHit2D rayHit = Physics2D.Raycast(
			transform.position,
			LookDirection,
			100,
			playerLayerMask);

		if (rayHit.collider == null) {
			return;
		}

		player = rayHit.transform;

		// Speedup
		moveSpeed += speedUpOnPlayerFound;

		isPatrolling = false;
		StartCoroutine(FollowPlayer());
	}

	private IEnumerator Patrol() {
		while (true) {
            while (!active) {
                yield return new WaitUntil(() => active);
            }
                
			if (isPatrolling) {
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
						LookDirection = patrolPosition1 - (Vector2)transform.position;
					}

				}
				else if (patrollingToPosition == 1) {

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
						LookDirection = patrolPosition1 - (Vector2)transform.position;
					}
				}

				Move(horizontalMovement, verticalMovement);
			}

			yield return null;
		}
	}

	private IEnumerator FollowPlayer() {
		while (true) {
			Vector2 distance = player.position - transform.position;

			if (distance.magnitude < 0.5f || !active) {
				animator.SetInteger("HorizontalSpeed", 0);
				animator.SetInteger("VerticalSpeed", 0);
				yield return new WaitForSeconds(1);

				isPatrolling = true;
				moveSpeed -= speedUpOnPlayerFound;
				patrolPosition1 = transform.position;
				patrolPosition2 = GetSecondPatrolPosition();
				LookDirection = patrolPosition2 - (Vector2)transform.position;
				yield break;
			}

			horizontalMovement = 0;
			verticalMovement = 0;

			if (Abs(distance.y) < 0.5f) {
				horizontalMovement = moveSpeed * Sign(distance.x) * Time.deltaTime;
			}
			else {
				verticalMovement = moveSpeed * Sign(distance.y) * Time.deltaTime;
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
			if (rayHits[i].distance * (1 + 1.5f * ((i + 1) % 2)) > farthestHitDistance) {
				// Slight offset so that it doesn;t clip into the walls
				farthestHit = rayHits[i].point - directions[i] * 1f;

                // Make vertical distance artificially longer for more chances at vertical patrolling...
				farthestHitDistance = rayHits[i].distance * (1 + 1.5f * ((i + 1) % 2));

                // Even i-s mean vertical movement and odd i-s horizontal
                isMovingHorizontally = i % 2 != 0;

                // If the enemy goes up or righ initially(has a positive horizontal/vertical movement)
                startWithPositiveMovement = i < 2;
            }
        }

        return farthestHit;
	}
}
