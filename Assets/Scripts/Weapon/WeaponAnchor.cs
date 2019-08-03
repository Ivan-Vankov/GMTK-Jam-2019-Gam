using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnchor : MonoBehaviour {

	[SerializeField] private Sprite weaponUp;
	[SerializeField] private Sprite weaponUpRight;
	[SerializeField] private Sprite weaponRight;
	[SerializeField] private Sprite weaponDownRight;
	[SerializeField] private Sprite weaponDown;
	[SerializeField] private Sprite weaponDownLeft;
	[SerializeField] private Sprite weaponLeft;
	[SerializeField] private Sprite weaponUpLeft;

	private SpriteRenderer weaponRenderer;

	private WeaponAttack weaponAttack;

	private void Start() {
		weaponRenderer = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
		weaponAttack = GetComponent<WeaponAttack>();
	}

	void Update() {
		if (weaponAttack.isAttacking) {
			return;
		}

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 lookDirection = new Vector2 {
			x = mousePosition.x - transform.position.x,
			y = mousePosition.y - transform.position.y
		};

		transform.right = lookDirection;
    }
}
