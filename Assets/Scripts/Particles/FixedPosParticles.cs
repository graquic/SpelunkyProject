using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPosParticles : MonoBehaviour
{
    protected ParticleSystem hitParticle;

    protected float currentWaitTime;

    protected PoolType thisPoolType;

    protected virtual void Awake()
    {
        hitParticle = GetComponent<ParticleSystem>();

    }
    protected virtual void OnEnable()
    {
        hitParticle.Play();
        SetDirection();
    }

    protected virtual void Update()
    {
        currentWaitTime += Time.deltaTime;

        if (currentWaitTime > hitParticle.main.duration + 0.5f)
        {
            ObjectPoolManager.Instance.ReturnObject(thisPoolType, gameObject);
        }
    }

    protected virtual void OnDisable()
    {
        currentWaitTime = 0;
        hitParticle.Stop();
    }

    protected void SetDirection()
    {
        float dir = GameManager.Instance.player.transform.localScale.x;

        transform.localScale = new Vector3(dir * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
