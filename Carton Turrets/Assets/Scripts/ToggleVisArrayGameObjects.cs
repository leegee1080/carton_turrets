using UnityEngine;

public class ToggleVisArrayGameObjects : MonoBehaviour
{
    [SerializeField]private GameObject[] _array;
    public void UnHide()
    {
        foreach (GameObject item in _array)
        {
            item.SetActive(true);
        }
    }
    public void Hide()
    {
        foreach (GameObject item in _array)
        {
            item.SetActive(false);
        }
    }
}
