using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbArrows : MonoBehaviour{

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Arrow")) {
			collision.GetComponent<Arrow>().Selfdestruct();
		}
	}
}
