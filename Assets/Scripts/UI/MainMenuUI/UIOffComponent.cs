using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class UIOffComponent : MonoBehaviour
{
    public GameObject UI;


    public void OnClick()
    {
        UI.active = false;
        UI.GetComponent<MainMenuUIController>().PageReset();
    }
}
