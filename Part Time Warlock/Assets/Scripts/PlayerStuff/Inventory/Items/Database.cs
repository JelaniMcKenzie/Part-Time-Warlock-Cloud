using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<ItemClass> _itemDatabase;

    public List<ItemClass> ItemDatabase => _itemDatabase;

    [ContextMenu("Set IDs")]
    public void SetItemIDs()
    {
        if (_itemDatabase == null) _itemDatabase = new List<ItemClass>();

        var foundItems = Resources.LoadAll<ItemClass>("ItemData").ToList();

        foreach (var item in foundItems)
        {
            if (!_itemDatabase.Contains(item))
            {
                _itemDatabase.Add(item);
                Debug.Log($"{item.itemName} found in project but was not in database. Adding to database");
                Debug.Log($"{item.itemName} found in project but was not in database. Adding to database");
            }
            else
            {
                Debug.Log($"{item.itemName} already in database");
            }
        }

        Debug.Log($"Setting item IDs for {_itemDatabase.Count}");
        
        for (int i = 0; i < _itemDatabase.Count; i++)
        {
            _itemDatabase[i].ID = i;
        }
    }

    public ItemClass GetItem(int id)
    {
        return _itemDatabase.Find(i => i.ID == id);
    }
    
    public ItemClass GetItem(string displayName)
    {
        return _itemDatabase.Find(i => i.itemName == displayName);
    }
}
