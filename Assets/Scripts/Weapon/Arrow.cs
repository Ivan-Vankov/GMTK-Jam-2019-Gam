using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour {

	private Rigidbody2D body;

	public GameObject holder;

	public bool isFired = false;

	public Vector2 fireDirection = Vector2.right;
	public float fireSpeed = 5f;


	private void Start() {
		body = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		if (isFired) {
			Move();
		}
	}

	private void Move() {
		transform.position += (Vector3)fireDirection.normalized * fireSpeed * Time.deltaTime;
	}

	public void Selfdestruct() {
		Destroy(gameObject);
	}
}
