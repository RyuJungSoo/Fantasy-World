using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InFoComponent : MonoBehaviour
{
    public enum InfoType {Time, Kill}
    public InfoType type;
    private TextMeshProUGUI my_Text;

    void Start()
    {
        my_Text = GetComponent<TextMeshProUGUI>();   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Time:
                float Time = GameManager.Instance.GameTime;
                int min = Mathf.FloorToInt(Time / 60);
                int sec = Mathf.FloorToInt(Time % 60);
                my_Text.text = string.Format("{0:D2} : {1:D2}", min, sec);
                break;
            case InfoType.Kill:
                my_Text.text = string.Format("{0:F0}", GameManager.Instance.kill);
                break;
        }
    }
}
