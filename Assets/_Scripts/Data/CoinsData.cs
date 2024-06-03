using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/CoinsData")]
public class CoinsData : Data, ISave
{
    [SerializeField] float _coins;

    public override float Value { get => _coins; 
        set 
        {
            _coins = value;
            OnValueChange?.Invoke();
        }
    }

    public override JObject Serialize()
    {
        SaveData sd = new SaveData((int)_coins);

        string jsonString = JsonUtility.ToJson(sd);
        JObject retVal = JObject.Parse(jsonString);

        return retVal;
    }
    public override void Deserialize(string jsonString)
    {
        SaveData sd = JsonUtility.FromJson<SaveData>(jsonString);

        Value = sd.coins;
    }

    public override void ResetData()
    {
        Value = 0;
    }

    public class SaveData
    {
        public int coins;

        public SaveData(int coins)
        {
            this.coins = coins;
        }
    }

}
