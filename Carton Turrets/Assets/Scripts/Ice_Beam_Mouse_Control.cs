using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Beam_Mouse_Control : MonoBehaviour
{
    [SerializeField] GameObject _endPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ParticleSystem[] beamFX;
 
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        EnableLaser();
        // RotateToMouse();
        lineRenderer.SetPosition(0, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaser();
    }

    private void DisableLaser()
    {
        lineRenderer.enabled = false;

        foreach(ParticleSystem effect in beamFX)
        {
            effect.Stop();
        }
    }

    private void UpdateLaser()
    {
        lineRenderer.SetPosition(1, _endPoint.transform.position);
    }

    private void EnableLaser()
    {
        lineRenderer.enabled = true;

        foreach (ParticleSystem effect in beamFX)
        {
            effect.Play();
        }
    }

    private void RotateToMouse()
    {
        Vector2 direction = _endPoint.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
    }
}
