using AnnulusGames.LucidTools.Inspector;
using Newtonsoft.Json.Linq;
using Obvious.Soap;
using UnityEngine;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/RangedUnitData")]
public class RangedUnitData : UnitData
{

    [FoldoutGroup("Ranged")][SerializeField] float _arrowSpeed;
    [FoldoutGroup("Ranged")][SerializeField] float _initArrowSpeed;
    [FoldoutGroup("Ranged")][SerializeField] float _maxArrowSpeed;
    [FoldoutGroup("Ranged")][SerializeField] AnimationCurve _arrowSpeedCurve;
    [FoldoutGroup("Ranged")][SerializeField] int arrowsPerAttack;
    [FoldoutGroup("Ranged")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] GameObject _arrow;

    public float ArrowSpeed { get => _arrowSpeed; }
    public GameObject Arrow { get => _arrow; }
    public int ArrowsPerAttack { get => arrowsPerAttack; set => arrowsPerAttack = value; }

    public override void UpdateStats()
    {
        base.UpdateStats();
        _arrowSpeed = Mathf.Abs(_arrowSpeedCurve.Evaluate(Level));
    }

    public override void SetCurveKeys()
    {
        base.SetCurveKeys();
        SetBaseCurveKeys(_arrowSpeedCurve, _initArrowSpeed, _maxArrowSpeed);
    }

    public override JObject Serialize()
    {
        SaveData sd = new SaveData(Level, SpawnCost, Armor, Regeneration, SpawnAnyWhere, ArrowsPerAttack);

        string jsonString = JsonUtility.ToJson(sd);
        JObject retVal = JObject.Parse(jsonString);

        return retVal;
    }

    public override void Deserialize(string jsonString)
    {
        SaveData sd = JsonUtility.FromJson<SaveData>(jsonString);

        Level = sd.level;
        SpawnCost = sd.spawnCost;
        Armor = sd.armor;
        Regeneration = sd.regeneration;
        SpawnAnyWhere = sd.spawnAnywhere;

        ArrowsPerAttack = sd.arrowsPerAttack;
    }

    private class SaveData
    {
        public int level;

        public int spawnCost;
        public bool armor;
        public bool regeneration;
        public bool spawnAnywhere;

        public int arrowsPerAttack;

        public SaveData(int level, int spawnCost, bool armor, bool regeneration, bool spawnAnywhere, int arrowsPerAttack)
        {
            this.level = level;
            this.spawnCost = spawnCost;
            this.armor = armor;
            this.regeneration = regeneration;
            this.spawnAnywhere = spawnAnywhere;
            this.arrowsPerAttack = arrowsPerAttack;
        }
    }
}
