using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    // Start is called before the first frame update

    public int counter = 0;
    public float timeElapsed = 0f;

    public List<Door> linkedDoors = new List<Door>();

    private bool unlocked = false;
    public TextMeshProUGUI associatedText = null;

    public AudioSource bigDoorSound;

    private void Update()
    {
        if (unlocked)
            return;

        if (counter >= 4) {
            timeElapsed += Time.deltaTime;
        }

        else timeElapsed = 0;

        if(timeElapsed >= 1f) {
            OpenDoors();
            unlocked = true;
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in linkedDoors) {
            door.ToggleDoor();
        }

        bigDoorSound.Play();
    }

    public void Increment()
    {
        counter++;
        associatedText.text = counter + "/4";
    }

    public void Decrement()
    {
        counter--;
        associatedText.text = counter + " /4";
    }
}
