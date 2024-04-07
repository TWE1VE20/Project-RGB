using QFX.SFX;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SamplePlayerDetecter : SFX_ControlledObject
{
    public GameObject Weapon;
    public Transform WeaponPosition;

    public float AllowedTimeInProtectionRadius;

    public float LaserMaxDistance;
    public LayerMask LaserLayerMask;
    public float LaserContactOffset;
    public ParticleSystem LaserImpact;
    public LineRenderer LaserLine;
    public int LasersCount;

    public SFX_AnimationModule ForceOverLifeTime;
    public SFX_AnimationModule LaserRotationOverLifeTime;
    public SFX_AnimationModule LaserRadiusOverLifeTime;

    public SFX_ObjectFinder ObjectFinder;

    private float _enemyEnteredRadiusTime;

    private bool _isLasersActive;

    private float _forceTime;
    private float _radiusTime;
    private float _rotationTime;

    private GameObject _target;

    private readonly List<LaserInstance> _lineInstances = new List<LaserInstance>();

    private SFX_ControlledObject _weapon;



    private class LaserInstance
    {
        public float Angle;
        public ParticleSystem Impact;
        public LineRenderer LineRenderer;
        public Vector3 Center;
    }
}
