using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public Portal currentPortal = null;
    public float lastPortalTime = -1f;
    public int enterCount = 0;
    private void Update()
    {
        if(currentPortal != null && !currentPortal.portalOn)
        {
            currentPortal = null;
        }
    }
}