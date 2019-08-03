using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponAnchor))]
public class WeaponAttack : MonoBehaviour {

	private KeyCode attackKey = KeyCode.Mouse0;
	public bool isAttacking = false;

	private float windupDuration = 0.025f;
	private float windupSpeed = 2000f;
	private float attackDuration = 0.1f;
	private float attackSpeed = 1000f;

	private SpriteRenderer slashEffectRenderer;

	[SerializeField] private Sprite[] slashSprites;

	private void Start() {
		slashEffectRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
	}

	void Update() {
        if (Input.GetKeyDown(attackKey) && !isAttacking) {
			isAttacking = true;
			StartCoroutine(Attack());
			StartCoroutine(SlashEffect());
		}
    }

	private IEnumerator Attack() {
		float windupEnd = Time.time + windupDuration;
		while (Time.time < windupEnd) {
			transform.Rotate(0, 0, windupSpeed * Time.deltaTime);
			yield return null;
		}

		float attackEnd = Time.time + attackDuration;
		while (Time.time < attackEnd) {
			transform.Rotate(0, 0, -attackSpeed * Time.deltaTime);
			yield return null;
		}

		isAttacking = false;
	}

	private IEnumerator SlashEffect() {

		if (slashSprites.Length == 0) {
			yield break;
		}

		float totalDuration = 2 * (windupDuration + attackDuration);
		float attackEnd = Time.time + totalDuration;

		// Initial wait
		yield return new WaitForSeconds(totalDuration / slashSprites.Length);

		int slashSpriteIndex = 0;

		while (slashSpriteIndex < slashSprites.Length) {
			slashEffectRenderer.sprite = slashSprites[slashSpriteIndex++];
			yield return new WaitForSeconds(totalDuration / slashSprites.Length / 2);
		}

		slashEffectRenderer.sprite = null;
	}
}
