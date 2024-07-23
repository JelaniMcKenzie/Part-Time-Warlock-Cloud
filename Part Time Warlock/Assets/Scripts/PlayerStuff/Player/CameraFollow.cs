using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public WizardPlayer P;

    private void Start()
    {
        P = FindAnyObjectByType<WizardPlayer>();
    }
    private void Update()
    {
        transform.position = new Vector3(P.transform.position.x, P.transform.position.y, -10f);
    }
}
