using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class loadSceneButton : MonoBehaviour
{
    public void LoadScene(string input)
    {
        SceneManager.LoadScene(input);
    }
}
