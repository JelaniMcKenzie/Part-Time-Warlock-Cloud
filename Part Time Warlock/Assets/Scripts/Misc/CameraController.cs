using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;

    Vector3 target, mousePos, refVel, shakeOffset;

    float cameraDistance = 3.5f;

    float smoothTime = 0.1f, zStart;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
        target = player.transform.position;
        zStart = transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();

        if (player.canMove)
        {
            UpdateCameraPosition();
        }
        
    }

    private Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = 0.9f;

        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }

        return ret;
    }

    private Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDistance;
        Vector3 ret = player.transform.position + mouseOffset;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }

    private void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }
}
