using Obvious.Soap;
using UnityEngine;

public class AddToGameObjectList : MonoBehaviour
{
    [SerializeField] ScriptableListGameObject _list;

    private void Start()
    {
        _list.Add(gameObject);
    }

    private void OnDisable()
    {
        _list.Remove(gameObject);
    }
}
