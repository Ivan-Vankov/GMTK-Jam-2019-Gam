using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffectPool : MonoBehaviour
{
    public GameObject EffectPrefab;
    public int PoolSize = 10;
    
    private List<GameObject> Effects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < PoolSize; i++) {
            GameObject go = Instantiate(EffectPrefab);
            go.SetActive(false);
            Effects.Add(go);
            go.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    public void Spawn(GameObject dying, Vector3 speed) {
        GameObject chosen = null;
        foreach (GameObject effect in Effects)
        {
            if (effect.activeSelf == false) {
                chosen = effect;
                break;
            }
        }
        if (chosen != null) {
            DeathEffect d = chosen.GetComponent<DeathEffect>();
            chosen.SetActive(true);
            chosen.transform.position = dying.transform.position;
            Rigidbody2D rb = dying.GetComponent<Rigidbody2D>();
            rb.AddForce(speed * 250);
            rb.drag = 4;
            d.Enable(dying);
        }
    }
}
