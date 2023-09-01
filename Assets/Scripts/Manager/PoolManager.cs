using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // �����յ��� ������ ����
    public GameObject[] monsterPrefabs;
    public GameObject[] itemPrefabs;
    public GameObject[] Prefabs;

    // Ǯ ������ �ϴ� ����Ʈ��
    public List<GameObject>[] monsterPools;
    List<GameObject>[] itemPools;
    List<GameObject>[] Pools;

    // UI
    public GameObject canvas;
    private HpBarScript monsterHpUI;

    // �÷��̾�
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
        // ... ������ Ǯ�� ��� (��Ȱ��ȭ��) �ִ� ���ӿ�����Ʈ ����
     
        foreach (GameObject item in monsterPools[index])
        {
            if (!item.activeSelf)
            {
                // ... �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                select.GetComponent<MonsterComponent>().Object_ON();
                monsterHpUI.ActiveMonster(index, curMonsterIndex);
                break;
            }
            curMonsterIndex++;
        }

        // ... �� ã����?
        if (select == null)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
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

        // ... ������ Ǯ�� ��� (��Ȱ��ȭ��) �ִ� ���ӿ�����Ʈ ����

        foreach (GameObject item in itemPools[index])
        {
            if (!item.activeSelf)
            {
                // ... �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);

                if(index == 0 || index == 5)
                    select.transform.position = pos.position;
                else
                    select.transform.position = pos.position + RandomVector;
                break;
            }
        }

        // ... �� ã����?
        if (select == null)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
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

        // ... ������ Ǯ�� ��� (��Ȱ��ȭ��) �ִ� ���ӿ�����Ʈ ����

        foreach (GameObject item in Pools[index])
        {
            if (!item.activeSelf)
            {
                // ... �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... �� ã����?
        if (select == null)
        {
            // ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(Prefabs[index], transform);
            Pools[index].Add(select);

        }

        return select;
    }

}
