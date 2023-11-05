using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelector : MonoBehaviour
{
    [SerializeField, Min(0)] private int _maxIndexSize = 9;
    
    private int _currentIndex = 0;

    private void ChangeIndex(int direction)
    {
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize) _currentIndex = 0;
        if (_currentIndex < 0) _currentIndex = _maxIndexSize;
        
        
    }
}
