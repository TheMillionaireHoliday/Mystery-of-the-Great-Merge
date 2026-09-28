using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFrame : MonoBehaviour
{
    private Portal sourcePortal;
    private void Start()
    {
        sourcePortal = GetComponentInChildren<Portal>();
    }

    public AudioClip portalTurnOn;
    public AudioClip portalTurnOff;

    public AudioSource audioSource;

    private void OnMouseEnter()
    {
        //if (sourcePortal.portalOn)
        //    return;

        if(!sourcePortal.portalOn)
            CursorManager.Instance.SetHoverCursor();
        else
            CursorManager.Instance.SetCancelCursor();
    }

    private void OnMouseExit()
    {
        CursorManager.Instance.SetDefaultCursor();
    }

    private void OnMouseUpAsButton()
    {
        if (sourcePortal.portalOn) {
            sourcePortal.TurnOffPortal();
            sourcePortal.linkedPortal.TurnOffPortal();
            CursorManager.Instance.SetHoverCursor();
            audioSource.PlayOneShot(portalTurnOff);
        }
        else {
            sourcePortal.TurnOnPortal();
            sourcePortal.linkedPortal.TurnOnPortal();
            CursorManager.Instance.SetCancelCursor();
            audioSource.PlayOneShot(portalTurnOn);
        }
    }
}