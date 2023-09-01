using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonComponent : MonoBehaviour
{
    public void OnClick()
    {
        
        SceneManager.LoadScene(0);
    }
}
