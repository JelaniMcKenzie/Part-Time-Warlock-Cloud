using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossMusic : MonoBehaviour
{
    public AudioClip bossMusic;
    public AudioClip defaultMusic;
    public AudioSource audioSource;

    public UIManager manager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        defaultMusic = audioSource.clip;
        manager = FindAnyObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.bossHealthBar.gameObject.SetActive(true);
            audioSource.clip = bossMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        
    }
}
