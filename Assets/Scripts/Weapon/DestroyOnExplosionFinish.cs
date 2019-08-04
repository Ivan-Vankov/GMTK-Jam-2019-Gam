using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExplosionFinish : MonoBehaviour {

	public void SpawnExplosionCollider() {
		BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
		collider.isTrigger = true;
		gameObject.tag = "Bomb Explosion";
	}

	public void Selfdestruct() {
		Destroy(gameObject);
    }
}
