using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {

	[SerializeField] private Arrow arrowPrefab;

	private Transform bowSprite;

	private void Start() {
		base.Start();
		bowSprite = transform.GetChild(0);
	}

	void Update() {
		base.Update();
		if (!isPickedUp) {
			return;
        }

        if (isInReserve)
        {
            return;
        }

        if (Input.GetKeyDown(attackKey) && holder == GameManager.instance.Player && cooldown <= 0) {
			Attack();
		}
	}

	public void Attack()
    {
        cooldown = maxCooldown;
        isAttacking = true;
        Arrow spawnedArrow = Instantiate(arrowPrefab, bowSprite.position, 
			Quaternion.Euler(0, 0, bowSprite.rotation.eulerAngles.z - 90));
		spawnedArrow.holder = holder;
		spawnedArrow.isFired = true;
		spawnedArrow.fireDirection = transform.right;

		Selfdestruct();
	}
}
