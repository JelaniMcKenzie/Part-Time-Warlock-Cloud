using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
    AudioSource source;
    bool isPlaying;
    bool toggleChange;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isPlaying == true)
        {
            isPlaying = false;
            source.Pause();
        }

        else if (Input.GetKeyDown(KeyCode.Return) && isPlaying == false)
        {
            isPlaying = true;
            source.Play();
        }
    }
}
