using UnityEngine;

public class ToggleVisArrayGameObjects : MonoBehaviour
{
    [SerializeField]private GameObject[] _array;
    [SerializeField]private bool _useInputArray;
    [SerializeField]private bool _hideOnAwake;

    private void Awake() {
        if(_useInputArray)
        {
            _array = new GameObject[this.gameObject.transform.childCount];
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = this.gameObject.transform.GetChild(i).gameObject;
            }
        }
        if(_hideOnAwake){Hide();}
    }
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
