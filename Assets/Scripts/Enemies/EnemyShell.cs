using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyShell : Movement
{
    [HideInInspector]
    public bool active = false;
    public UnityAction SetActiveAction;
    public UnityAction SetInactiveAction;

    public UnityEvent OnDeath;

    public GameObject drop;

    public virtual void Die(GameObject culprit) {

        AudioManager.instance.PlayBigDeathSound();
        OnDeath.Invoke();
        active = false;
        if (drop != null)
        {
            GameObject dropObject = Instantiate(drop, transform.position, transform.rotation);
            dropObject.transform.position = transform.position + Vector3.left;
        }

        GameManager.instance.SpawnDeathEffect(gameObject, (transform.position - culprit.transform.position).normalized);
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Weapon") && collision.name.Equals("Shield")) {
			Shield shield = collision.GetComponentInParent<Shield>();
			shield.holder.GetComponent<Movement>().iFrames = 2;
			shield.Selfdestruct();
			StartCoroutine(Stun(2));
		}
	}

	private IEnumerator Stun(float duration) {
		isStunned = true;
		animator.SetInteger("HorizontalSpeed", 0);
		animator.SetInteger("VerticalSpeed", 0);
		yield return new WaitForSeconds(duration);
		isStunned = false;
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
		if (!active) { return; }

		print("here");
		if (collision.CompareTag("Bomb Explosion")) {
			print("here");
			Die(gameObject);
		}

		if (collision.CompareTag("Player Hurtbox")) {
            Movement p = collision.GetComponentInParent<Movement>();
            if (p.iFrames <= 0){
                p.Hurt();
            }
        }

        if (collision.CompareTag("Weapon"))
        {
            // Get WeaponAttack from Weapon Anchor
            Weapon weapon = collision.GetComponentInParent<Weapon>();

            if (weapon.cooldown > 0 && weapon.holder.tag == "Player" && weapon.isAttacking)
            {
                weapon.Selfdestruct();
                Die(collision.gameObject);
            }
        }
        else if (collision.CompareTag("Arrow"))
        {

            Arrow arrow = collision.GetComponent<Arrow>();

            if (arrow.holder.tag == "Player")
            {
                arrow.Selfdestruct();
                Die(collision.gameObject);
            }
        }
    }

    public void Start()
    {
        SetActiveAction += SetActive;
        SetInactiveAction += SetInactive;
    }

    public void SetActive(){
        active = true;
    }

    public void SetInactive()
    {
        active = false;
    }
}
