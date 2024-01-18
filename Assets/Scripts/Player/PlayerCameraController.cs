using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vCam;

    CinemachineFramingTransposer composer;

    [SerializeField] float waitTime;
    bool isLookDown;

    private void Awake()
    {
        composer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private IEnumerator Start()
    {
        composer.m_XDamping = composer.m_YDamping = composer.m_ZDamping = 0;

        float waitSec = 0.5f;
        yield return new WaitForSeconds(waitSec);

        composer.m_XDamping = composer.m_YDamping = composer.m_ZDamping = 1;
    }

    public void LookDown()
    {
        if(isLookDown == false)
        {
            isLookDown = true;
            composer.m_YDamping = 1;
            composer.m_TrackedObjectOffset.y = -5;
        }
    }

    public void LookUp()
    {
        isLookDown = false;
        composer.m_YDamping = 2;
        composer.m_TrackedObjectOffset.y = 1;
        
    }

}
