using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public static GameManager instance;
    public GameObject GameOverScreen;

    public GameObject InventoryUI;
    public Image InventoryDisplay;

    [SerializeField]
    private DeathEffectPool DeathEffects;

    [HideInInspector]
    public GameObject Player;
    [HideInInspector]
    public bool GameOverState = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) {
            instance = this;
            Init();
            DontDestroyOnLoad(this);
            return;
        } else {
            Destroy(this);
        }
    }

    public void SpawnDeathEffect(GameObject go, Vector3 speed) {
        DeathEffects.Spawn(go, speed);
    }

    public void SpawnDeathEffect(GameObject go) {
        DeathEffects.Spawn(go, Vector3.zero);
    }

    private void Init()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        DeathEffects = GetComponentInChildren<DeathEffectPool>();
    }

    public void GameOver() {
        GameOverScreen.SetActive(true);
        GameOverState = true;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
