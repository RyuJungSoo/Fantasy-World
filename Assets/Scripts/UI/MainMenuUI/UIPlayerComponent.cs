using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerComponent : MonoBehaviour
{
    public Sprite[] walkImages;
    public float speed = 170;
    private float startPoint_x;
    public Transform endPoint;
    private int index = 0;
    Image image;

    private float curTime;
    private float pauseTime;

    private void Start()
    {
        pauseTime = Time.deltaTime*4;
        startPoint_x = transform.position.x;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position.x);
        Move();
        if (endPoint.position.x < transform.position.x)
        {
            transform.position = new Vector3(startPoint_x, transform.position.y, transform.position.z);
        }
    }

    private void changeSprite()
    {
        if (curTime < 0)
        {
            index++;
            image.sprite = walkImages[index];
            curTime = pauseTime;
            if (index == walkImages.Length-1)
                index = 0;
        }
        else
            curTime -= Time.deltaTime;
    }

    private void Move()
    {
        changeSprite();
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
