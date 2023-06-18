using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HandParent : MonoBehaviour
{
    public Player P = null;
    public GameObject crosshair;
    // Start is called before the first frame update
    void Start()
    {
        P = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (P.canMove == true)
        {
            transform.localPosition = Vector3.MoveTowards(new Vector3(), crosshair.transform.position, 0.1f);
            Vector3 pos = transform.localPosition;
            pos.x = Mathf.Min(0.1f, Mathf.Max(-0.1f, pos.x));
            pos.y = Mathf.Min(0.1f, Mathf.Max(-0.1f, pos.y)) - 0.3f;
            transform.localPosition = pos;
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            
        }
    }
}
