using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Animator animator;
    public List<Door> linkedDoors;

    public bool onlyOnce = false;
    private bool activatedOnce = false;

    public Counter associatedCounter = null;

    public bool isPressed = false;

    public AudioSource buttonSource;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(activatedOnce && onlyOnce)
            return;

        if (isPressed)
            return;

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            animator.SetTrigger("Button Pressed");
        }

        buttonSource.Play();
        isPressed = true;

        if (associatedCounter != null)
        {
            associatedCounter.Increment();
            return;
        }

        activatedOnce = true;

        foreach (Door door in linkedDoors) {
            door.ToggleDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onlyOnce)
            return;

        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            animator.SetTrigger("Button Depressed");
        }

        buttonSource.Play();
        isPressed = false;

        if (associatedCounter != null)
        {
            associatedCounter.Decrement();
            return;
        }

        foreach (Door door in linkedDoors) {
            door.ToggleDoor();
        }

    }
}
