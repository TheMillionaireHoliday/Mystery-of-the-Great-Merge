using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<GroundedCharacterController>() != null)
        {
            //GetComponent<AudioSource>().Play();
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
