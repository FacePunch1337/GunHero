using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Outline outline;
    public void ActivateOutline(bool state)
    {
        if (outline != null)
        {
            outline.enabled = state;
        }
    }
}
