using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    private float startingZ;

    private Transform player;

    public float acceleration = 10f;
    public float maxSpeed = 5f;

    private float direction = 0;
    private bool isChasing = false;

    private Rigidbody rb;

    [SerializeField] public bool isKnight = true; // for knights

    public GameObject explosionPrefab;

    void Start()
    {
        startingZ = transform.position.z;

        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        player = GameObject.Find("DefaultCharacter").transform;

        if(EnemiesManager.instance.targetsList.Count == 0) {
            EnemiesManager.instance.targetsList.Add(player);
        }

        EnemiesManager.instance.targetsList.Add(transform);
    }

    void FixedUpdate()
    {
        rb.position = new Vector3(rb.position.x, rb.position.y, startingZ);

        Transform currentTarget = null;
        float currentShortestDist = 100000f;

        foreach (Transform target in EnemiesManager.instance.targetsList)
        {
            if (target != null && target.gameObject.activeSelf && target != transform)
            {
                /*if (target.GetComponent<GroundedCharacterController>() != null) { // If sees player, chase only him

                    if (CheckLOS(target.transform)) {

                        // When found the target

                        currentTarget = target;
                        currentShortestDist = 0;

                        isChasing = true;
                        direction = Mathf.Sign(target.position.x - transform.position.x);

                        break;
                    }
                    else 
                        continue;
                } */

                if (target.GetComponent<Enemy>() != null) {
                    if (target.GetComponent<Enemy>().isKnight == isKnight) // Ignore targets that are their own faction
                        continue;
                }

                if (CheckLOS(target))
                {
                    float dist = (target.position - transform.position).magnitude;

                    if (dist < currentShortestDist)
                    {
                        // When found the target

                        currentTarget = target;
                        currentShortestDist = dist;

                        isChasing = true;
                        direction = Mathf.Sign(target.position.x - transform.position.x);
                    }
                }
            }
        }

        WallHitCheck();

        if (isChasing) {
            Vector3 force = new Vector3(direction * acceleration, 0f, 0f);
            rb.AddForce(force, ForceMode.Acceleration);

            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
            rb.velocity = velocity;
        }
        else
        {
            Vector3 velocity = rb.velocity;
            velocity.x = 0;
            rb.velocity = velocity;
        }
    }

    private bool CheckLOS(Transform target)
    {
        if (target == null) return false;

        Vector3 origin = transform.position;
        Vector3 direction = (target.position - origin).normalized;
        float distance = Vector3.Distance(origin, target.position);

        // Early exit if target is out of view distance
        if (distance > 10000f) return false;

        // Perform raycast against Default layer only
        if (Physics.Raycast(origin, direction, distance, 1 << 0))
        {
            // Something blocked the LOS
            return false;
        }
        
        return true;
    }
    private void WallHitCheck()
    {
        bool hitWall = Physics.Raycast(
            transform.position,
            new Vector3(direction, 0f, 0f),
            0.35f
        );

        if (hitWall)
        {
            isChasing = false;
            direction = 0f;

            print("hit a wall");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GroundedCharacterController>() != null)
        {
            collision.gameObject.SetActive(false); // Kill player
            direction = 0f;

            var pos = collision.transform.position;
            pos.z = -5f;

            Instantiate(explosionPrefab, pos, Quaternion.identity);

            CameraManager.instance.shakeParentAnimator.SetTrigger("Shake");

            StartCoroutine(ReloadLevel());
        }

        else if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            if (collision.gameObject.GetComponent<Enemy>().isKnight != isKnight)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);

                if (isKnight) {
                    Vector3 pos = transform.position + (collision.transform.position - transform.position) * 0.5f;
                    pos.z = -5f;

                    Instantiate(explosionPrefab, pos, Quaternion.identity);

                    CameraManager.instance.shakeParentAnimator.SetTrigger("Shake");
                }
            }
        }
    }

    private IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(1);
        
        LevelManager.Instance.ReloadCurrentLevel();
    }
}