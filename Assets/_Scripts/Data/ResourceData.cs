using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/ResourceData")]
public class ResourceData : Data
{
    public Action onValueChange;
    [SerializeField] string _name;
    [SerializeField][Multiline] string _description;
    [SerializeField] int _value;
    [SerializeField] int _maxValue;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public override float Value 
    { get => _value;  
      set
      {
            _value = (int)Mathf.Clamp(value, 0, MaxValue);
            onValueChange?.Invoke();
      }
    }

    public int MaxValue
    { get => _maxValue;
        set
        {
            _maxValue = value;
            onValueChange?.Invoke();
        }
    }

    public float ValueFillAmount
    {
        get
        {
            return (float)_value / (float)_maxValue;
        }
    }

    

}
