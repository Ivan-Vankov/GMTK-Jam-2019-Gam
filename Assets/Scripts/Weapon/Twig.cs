using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twig : Weapon
{
    private Transform twigSprite;
    private void Start()
    {
        base.Start();
        twigSprite = transform.GetChild(0);
    }

    void Update()
    {
        base.Update();
        if (!isPickedUp)
        {
            return;
        }

        if (isInReserve)
        {
            return;
        }

        if (Input.GetKeyDown(attackKey) && holder == GameManager.instance.Player && cooldown <= 0)
        {
            StartCoroutine(Attack());
        }
    }

    public IEnumerator Attack()
    {
        cooldown = maxCooldown;
        isAttacking = true;
        Vector3 pos = twigSprite.transform.localPosition;
        for (int i=1; i <= 4; i++) {
            pos.x += 0.1f;
            twigSprite.transform.localPosition = pos;
            yield return new WaitForEndOfFrame();
        }
        isAttacking = false;
        for (int i = 1; i <= 4; i++)
        {
            pos.x -= 0.1f;
            twigSprite.transform.localPosition = pos;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        cooldown = 0;
    }
}
