using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
public class ReceiveDamage : MonoBehaviour {

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Weapon")) {

			// Get WeaponAttack from Weapon Anchor
			WeaponAttack weaponAttack = collision.GetComponentInParent<WeaponAttack>();

			if (weaponAttack.isAttacking) {
				AudioManager.instance.PlayBigDeathSound();
				Destroy(gameObject);
			}
		}
	}
}
