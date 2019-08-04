using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
	private float windupDuration = 0.025f;
	private float windupSpeed = 2000f;
	private float attackDuration = 0.1f;
	private float attackSpeed = 1000f;

	private SpriteRenderer slashEffectRenderer;

	[SerializeField] private Sprite[] slashSprites;

	private void Start() {
        base.Start();
		slashEffectRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
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
			StartCoroutine(Attack());
		}
    }

	public IEnumerator Attack()
    {
        cooldown = maxCooldown;
        isAttacking = true;
        StartCoroutine(SlashEffect());
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
        cooldown = 0;
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
