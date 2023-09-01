using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonComponent : MonoBehaviour
{

    public void OnClick()
    {
        GameManager.Instance.Resume();
        SceneManager.LoadScene(1);
    }
}
