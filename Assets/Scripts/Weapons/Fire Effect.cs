using QFX.SFX;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireEffect : SFX_ControlledObject
{
    public GameObject LaunchParticleSystem;
    public Transform LaunchTransform;
    public SFX_LightAnimator LightAnimator;
    public GameObject Projectile;
    public float FireRate = 0.5f;
    private bool _isFireAllowed = true;
    private ParticleSystem _launchPs;

    public void GunFireEffect()
    {
        Setup();
        Run();
        StartCoroutine("WaitSFXEmmit");
    }

    public override void Setup()
    {
        base.Setup();

        var launchGo = Instantiate(LaunchParticleSystem, transform, true);
        launchGo.transform.rotation = LaunchTransform.rotation;
        _launchPs = launchGo.GetComponent<ParticleSystem>();
    }

    public override void Run()
    {
        base.Run();

        LightAnimator.Run();

        _launchPs.transform.position = LaunchTransform.position;
    }

    public override void Stop()
    {
        base.Stop();

        LightAnimator.Stop();
    }

    private void Update()
    {
        if (!IsRunning)
            return;

        if (_isFireAllowed)
            StartCoroutine("Fire");
    }

    private void OnDisable()
    {
        StopCoroutine("Fire");
    }

    private IEnumerator Fire()
    {
        _isFireAllowed = false;

        _launchPs.Play(true);

        Vector3 position;
        Quaternion rotation;

        if (LaunchTransform != null)
        {
            position = LaunchTransform.position;
            rotation = LaunchTransform.rotation;
        }
        else
        {
            position = transform.position;
            rotation = transform.rotation;
        }

        var go = Instantiate(Projectile, position, rotation);
        var emitterKeeper = go.GetComponent<SFX_IEmitterKeeper>();
        if (emitterKeeper != null)
            emitterKeeper.EmitterTransform = transform;

        LightAnimator.Run();

        yield return new WaitForSeconds(FireRate);

        _isFireAllowed = true;
    }

    private IEnumerator WaitSFXEmmit()
    {
        yield return new WaitForSeconds(FireRate);
        Stop();
    }
}
