using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeLogic : MonoBehaviour
{
    public UIManager ui;
    // Start is called before the first frame update
    void Start()
    {
        ui = FindAnyObjectByType<UIManager>();
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
        yield return new WaitForSeconds(30f);
        ui.timerSpeed = 1f;
        Destroy(this.gameObject);
    }
}
