using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	private Transform primaryWeaponAnchor;
	private Transform secondaryWeaponAnchor;

	private void Update() {
		if (primaryWeaponAnchor != null) {
            primaryWeaponAnchor.position = transform.position;
        }

        if (Input.GetButtonDown("Fire2")) {
            SwapWeapons();
        }

        if (Input.GetButtonDown("Fire3")) {
            DepositWeapon();
        }
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Weapon")) {
            GetNewWeapon(collision.gameObject);
		}
	}

    private void GetNewWeapon(GameObject other) {
        Weapon weapon = other.transform.parent.GetComponent<Weapon>();
        if (!weapon.isPickedUp) {
            if (primaryWeaponAnchor == null) {
                primaryWeaponAnchor = other.transform.parent;
                weapon.isPickedUp = true;
                weapon.holder = gameObject;
            } else if (secondaryWeaponAnchor == null) {
                secondaryWeaponAnchor = other.transform.parent;
                weapon.isPickedUp = true;
                weapon.SetReserve(true, transform.position);
                weapon.holder = gameObject;
            }
        }
    }

    public void SwapWeapons()
    {

		Transform temp = primaryWeaponAnchor;
        primaryWeaponAnchor = secondaryWeaponAnchor;
        secondaryWeaponAnchor = temp;

		if (primaryWeaponAnchor != null) {
            Weapon w = primaryWeaponAnchor.GetComponent<Weapon>();
            w.SetReserve(false, transform.position);
        }
        if (secondaryWeaponAnchor != null) {
            Weapon w = secondaryWeaponAnchor.GetComponent<Weapon>();
            w.SetReserve(true, transform.position);
        }
    }

    public void DepositWeapon() {
        Weapon w = primaryWeaponAnchor.GetComponent<Weapon>();
        w.isPickedUp = false;
        primaryWeaponAnchor = null;
    }
}
