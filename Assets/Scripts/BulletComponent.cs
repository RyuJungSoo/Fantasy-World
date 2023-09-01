using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour
{
    public bool isPlayer; // 플레이어 탄알 유무
    public bool isBoss; // 보스 탄막 유무
    public float damage;
    public int per; // 관통 변수

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigid.velocity = dir;
        }
    }

    public void RotationToShot(float ShootPower)
    {

        if (isBoss == false)
            return;
        Rigidbody2D BulletRig = GetComponent<Rigidbody2D>();

        this.per = 0;
        transform.parent = GameManager.Instance.pool.transform;
        BulletRig.bodyType = RigidbodyType2D.Dynamic;
        BulletRig.AddForce(transform.up * ShootPower, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision) // 플레이어 피격용
    {
        if (collision.CompareTag("Player") && isPlayer == false)
        {
            

            //Debug.Log("공격 당함");
            collision.GetComponent<PlayerComponent>().TakeDamage(damage);
            if (per == -100)
            {

                return;
            }

            per--;

            if (per < 0)
            {
                rigid.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }



        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;
        if(isBoss == true)
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.SetActive(false);
    }
}
