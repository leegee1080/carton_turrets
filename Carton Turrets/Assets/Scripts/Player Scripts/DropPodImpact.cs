using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IDroppodControllable
{
    virtual void Impact()
    {

    }
    virtual void CompleteLanding()
    {

    }
}

public class DropPodImpact : MonoBehaviour
{

    [SerializeField]IDroppodControllable _dpController; 
    [SerializeField]GameObject _dpControllerObject; 
    [SerializeField]ParticleSystem _nukePS;
    [SerializeField]float _playerSpawnTime;
    [SerializeField]float _nukeCompleteTime;
    [SerializeField] GameObject _rubble;

    private void Start()
    {
        _rubble.SetActive(false);
        _dpController = (IDroppodControllable)_dpControllerObject.GetComponent<IDroppodControllable>();
    }

    public void Nuke(Vector3 impactLocation)
    {
        this.transform.position = impactLocation;
        _nukePS.Play();

        IEnumerator spawnTimer()
        {
            yield return new WaitForSeconds(_playerSpawnTime);
            SpawnPlayer();
        }
        IEnumerator completeTimer()
        {
            yield return new WaitForSeconds(_nukeCompleteTime);
            CompleteNuke();
        }

        StartCoroutine(spawnTimer());
        StartCoroutine(completeTimer());

    }
    public void SpawnPlayer()
    {
        _rubble.SetActive(true);
        _dpController.Impact();
    }
    public void CompleteNuke()
    {
        _dpController.CompleteLanding();
        _dpControllerObject.SetActive(false);
    }
}
