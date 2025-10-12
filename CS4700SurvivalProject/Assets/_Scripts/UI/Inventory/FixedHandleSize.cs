using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedHandleSize : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;

    [SerializeField] private float size;
    void Update()
    {
        ResetSize();
    }

    public void ResetSize()
    {
        scrollBar.size = size;
    }
}
