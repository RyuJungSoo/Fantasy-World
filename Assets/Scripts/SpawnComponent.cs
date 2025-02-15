using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    public Transform[] spawnPoint;

    private float curTime;
    public float SpawnCoolTime = 3f; // 스폰 쿨타임
    private int SpawnLevel;
    public int BossSpawnLevel;
    public float LevelChangeSec = 60f; // 몇초 주기로 스폰 레벨을 바꿀 것인지
    public float speed = 1.5f;
    public GameObject Boss;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 interV = GameManager.Instance.playerTransform().position - transform.position;
        transform.Translate(interV.normalized*speed*Time.deltaTime);

        if (GameManager.Instance.isGameover == true)
            return;

        if (curTime <= 0)
        {
            SpawnLevel = Mathf.FloorToInt(GameManager.Instance.GameTime / LevelChangeSec);

            if (BossSpawnLevel == SpawnLevel && GameManager.Instance.isBossStart == false)
            {
                StartBoss();
            }
            else
            {
                if (SpawnLevel >= GameManager.Instance.pool.monsterPrefabs.Length)
                    SpawnLevel = GameManager.Instance.pool.monsterPrefabs.Length - 1;
                Spawn(Random.Range(0, SpawnLevel + 1));
            }
            curTime = SpawnCoolTime;
        }

        else
            curTime -= Time.deltaTime;
    }

    void Spawn(int SpawnIndex)
    {
        if (GameManager.Instance.isGameover == false)
        {
            GameObject enemy = GameManager.Instance.pool.monsterGet(SpawnIndex);
            enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 자식 오브젝트만 선택되도록 랜덤 시작은 1부터
        }
    }

    void StartBoss()
    {
        Boss.active = true;
        GameManager.Instance.ChangeBGM(0, 1);
        UIManager.Instance.BossUI_ON();
        Boss.transform.position = GameManager.Instance.playerTransform().position + new Vector3(0, 5f);
        GameManager.Instance.Stop();
       
        Debug.Log("보스 시작");
    }
}
