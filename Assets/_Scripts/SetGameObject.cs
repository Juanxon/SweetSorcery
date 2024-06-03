using Obvious.Soap;
using UnityEngine;

public class SetGameObject : MonoBehaviour
{
    [SerializeField] GameObjectVariable _reference;

    private void Start()
    {
        _reference.Value = gameObject;
    }

    private void OnEnable()
    {
        _reference.Value = gameObject;
    }

    private void OnDisable()
    {
        _reference.Value = null;
    }
}
