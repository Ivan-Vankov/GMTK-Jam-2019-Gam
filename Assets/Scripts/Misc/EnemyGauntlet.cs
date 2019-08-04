using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RoomListener))]
public class EnemyGauntlet : MonoBehaviour
{
    public GameObject[] Doors;

    private RoomListener rl;

    private UnityAction CheckAction;
    private int count = 0;

    public UnityEvent OnFinished;
	
    void Start()
    {
        CheckAction += CheckEnemies;
        rl = GetComponent<RoomListener>();
        foreach(EnemyShell e in rl.Enemies){
            e.OnDeath.AddListener(CheckAction);
        }
        count = rl.Enemies.Count;
    }

    void CheckEnemies(){
        count -= 1;
        if (count == 0) {
            OnFinished.Invoke();
            rl.finished = true;
            foreach(GameObject door in Doors){
                Animator a = door.GetComponent<Animator>();
                a.SetBool("IsDead", true);
                BoxCollider2D b = door.GetComponent<BoxCollider2D>();
                b.enabled = false;
            }
        }
    }
}
