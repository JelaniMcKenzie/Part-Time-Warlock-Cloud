using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    //This is readonly because we DON'T want to change the ID of each chest.
    //It needs to be generated once and stay ghere
    [ReadOnly][SerializeField] private string _id = Guid.NewGuid().ToString();
    [SerializeField] private static SerializableDictionary<string, GameObject> idDatabase = new SerializableDictionary<string, GameObject>();

    public string ID => _id;

    private void Awake()
    {
        if (idDatabase == null)
        {
            idDatabase = new SerializableDictionary<string, GameObject>();
        }

        if (idDatabase.ContainsKey(_id))
        {
            Generate();
        }
        else
        {
            idDatabase.Add(_id, this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(_id)) 
        { 
            idDatabase.Remove(_id);
        }
    }

    private void Generate()
    {
        //Makes sure no IDs are identical, and that they reference the proper GameObjects
        _id = Guid.NewGuid().ToString();
        idDatabase.Add(_id, this.gameObject);
        Debug.Log(idDatabase.Count);
    }
}
