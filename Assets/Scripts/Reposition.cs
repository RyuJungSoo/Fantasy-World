using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    public PlayerInput playerInput;

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || GameManager.Instance.isGameover == true)
        {
            return;
        }

        Vector3 playerPos = GameManager.Instance.playerTransform().position;
        Vector3 myPos = transform.position;

        switch (transform.tag){ 
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 60);
                }
                else if(diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 60);
                }
                break;
            case "Monster":
                if (collision.enabled && GetComponent<MonsterComponent>().isDead == false)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran+dist*2);
                }
                break;
        }
    }
}
