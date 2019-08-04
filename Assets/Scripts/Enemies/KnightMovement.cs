using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class KnightMovement : EnemyShell {

	private Vector2 patrolPosition1;
	private Vector2 patrolPosition2;

	public Vector2 LookDirection { get; private set; }

	private int patrollingToPosition = 2;
	private float stopDuration = 1f;

	private bool isMovingHorizontally;
	private bool startWithPositiveMovement;

	float horizontalMovement;
	float verticalMovement;

    private Weapon heldWeapon;

	private Transform player;

    public LayerMask weaponsMask;

	public bool isPatrolling { get; set; } = true;
	private int playerLayerMask;

    private float weaponCooldown = 2f;
    private float weaponCooldownMax = 2f;

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
        if (!active || isStunned)
        {
            return;
        }

        if (heldWeapon != null)
        {
            heldWeapon.transform.position = transform.position;
            if (heldWeapon.cooldown <= 0) {
                weaponCooldown = weaponCooldown - Time.deltaTime;
                if (weaponCooldown <= 0) {
                    weaponCooldown = weaponCooldownMax;
                    switch(heldWeapon) {
                        case Sword s:
                            StartCoroutine(s.Attack());
                            break;
                        case Bow bow:
                            bow.Attack();
                            break;
                        case Twig t:
                            StartCoroutine(t.Attack());
                            break;
                        case Bomb bomb:
                            StartCoroutine(bomb.Attack());
                            break;
                    }
                }
            }
        }

        if (!isPatrolling) {
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
        if (moveSpeed < 10)
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

                if (heldWeapon == null) {
                    Weapon weapon = null;
                    Vector2 distance = Vector2.zero;
                    RaycastHit2D[] results = Physics2D.BoxCastAll(transform.position, Vector2.one * 8, 0, Vector2.zero, 0, weaponsMask, 0);
                    foreach (RaycastHit2D result in results) {
                        Weapon w = result.transform.gameObject.GetComponentInParent<Weapon>();
                        if (w) {
                            if (!w.isInReserve && !w.isAttacking && !w.isPickedUp) {
                                if (weapon != null)
                                {
                                    Vector2 d = weapon.transform.position - transform.position;
                                    if (d.sqrMagnitude > distance.sqrMagnitude) {
                                        continue;
                                    }
                                }
                                weapon = w;
                                distance = weapon.transform.position - transform.position;
                            }
                        }
                    }
                    if (weapon != null) {
                        player = weapon.transform.GetChild(0);
                        if (moveSpeed < 10)
                            moveSpeed += speedUpOnPlayerFound;
                        isPatrolling = false;
                        StartCoroutine(FollowPlayer());
                        break;
                    }
                }

				Move(horizontalMovement, verticalMovement);
			}

			yield return null;
		}
	}

	private IEnumerator FollowPlayer() {
		while (player != null) {
            if (player.GetComponent<Weapon>() is Weapon w) {
                if (w.isPickedUp || w.isAttacking || w.isInReserve)
                {
                    isPatrolling = true;
                    if (moveSpeed > 7) moveSpeed -= speedUpOnPlayerFound;
                    patrolPosition1 = transform.position;
                    patrolPosition2 = GetSecondPatrolPosition();
                    yield break;
                }
            }
			Vector2 distance = player.position - transform.position;

			if (distance.magnitude < 0.2f || !active) {
				animator.SetInteger("HorizontalSpeed", 0);
				animator.SetInteger("VerticalSpeed", 0);
				yield return new WaitForSeconds(1);

				isPatrolling = true;
                if (moveSpeed > 10)
                    moveSpeed += speedUpOnPlayerFound;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Weapon") && collision.transform == player)
        {
            Weapon weapon = collision.GetComponentInParent<Weapon>();

            if (weapon.holder == null)
            {
                heldWeapon = weapon;
                heldWeapon.isPickedUp = true;
                heldWeapon.holder = gameObject;
                patrolPosition1 = transform.position;
                patrolPosition2 = GetSecondPatrolPosition();
                patrollingToPosition = 2;
                isPatrolling = true;
                player = null;
            }
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

    public override void Die(GameObject culprit) {
        if (heldWeapon != null) {
            heldWeapon.isPickedUp = false;
            heldWeapon.isAttacking = false;
            heldWeapon.cooldown = 0;
            heldWeapon.holder = null;
        }
        base.Die(culprit);
    }
}
