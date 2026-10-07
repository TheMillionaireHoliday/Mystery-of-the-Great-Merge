using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static EnemiesManager instance;

    public List<Transform> targetsList = new List<Transform>();

    void Awake()
    {
        if(instance == null) {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
