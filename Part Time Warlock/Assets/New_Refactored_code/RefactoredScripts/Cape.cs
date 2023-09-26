using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cape : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    public Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;


    // Start is called before the first frame update
    private void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
        
    }

    // Update is called once per frame
    private void Update()
    {


        segmentPoses[0] = targetDir.position; 

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed + i / trailSpeed);
        }
        lineRend.SetPositions(segmentPoses);
    }
}
