using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] mapPrefab = null;
    public GameObject player = null;
    string checkMapPos = "";
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, transform.position, Quaternion.identity);
        for (int i = 0; i < 5; i++)
        {
            Vector3 pos = new Vector3();

            for (int j = 0; j < 20; j++)
            {
                
                //Generate a random number
                //based of the random number it will randomly move pos up, down, left, or right
                Vector3 tempPos = pos;
                string tempPosString = "*" + pos + "*";
                
                
                int safety = 0;
                //while loop to check if a room already exists at the tempPosString's coordinates
                while (checkMapPos.Contains(tempPosString) && safety < 100)
                {
                    
                    pos = tempPos;

                    //Generate a random number
                    //based of the random number it will randomly move pos (a clone of the room) up, down, left, or right
                    //of the room generated before it
                    int posMover = Random.Range(0, 3);

                    if (posMover == 0)
                    {
                        pos += Vector3.up * 10; //change the 10 value depending on the spacing of the rooms.
                                                //Get the spacing value by layering 2 rooms on top of each other,
                                                //and then move one room directly to the right of it so the doorways ate touching
                                                //If you make new rooms and there's a different spacing value (e.g. 20, put that there)
                    } 
                    else if (posMover == 1) {
                        pos += Vector3.down * 10;
                    } 
                    else if (posMover == 2)
                    {
                        pos += Vector3.left * 10;
                    } 
                    else
                    {
                        pos += Vector3.right * 10;
                    }

                    tempPosString = "*" + pos + "*";

                    //automatically stops the while loop after 100 iterations
                    safety++;
                }
                
                //If a room doesn't exist at the tempPos coordinates, generate a new room
                if (!checkMapPos.Contains(tempPosString))
                {
                    GameObject tempMap = Instantiate(mapPrefab[Random.Range(0, mapPrefab.Length)]);
                    tempMap.transform.position = pos;
                }
               
                checkMapPos += tempPosString;
            }
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/
}
