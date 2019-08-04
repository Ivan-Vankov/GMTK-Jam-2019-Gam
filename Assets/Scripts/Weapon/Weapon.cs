using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Sprite weaponUp;
	public Sprite weaponUpRight;
	public Sprite weaponRight;
	public Sprite weaponDownRight;
    public Sprite weaponDown;
	public Sprite weaponDownLeft;
	public Sprite weaponLeft;
	public Sprite weaponUpLeft;

    protected SpriteRenderer weaponRenderer;


    [HideInInspector]
    public KeyCode attackKey = KeyCode.Mouse0;

    public float maxCooldown = 2;

    public bool useIndividualSprites;

    public GameObject holder;

    public bool isInReserve;

    private Weapon parent;

    [HideInInspector]
    public float cooldown = 0;

    public bool isPickedUp { get; set; } = false;

	public bool isAttacking { get; set; } = false;

    public void Start() {
        weaponRenderer = transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
    }

    public void SetReserve(bool inreserve, Vector3 position) {
        isInReserve = inreserve;
        gameObject.SetActive(!inreserve);
        if (inreserve) {
            GameManager.instance.InventoryDisplay.enabled = true;
            GameManager.instance.InventoryDisplay.sprite = weaponRenderer.sprite;
        } else
        {
            GameManager.instance.InventoryDisplay.enabled = false;
            transform.position = position;
            Update();
        }
    }

    public void Selfdestruct()
    {
        if (parent != null){
            parent.Selfdestruct();
        }
        Destroy(gameObject);
    }

    public void Update()
    {
        if (!isPickedUp)
        {
            return;
        }
        if (cooldown > 0){
            return;
        }

        if (isInReserve) {
            return;
        }

        if (holder.activeSelf == false) {
            Selfdestruct();
        }

        Vector2 lookDirection = Vector2.down;

        if (holder.tag == "Player") {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            lookDirection = new Vector2
            {
                x = mousePosition.x - transform.position.x,
                y = mousePosition.y - transform.position.y
            };
        } else {
            lookDirection = Vector2.MoveTowards(transform.right, GameManager.instance.Player.transform.position - transform.position, 0.04f);
        }

        if (useIndividualSprites){
            if (lookDirection.x < -1 && lookDirection.y < -1) {
                weaponRenderer.sprite = weaponUpLeft;
            } else if (lookDirection.x > -1 && lookDirection.x < 1 && lookDirection.y < -1) {
                weaponRenderer.sprite = weaponUp;
            }else if (lookDirection.x > 1 && lookDirection.y < -1) {
                weaponRenderer.sprite = weaponUpRight;
            }else if (lookDirection.x < -1 && lookDirection.y < 1 && lookDirection.y > -1) {
                weaponRenderer.sprite = weaponLeft;
            }else if (lookDirection.x > 1 && lookDirection.y < 1 && lookDirection.y < -1) {
                weaponRenderer.sprite = weaponRight;
            }else if (lookDirection.x < -1 && lookDirection.y > 1) {
                weaponRenderer.sprite = weaponDownLeft;
            }else if (lookDirection.x > -1 && lookDirection.x < 1 && lookDirection.y > 1) {
                weaponRenderer.sprite = weaponDown;
            }else if (lookDirection.x > 1 && lookDirection.y > 1) {
                weaponRenderer.sprite = weaponDownRight;
            }
        } else {
            transform.right = lookDirection;
        }
    }

    public virtual IEnumerator Attack() {
        yield return new WaitForEndOfFrame();
    }
}
