using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon {

	private Transform weaponTransform;

	private void Start() {
		base.Start();
		weaponTransform = weaponRenderer.transform;
	}

	// Overrides the default weapon update loop
	void Update() {

		transform.localRotation = Quaternion.Euler(0, 0, 0);

		if (!isPickedUp) {
			return;
		}

		if (isInReserve) {
			return;
		}

		if (holder.activeSelf == false) {
			Selfdestruct();
		}

		Vector2 lookDirection = Vector2.down;

		if (holder.tag == "Player") {
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			lookDirection = new Vector2 {
				x = mousePosition.x - transform.position.x,
				y = mousePosition.y - transform.position.y
			}.normalized;
		}

		Vector2 weaponOffset = new Vector2(0, -2);

		if (lookDirection.x >= -0.88 && lookDirection.x <= -0.46 && 
			lookDirection.y >= 0.44  && lookDirection.y <= 0.90) {
			weaponOffset.x = -0.35f;
			weaponOffset.y =  0.35f;
			weaponRenderer.sortingLayerName = "Weapon Below Holder";
			weaponRenderer.sprite = weaponUpLeft;
		}
		else if (lookDirection.x >= -0.44 && lookDirection.x <= 0.46 &&
			lookDirection.y >= 0.88 && lookDirection.y <= 1) {
			weaponOffset.x = 0;
			weaponOffset.y = 0.5f;
			weaponRenderer.sortingLayerName = "Weapon Below Holder";
			weaponRenderer.sprite = weaponUp;
		}
		else if (lookDirection.x >= 0.44 && lookDirection.x <= 0.90 &&
			lookDirection.y >= 0.44 && lookDirection.y <= 0.90) {
			weaponOffset.x = 0.35f;
			weaponOffset.y = 0.35f;
			weaponRenderer.sortingLayerName = "Weapon Below Holder";
			weaponRenderer.sprite = weaponUpRight;
		}
		else if (lookDirection.x >= -1 && lookDirection.x <= -0.90 &&
			lookDirection.y >= -0.44 && lookDirection.y <= 0.46) {
			weaponOffset.x = -0.5f;
			weaponOffset.y = 0;
			weaponRenderer.sortingLayerName = "Weapon Above Holder";
			weaponRenderer.sprite = weaponLeft;
		}
		else if (lookDirection.x >= 0.88 && lookDirection.x <= 1 &&
			lookDirection.y >= -0.44 && lookDirection.y <= 0.46) {
			weaponOffset.x = 0.5f;
			weaponOffset.y = 0;
			weaponRenderer.sortingLayerName = "Weapon Above Holder";
			weaponRenderer.sprite = weaponRight;
		}
		else if (lookDirection.x >= -0.88 && lookDirection.x <= -0.46 &&
			lookDirection.y >= -0.88 && lookDirection.y <= -0.46) {
			weaponOffset.x = -0.35f;
			weaponOffset.y = -0.35f;
			weaponRenderer.sortingLayerName = "Weapon Above Holder";
			weaponRenderer.sprite = weaponDownLeft;
		}
		else if (lookDirection.x >= -0.44 && lookDirection.x <= 0.46 &&
			lookDirection.y >= -1 && lookDirection.y <= -0.90) {
			weaponOffset.x = 0;
			weaponOffset.y = -0.5f;
			weaponRenderer.sortingLayerName = "Weapon Above Holder";
			weaponRenderer.sprite = weaponDown;
		}
		else if (lookDirection.x >= 0.44 && lookDirection.x <= 0.90 &&
			lookDirection.y >= -0.88 && lookDirection.y <= -0.46) {
			weaponOffset.x = 0.35f;
			weaponOffset.y = -0.35f;
			weaponRenderer.sortingLayerName = "Weapon Above Holder";
			weaponRenderer.sprite = weaponDownRight;
		}

		if (weaponOffset.y != -2) {
			weaponTransform.localPosition = weaponOffset;
		}
	}
}
