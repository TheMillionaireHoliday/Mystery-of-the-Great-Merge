using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Portal : MonoBehaviour
{
    private BoxCollider boxCollider;
    public Portal linkedPortal;
    public bool portalOn = false;
    //private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool pointTeleport = false;

    public AudioClip portalEnterSound;
    public AudioClip portalExitSound;

    public AudioSource audioSource;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //spriteRenderer.enabled = false;

        animator = GetComponentInChildren<Animator>();
    }

    public void TurnOnPortal()
    {
        portalOn = true;
        boxCollider.enabled = true;
        //spriteRenderer.enabled = true;

        animator.SetTrigger("Turn On");
    }
    public void TurnOffPortal()
    {
        portalOn = false;
        boxCollider.enabled = false;
        //CursorManager.instance.SetDefaultCursor();
        //spriteRenderer.enabled = false;

        animator.SetTrigger("Turn Off");
    }

    private void OnTriggerEnter(Collider other)
    {
        PortalTraveller traveller = other.GetComponent<PortalTraveller>();
        if (traveller == null) return;

        if (traveller.currentPortal != null) return;

        Teleport(traveller);
    }

    private void OnTriggerExit(Collider other)
    {
        PortalTraveller traveller = other.GetComponent<PortalTraveller>();
        if (traveller == null) return;

        if (traveller.currentPortal == this)
        {
            traveller.currentPortal = null;
        }
    }

    void Teleport(PortalTraveller traveller)
    {
        //if (animator.runtimeAnimatorController.name == "Teleporter Top")
        //    PlayRandomPitchOneShot(portalEnterSound);
        //else
        //    PlayRandomPitchOneShot(portalExitSound);

        if (Time.time - traveller.lastPortalTime > 3.0f)
            traveller.enterCount = 0;

        if (traveller.enterCount % 2 == 0) {
            PlayRandomPitchOneShot(portalEnterSound);
        }
        else
            PlayRandomPitchOneShot(portalExitSound);

        traveller.enterCount += 1;

        traveller.currentPortal = linkedPortal;
        traveller.lastPortalTime = Time.time;

        /*if (traveller.gameObject.GetComponent<Enemy>() != null) {
            var position = linkedPortal.transform.position;
            position.z = traveller.transform.position.z;

            traveller.transform.position = position;
        }
        else {
            var position = traveller.transform.position - transform.position + linkedPortal.transform.position;
            position.z = traveller.transform.position.z;

            traveller.transform.position = position;
        }
        */

        if (!pointTeleport)
        {
            var position = traveller.transform.position - transform.position + linkedPortal.transform.position;
            position.z = traveller.transform.position.z;

            traveller.transform.position = position;
        }
        else
        {
            var position = linkedPortal.transform.position;
            position.z = traveller.transform.position.z;

            traveller.transform.position = position;
        }
    }

    private void PlayRandomPitchOneShot(AudioClip clip)
    {
        float randomPitch = Random.Range(0.5f, 4f);
        audioSource.pitch = randomPitch;
        audioSource.clip = clip;
        audioSource.Play();

        audioSource.pitch = 1f;
    }
}
