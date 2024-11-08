using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdiotSandwichLogic : MonoBehaviour
{
    public WizardPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = FindAnyObjectByType<WizardPlayer>();
        StartCoroutine(ReverseControls());
    }

    private IEnumerator ReverseControls()
    {
        player.controlsReversed = true;
        yield return new WaitForSeconds(20f);
    }
}
