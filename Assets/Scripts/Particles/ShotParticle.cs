using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotParticle : MonoBehaviour
{
    float currentTime;
    ParticleSystem shotParticle;

    private void Awake()
    {
        shotParticle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        shotParticle.Play();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > shotParticle.main.duration)
        {
            ObjectPoolManager.Instance.ReturnObject(PoolType.ShotParticle, gameObject);
        }
    }

    private void OnDisable()
    {
        currentTime = 0;
        shotParticle?.Stop();
    }
}
