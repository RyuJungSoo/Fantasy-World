using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSizeUp : MonoBehaviour
{
    float time;
    public float speed=7;
    private float radius;
    public bool isBoss;

    private void Start()
    {
        if (isBoss == false)
            radius = GameObject.Find("Player").GetComponent<FreezeSkillComponent>().radius;
        else
            radius = GameObject.Find("Boss").GetComponent<BossAttack>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (1 * time);
        time += Time.deltaTime*speed;
        if (transform.localScale.x >= radius)
        {
            time = 0;
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}
