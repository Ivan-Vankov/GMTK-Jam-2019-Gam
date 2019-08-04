using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon {

	private KeyCode explosionKey = KeyCode.Q;

	public Animator explosion;

	// Override the base weapon update because we don't want the bomb to move
    void Update() {
		if (!isPickedUp || cooldown > 0 || isInReserve) {
			return;
		}

		if (Input.GetKeyDown(explosionKey)) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			Selfdestruct();
		}
	}
}
