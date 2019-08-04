using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{

    public GameObject player;

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D other)
    {
        if (GameManager.instance.GameOverState) {
            return;
        }
        if (other.gameObject == player && player.activeSelf){

            Vector3 dir = Vector3.zero;

            Vector3 distance = player.transform.position - transform.position;
            if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y)){
                dir.x = Mathf.Sign(distance.x);
            } else {
                dir.y = Mathf.Sign(distance.y);
            }

            StartCoroutine(Transition(dir));
        }
        if (other.gameObject.tag == "Room")
        {
            RoomListener rl = other.gameObject.GetComponent<RoomListener>();
            if (!rl.finished)
            {
                rl.OnDeactivate.Invoke();
                rl.active = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Room") {
            RoomListener rl = collision.gameObject.GetComponent<RoomListener>();
            if (!rl.finished)
            {
                rl.active = true;
                rl.OnActivate.Invoke();
            }
        }
    }

    private IEnumerator Transition(Vector3 direction){
        Time.timeScale = 0;
        float height = Camera.main.orthographicSize * 2;
        float width = height * Camera.main.aspect + 0.215f;
        direction = direction * new Vector2(width, height);
        //yield return new WaitForSecondsRealtime(0.5f);
        for (int i=0; i < 50; i++){
            Vector3 pos = transform.position;
            pos.x += direction.x * 0.02f;
            pos.y += direction.y * 0.02f;
            transform.position = pos;

            Vector3 ppos = player.transform.position;
            ppos.x += direction.normalized.x * 0.04f;
            ppos.y += direction.normalized.y * 0.04f;
            player.transform.position = ppos;

            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = 1;
    }
}
