using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeLogic : MonoBehaviour
{
    public UIManager ui;
    GameObject audioObject;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        ui = FindAnyObjectByType<UIManager>();
        audioObject = GameObject.Find("AudioSource");

        if (audioObject != null )
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SlowDownTime());
        Debug.Log("slowed Time");
    }

    public IEnumerator SlowDownTime()
    {
        
        ui.timerSpeed = 0.5f;
        audioSource.pitch = 0.5f;
        yield return new WaitForSeconds(30f);
        ui.timerSpeed = 1f;
        audioSource.pitch = 1.02f;
        Destroy(this.gameObject);
    }
}
