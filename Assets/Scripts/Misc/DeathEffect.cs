using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{

    private ParticleSystem ps;
    private Animator anim;
    private GameObject dying;

    public void Enable(GameObject dying) {
        if (ps == null) {
            ps = GetComponent<ParticleSystem>();
            anim = GetComponent<Animator>();
        }
        anim.Play("Death_Animate");
        ps.Play();
        this.dying = dying;
        StartCoroutine(Animate());
    }

    private void Update()
    {
        if (dying != null) {
            transform.position = dying.transform.position;
        }
    }

    private IEnumerator Animate() {
        yield return new WaitForSeconds(0.2f);
        Animator a = dying.GetComponent<Animator>();
        SpriteRenderer r = dying.GetComponent<SpriteRenderer>();
        a.speed = 0;
        float originalScale = dying.transform.localScale.y;
        while (dying.transform.localScale.y > 0) {
            Vector3 scale = dying.transform.localScale;
            scale.y -= Time.deltaTime * originalScale * 2;
            dying.transform.localScale = scale;
            Color c = r.color;
            c.a -= 0.1f;
            r.color = c;
            yield return new WaitForEndOfFrame();
        }
        dying.SetActive(false);
        gameObject.SetActive(false);
    }
}
