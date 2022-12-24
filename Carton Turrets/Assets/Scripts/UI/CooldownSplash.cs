using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSplash : MonoBehaviour
{
    [SerializeField]ParticleSystem _gatherRightPS;
    [SerializeField]ParticleSystem _gatherLeftPS;
    [SerializeField]ParticleSystem _impactUpPS;
    [SerializeField]ParticleSystem _impactDownPS;
    public void BlastOffEffect()
    {
        _gatherRightPS.Play();
        _gatherLeftPS.Play();
        _impactUpPS.Play();
        _impactDownPS.Play();
    }
}
