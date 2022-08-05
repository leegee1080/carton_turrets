using System.Linq;
using UnityEngine;

public interface IPassableObject
{
    
}

public class ObjectPooler
{

    public GameObject[] Pool;
    public int index;
    private bool Expandable {get;}
    private GameObject Container{get;}

    public ObjectPooler(GameObject obj, int amount, GameObject parent, bool exp)
    {   
        if(obj.GetComponent(typeof(PoolableObject)) == null)
        {
            Debug.LogWarning("Passed object "+ obj.name +" is not poolable. ObjectPooler on: " + this + "");
            return;
        }

        Pool = new GameObject[amount];

        for (int i = 0; i < Pool.Length; i++)
        {
            Pool[i] = GameObject.Instantiate(obj, parent.transform);
        }
        Expandable = exp;
        index = 0;
        Container = parent;
    }

    public GameObject ActivateNextObject(IPassableObject ObjectCaller)
    {
        GameObject pickedGO = Pool[index];
        index +=1;
        if(index >= Pool.Length){index =0;}
        pickedGO.GetComponent<PoolableObject>().ActivateObjectFunc.Invoke(ObjectCaller);
        return pickedGO;
    }
    public GameObject PickNextObject(int index, IPassableObject ObjectCaller = default)
    {
        if(index >= Pool.Length || index < 0){Debug.LogWarning("index parameter is not correct: " + index + ". Setting index to 0."); return Pool[0];}
        Pool[index].GetComponent<PoolableObject>().ActivateObjectFunc.Invoke(ObjectCaller);
        return Pool[index];
    }
    public void PoolMoreOjects(GameObject obj, int amount)
    {
        if(!Expandable){Debug.LogWarning("This objectpooler is not expandable"); return;}

        GameObject[] tempPool = new GameObject[amount];

        for (int i = 0; i < tempPool.Length; i++)
        {
            tempPool[i] = GameObject.Instantiate(obj, Container.transform);
        }

        Pool = Pool.Concat(tempPool).ToArray();
    }
}
