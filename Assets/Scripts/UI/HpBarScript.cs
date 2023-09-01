using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HpBarScript : MonoBehaviour
{
    public PoolManager poolManager;

    private float hpBar_width;
    private float hpBar_height;

    [SerializeField] GameObject m_goPrefab = null;

    List<Transform>[] m_objectList;
    List<GameObject>[] m_hpBarList;
    List<MonsterComponent>[] m_MonsterList;
    List<RectTransform>[] m_RectTransform;

    Camera m_cam = null;

    private void Awake()
    {
        
        m_objectList = new List<Transform>[poolManager.monsterPrefabs.Length];
        m_hpBarList = new List<GameObject>[poolManager.monsterPrefabs.Length];
        m_MonsterList = new List<MonsterComponent>[poolManager.monsterPrefabs.Length];
        m_RectTransform = new List<RectTransform>[poolManager.monsterPrefabs.Length];

        for (int i = 0; i < m_objectList.Length; i++)
        {
            m_objectList[i] = new List<Transform>();
        }

        for (int i = 0; i < m_hpBarList.Length; i++)
        {
            m_hpBarList[i] = new List<GameObject>();
        }

        for (int i = 0; i < m_MonsterList.Length; i++)
        {
            m_MonsterList[i] = new List<MonsterComponent>();
        }

        for (int i = 0; i < m_hpBarList.Length; i++)
        {
            m_RectTransform[i] = new List<RectTransform>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;
        hpBar_width = m_goPrefab.GetComponent<RectTransform>().sizeDelta.x;
        hpBar_height = m_goPrefab.GetComponent<RectTransform>().sizeDelta.y;


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < poolManager.monsterPrefabs.Length; i++)
        {
            for (int j = 0; j < m_objectList[i].Count; j++)
            {
                if (!m_objectList[i][j].gameObject.activeSelf)
                {

                    m_hpBarList[i][j].SetActive(false);
                }
                else
                {
                    m_hpBarList[i][j].transform.position = m_cam.WorldToScreenPoint(m_objectList[i][j].position + new Vector3(0, 0.5f, 0));
                    m_RectTransform[i][j].sizeDelta = new Vector2(hpBar_width * m_MonsterList[i][j].Hp / m_MonsterList[i][j].maxHp, hpBar_height);

                }
            }
        }
    }

    public void AddMonster(int index, GameObject monsterObj) // 새로운 몬스터 오브젝트가 추가될 때
    {
        m_objectList[index].Add(monsterObj.transform);
        GameObject t_hpbar = Instantiate(m_goPrefab, monsterObj.transform.position, Quaternion.identity, transform);
        t_hpbar.transform.SetAsFirstSibling(); // 몬스터 체력바가 플레이어 체력바 앞으로 오는 것을 막음.
        m_hpBarList[index].Add(t_hpbar);
        m_MonsterList[index].Add(monsterObj.GetComponent<MonsterComponent>());
        m_RectTransform[index].Add(t_hpbar.transform.GetChild(0).gameObject.GetComponent<RectTransform>());
    }

    public void ActiveMonster(int index, int monsterindex) // 꺼놓은 몬스터 오브젝트를 다시 킬 때
    {
        m_hpBarList[index][monsterindex].SetActive(true);
    }

}
