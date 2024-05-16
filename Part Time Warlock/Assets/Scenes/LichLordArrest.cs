using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LichLordArrest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneOnWait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator LoadSceneOnWait()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Title");
    }
}
