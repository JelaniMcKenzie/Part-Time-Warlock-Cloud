using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    public Image itemSprite;
    public Text itemCount;

    private void Awake()
    {
        itemSprite.color = Color.clear;
        itemCount.text = string.Empty;
    }
}