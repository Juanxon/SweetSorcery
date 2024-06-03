using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/LevelData")]
public class LevelData : Data, ISave
{
    [SerializeField] string _levelName;
    public string LevelName { get => _levelName; }
    [SerializeField] string _sceneName;
    public string SceneName { get => _sceneName; }

    [Header("Easy")]
    [SerializeField] bool _isEasyDifficultComplete;
    public bool IsEasyDifficultComplete { get => _isEasyDifficultComplete; set => _isEasyDifficultComplete = value; }
    [SerializeField] int _coinsOnCompleteEasy;
    public int CoinsOnCompleteEasy { get => _coinsOnCompleteEasy; }
    [SerializeField] int _onEasyEnemyLevel;
    public int OnEasyEnemyLevel { get => _onEasyEnemyLevel; }

    [Header("Normal")]
    [SerializeField] bool _isNormalDifficultComplete;
    public bool IsNormalDifficultComplete { get => _isNormalDifficultComplete; set => _isNormalDifficultComplete = value; }
    [SerializeField] int _coinsOnCompleteNormal;
    public int CoinsOnCompleteNormal { get => _coinsOnCompleteNormal; }
    [SerializeField] int _onNormalEnemyLevel;
    public int OnNormalEnemyLevel { get => _onNormalEnemyLevel; }

    [Header("Hard")]
    [SerializeField] bool _isHardDifficultComplete;
    public bool IsHardDifficultComplete { get => _isHardDifficultComplete; set => _isHardDifficultComplete = value; }
    [SerializeField] int _coinsOnCompleteHard;
    public int CoinsOnCompleteHard { get => _coinsOnCompleteHard; }
    [SerializeField] int _onHardEnemyLevel;
    public int OnHardEnemyLevel { get => _onHardEnemyLevel; }

    public override JObject Serialize()
    {
        SaveData sd = new SaveData(_isEasyDifficultComplete, _isNormalDifficultComplete, _isHardDifficultComplete);

        string jsonString = JsonUtility.ToJson(sd);
        JObject retVal = JObject.Parse(jsonString);

        return retVal;
    }

    public override void Deserialize(string jsonString)
    {
        SaveData sd = JsonUtility.FromJson<SaveData>(jsonString);

        IsEasyDifficultComplete = sd.isEasyDifficultComplete;
        IsNormalDifficultComplete = sd.isNormalDifficultComplete;
        IsHardDifficultComplete = sd.isHardDifficultComplete;
    }

    public override void ResetData()
    {
        _isEasyDifficultComplete = false;
        _isNormalDifficultComplete = false;
        _isHardDifficultComplete = false;
    }

    private class SaveData
    {
        public bool isEasyDifficultComplete;
        public bool isNormalDifficultComplete;
        public bool isHardDifficultComplete;

        public SaveData(bool isEasyDifficultComplete, bool isNormalDifficultComplete, bool isHardDifficultComplete)
        {
            this.isEasyDifficultComplete = isEasyDifficultComplete;
            this.isNormalDifficultComplete = isNormalDifficultComplete;
            this.isHardDifficultComplete = isHardDifficultComplete;
        }
    }
}
