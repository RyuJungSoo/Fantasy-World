using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterComponent : MonoBehaviour
{
    // 파이어볼 프리팹 관련
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    private FireSkillComponent fireSkillComponent;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        fireSkillComponent = this.GetComponentInParent<FireSkillComponent>();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (id)
        {
            case 0: // 플레이어 탄막 회전
                if(GameManager.Instance.playerTransform().localScale.x > 0)
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                else if(GameManager.Instance.playerTransform().localScale.x < 0)
                    transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case 1: // 보스 탄막 회전
                if (gameObject.GetComponentInParent<MonsterComponent>().isFreeze == true)
                    return;
                if (transform.parent.transform.localScale.x > 0)
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                else if (transform.parent.transform.localScale.x < 0)
                    transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            default:
                break;
        }

    }

    public void LevelUp(float damage, int count)
    {
        if (this.count < 10)
        {
            this.damage += damage;
            this.count += count;

            if (id == 0)
            {
                BulletSetting();
            }
        }
    }

    public void Init()
    {
        switch (id)
        {
            case 0: // 플레이어 center
                speed = 150;
                BulletSetting();
                break;
            case 1: // 보스 center
                speed = 160;
                BulletSetting_Boss();
                break;
            default:
                break;
        }

    }
    void BulletSetting()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
                
            }
            else
            {
                bullet  = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 2, Space.World);
            bullet.GetComponent<BulletComponent>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per

        }
    }

    void BulletSetting_Boss()
    {

        for (int index = 0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.Instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 3f, Space.World);
            bullet.GetComponent<BulletComponent>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per

        }
    }
}
