using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenuButtonComponent : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.Resume();
        SceneManager.LoadScene(0);
    }
}
