using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera startCamera;
    private CinemachineVirtualCamera targetCamera;
    private CinemachineVirtualCamera endingCamera;

    [Header("Timing")]
    public float holdStartCameraTime = 1.5f;
    public float endingCameraHold = 1.5f;
    public float transitionSpeed = 1.5f;

    public static CameraManager instance;
    public Animator shakeParentAnimator;

    void Start()
    {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }

        shakeParentAnimator = GameObject.Find("Shake Parents").GetComponent<Animator>();

        startCamera = GameObject.Find("Starting Camera").GetComponent<CinemachineVirtualCamera>();
        targetCamera = GameObject.Find("Normal Camera").GetComponent<CinemachineVirtualCamera>();
        endingCamera = GameObject.Find("Ending Camera").GetComponent<CinemachineVirtualCamera>();

        var brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_IgnoreTimeScale = true;
        
        var blend = brain.m_DefaultBlend;
        blend.m_Time = transitionSpeed;

        brain.m_DefaultBlend = blend;

        // Ensure start camera is active first
        startCamera.Priority = 20;
        targetCamera.Priority = 10;

        // Begin transition sequence
        StartCoroutine(SwitchCameraRoutine());
    }



    private IEnumerator SwitchCameraRoutine()
    {
        // Hold initial camera for a moment
        yield return new WaitForSeconds(holdStartCameraTime);

        // Switch priorities (CinemachineBrain handles blending automatically)
        startCamera.Priority = 10;
        targetCamera.Priority = 20;
    }

    public IEnumerator EndLevelCamera()
    {
        yield return new WaitForSecondsRealtime(holdStartCameraTime);

        endingCamera.Priority = 30;

        yield return new WaitForSecondsRealtime(endingCameraHold + transitionSpeed);
    }
}