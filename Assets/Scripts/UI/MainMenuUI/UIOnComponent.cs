using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnComponent : MonoBehaviour
{
    public GameObject UI;


    public void OnClick()
    {
        UI.active = true;
    }
}
