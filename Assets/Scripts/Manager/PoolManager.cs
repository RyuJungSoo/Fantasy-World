using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수
    public GameObject[] monsterPrefabs;
    public GameObject[] itemPrefabs;
    public GameObject[] Prefabs;

    // 풀 담장을 하는 리스트들
    public List<GameObject>[] monsterPools;
    List<GameObject>[] itemPools;
    List<GameObject>[] Pools;

    // UI
    public GameObject canvas;
    private HpBarScript monsterHpUI;

    // 플레이어
    public GameObject Player;
    private PlayerComponent playerComponent;

    private void Awake()
    {
        monsterPools = new List<GameObject>[monsterPrefabs.Length];

        for (int i = 0; i < monsterPools.Length; i++)
        {
            monsterPools[i] = new List<GameObject>();
        }

        itemPools = new List<GameObject>[itemPrefabs.Length];

        for (int i = 0; i < itemPools.Length; i++)
        {
            itemPools[i] = new List<GameObject>();
        }

        Pools = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < Pools.Length; i++)
        {
            Pools[i] = new List<GameObject>();
        }
    }

    private void Start()
    {
        monsterHpUI = canvas.GetComponent<HpBarScript>();
        playerComponent = Player.GetComponent<PlayerComponent>();
    }

    public GameObject monsterGet(int index)
    {
        GameObject select = null;
        int curMonsterIndex = 0;
        // ... 선택한 풀의 놀고 (비활성화된) 있는 게임오브젝트 접근
     
        foreach (GameObject item in monsterPools[index])
        {
            if (!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                select.GetComponent<MonsterComponent>().Object_ON();
                monsterHpUI.ActiveMonster(index, curMonsterIndex);
                break;
            }
            curMonsterIndex++;
        }

        // ... 못 찾으면?
        if (select == null)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(monsterPrefabs[index], transform);
            monsterPools[index].Add(select);
            monsterHpUI.AddMonster(index,select);
            
        }

        return select;
    }

    public GameObject itemGet(int index, Transform pos)
    {
        GameObject select = null;
        int RandomPosition = Random.Range(0, 2);
        Vector3 RandomVector;

        if (RandomPosition == 0)
            RandomVector = new Vector3(1, 0);
        else
            RandomVector = new Vector3(-1, 0);

        // ... 선택한 풀의 놀고 (비활성화된) 있는 게임오브젝트 접근

        foreach (GameObject item in itemPools[index])
        {
            if (!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);

                if(index == 0 || index == 5)
                    select.transform.position = pos.position;
                else
                    select.transform.position = pos.position + RandomVector;
                break;
            }
        }

        // ... 못 찾으면?
        if (select == null)
        {
            // 새롭게 생성하고 select 변수에 할당
            if (index == 0)
                select = Instantiate(itemPrefabs[index], pos.position,Quaternion.identity,transform);
            else
                select = Instantiate(itemPrefabs[index], pos.position + RandomVector, Quaternion.identity, transform);
            itemPools[index].Add(select);
            
        }

        return select;
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 (비활성화된) 있는 게임오브젝트 접근

        foreach (GameObject item in Pools[index])
        {
            if (!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾으면?
        if (select == null)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(Prefabs[index], transform);
            Pools[index].Add(select);

        }

        return select;
    }

}
