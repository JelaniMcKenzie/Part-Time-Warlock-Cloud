using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningChain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("velocity = " + other.GetComponent<Rigidbody>().velocity);
        /*IEnumerator stunned()
        {

                //get the current velocity and store it somewhere
                //change the speed to 0
                //restore the previous speed of the object
            //other.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            yield return new WaitForSeconds(3); 
        }*/
        if (other.CompareTag("Enemy"))
        {
            //collide with enemy
            //stun enemy for 3 seconds
                    //StartCoroutine(stunned());
                    //Debug.Log("stunned");
            //Animation?
            //Two sprite renderers? one for the enemy and one for a lightning effect?
            //Ask Jackson. He had the same idea with the ice tome.
            //find nearest enemy within range x
            //instantiate lightning sprite that connects enemy 1 to enemy 2
            //rinse repeat for 3 enemies
            //accomplish through foreach loop?
        }
    }
}
