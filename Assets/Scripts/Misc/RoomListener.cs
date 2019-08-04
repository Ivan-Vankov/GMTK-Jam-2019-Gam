using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomListener : MonoBehaviour
{
    public bool finished = false;
    public bool active = false;
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    public List<EnemyShell> Enemies;

    private void Start()
    {
        foreach (EnemyShell e in Enemies) {
            OnActivate.AddListener(e.SetActiveAction);
            OnDeactivate.AddListener(e.SetInactiveAction);
            if (active){
                e.SetActive();
            }
        }
    }
}
