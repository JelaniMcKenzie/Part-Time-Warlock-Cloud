using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInputPrompt : MonoBehaviour
{
    [SerializeField] private GameObject inputPrompt;

    private void Start()
    {
        inputPrompt.SetActive(false);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.TryGetComponent<Player>(out var p))
        {
            inputPrompt.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var p))
        {
            inputPrompt.SetActive(false);
        }
    }
}
