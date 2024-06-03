using Newtonsoft.Json.Linq;
using System;
using TMPro;
using UnityEngine;

public abstract class Data : ScriptableObject, ISave
{
    public Action OnValueChange { get; set; }
    public virtual float Value { get; set; }
    public virtual void Deserialize(string jsonString)
    {
        throw new System.NotImplementedException();
    }

    public virtual JObject Serialize()
    {
        throw new System.NotImplementedException();
    }

    public virtual void ResetData()
    {
        throw new System.NotImplementedException();
    }

}
