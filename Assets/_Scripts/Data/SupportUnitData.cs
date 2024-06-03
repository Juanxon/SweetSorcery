using AnnulusGames.LucidTools.Inspector;
using Newtonsoft.Json.Linq;
using Obvious.Soap;
using UnityEngine;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/SupportUnitData")]
public class SupportUnitData : UnitData
{

    [FoldoutGroup("Support")][SerializeField] float _healPulseRange;
    [FoldoutGroup("Support")][SerializeField] float _initHealPulseRange;
    [FoldoutGroup("Support")][SerializeField] float _maxHealPulseRange;
    [FoldoutGroup("Support")][SerializeField] AnimationCurve _healPulseRangeCurve;

    [FoldoutGroup("Support")][SerializeField] float _healPulseTime;
    [FoldoutGroup("Support")][SerializeField] float _initHealPulseTime;
    [FoldoutGroup("Support")][SerializeField] float _maxHealPulseTime;
    [FoldoutGroup("Support")][SerializeField] AnimationCurve _healPulseTimeCurve;

    [FoldoutGroup("Support")][SerializeField] float _healAmount;
    [FoldoutGroup("Support")][SerializeField] float _initHealAmount;
    [FoldoutGroup("Support")][SerializeField] float _maxHealAmount;
    [FoldoutGroup("Support")][SerializeField] AnimationCurve _healAmountCurve;

    [FoldoutGroup("Support")][SerializeField] bool _giveRegeneration;
    [FoldoutGroup("Support")][SerializeField] bool _giveArmor;
    [FoldoutGroup("Support")][SerializeField] bool _explode;

    public float HealPulseRange { get => _healPulseRange; }
    public float HealPulseTime { get => _healPulseTime; }
    public float HealAmount { get => _healAmount; }
    public bool GiveRegeneration { get => _giveRegeneration; set => _giveRegeneration = value; }
    public bool GiveArmor { get => _giveArmor; set => _giveArmor = value; }
    public bool Explode { get => _explode; set => _explode = value; }

    public override void UpdateStats()
    {
        base.UpdateStats();
        _healPulseRange = Mathf.Abs(_healPulseRangeCurve.Evaluate(Level));
        _healPulseTime = Mathf.Abs(_healPulseTimeCurve.Evaluate(Level));
        _healAmount = Mathf.Abs(_healAmountCurve.Evaluate(Level));

    }

    public override void SetCurveKeys()
    {
        base.SetCurveKeys();
        SetBaseCurveKeys(_healPulseRangeCurve, _initHealPulseRange, _maxHealPulseRange);
        SetBaseCurveKeys(_healPulseTimeCurve, _initHealPulseTime, _maxHealPulseTime);
        SetBaseCurveKeys(_healAmountCurve, _initHealAmount, _maxHealAmount);

    }

    public override JObject Serialize()
    {
        SaveData sd = new SaveData(Level, SpawnCost, Armor, Regeneration, SpawnAnyWhere, GiveRegeneration, GiveArmor, Explode);

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

        GiveRegeneration = sd.giveRegeneration;
        GiveArmor = sd.giveArmor;
        Explode = sd.explode;
    }

    private class SaveData
    {
        public int level;

        public int spawnCost;
        public bool armor;
        public bool regeneration;
        public bool spawnAnywhere;

        public bool giveRegeneration;
        public bool giveArmor;
        public bool explode;

        public SaveData(int level, int spawnCost, bool armor, bool regeneration, bool spawnAnywhere, bool giveRegeneration, bool giveArmor, bool explode)
        {
            this.level = level;
            this.spawnCost = spawnCost;
            this.armor = armor;
            this.regeneration = regeneration;
            this.spawnAnywhere = spawnAnywhere;

            this.giveRegeneration = giveRegeneration;
            this.giveArmor = giveArmor;
            this.explode = explode;

        }
    }
}
