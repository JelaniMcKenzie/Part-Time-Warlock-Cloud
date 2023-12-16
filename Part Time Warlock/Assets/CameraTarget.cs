using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    [SerializeField] private float threshold;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private float smoothTime = 0.3f;

    private void Start()
    {
        player = FindAnyObjectByType<Player>().transform;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        threshold = cam.orthographicSize / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (player.position + mousePos) / 2f;


        //Constrains the x coord of targetPos; it can't go below or above the threshold value
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

        this.transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

    }
}
