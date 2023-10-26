using System;
using UnityEngine;

namespace SaveLoadSystem
{
    [System.Serializable]
    public class SaveData
    {
        public int index = 1;
        [SerializeField] private float myFloat = 5.8f;
    }
}
