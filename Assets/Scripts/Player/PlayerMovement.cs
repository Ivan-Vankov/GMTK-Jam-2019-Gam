using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : Movement {

	private void Start() {
		base.MovementSetup();
	}

	void Update() {
		float horizontalMovement = Input.GetAxisRaw("Horizontal");
		float verticalMovement = Input.GetAxisRaw("Vertical");

		Move(horizontalMovement, verticalMovement);
	}
}
