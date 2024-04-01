using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Player P;

    private void Start()
    {
        P = FindAnyObjectByType<Player>();
    }
    private void Update()
    {
        transform.position = new Vector3(P.transform.position.x, P.transform.position.y, -10f);
    }
}
