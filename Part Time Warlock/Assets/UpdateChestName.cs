using InventoryPlus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateChestName : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public InputReader inputReader;
    private void Start()
    {
        this.gameObject.SetActive(false);

    }

    public void ShowChestObj(bool inChestRange)
    {
        if (inChestRange)
        {
            this.gameObject.SetActive(inChestRange);
        }
        
    }

    public void UpdateText(string objName)
    {
        tmp.text = objName;
    }
}
