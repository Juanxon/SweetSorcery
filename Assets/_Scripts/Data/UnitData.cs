using AnnulusGames.LucidTools.Inspector;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/UnitData")]
public class UnitData : Data
{
    [FoldoutGroup("Level")][SerializeField] int _initLevelCost;
    [FoldoutGroup("Level")][SerializeField] int _maxLevelCost;
    [FoldoutGroup("Level")][SerializeField] int _levelCost;
    [FoldoutGroup("Level")][SerializeField] AnimationCurve _levelCostCurve;
    [FoldoutGroup("Level")][SerializeField] int _maxLevel;
    [FoldoutGroup("Level")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] int _level;
    [FoldoutGroup("Units")][SerializeField] GameObject _unitPrefab;
    [FoldoutGroup("Units")][SerializeField] GameObject[] _unitPrefabs;
    [FoldoutGroup("Health")][SerializeField] int _initHealth;
    [FoldoutGroup("Health")][SerializeField] int _maxHealth;
    [FoldoutGroup("Health")][SerializeField] AnimationCurve _healthCurve;
    [FoldoutGroup("Health")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] int _health;

    [FoldoutGroup("Damage")][SerializeField] int _initDamage;
    [FoldoutGroup("Damage")][SerializeField] int _maxDamage;
    [FoldoutGroup("Damage")][SerializeField] AnimationCurve _damageCurve;
    [FoldoutGroup("Damage")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] int _damage;

    [FoldoutGroup("Range")][SerializeField] float _initRange;
    [FoldoutGroup("Range")][SerializeField] float _maxRange;
    [FoldoutGroup("Range")][SerializeField] AnimationCurve _rangeCurve;
    [FoldoutGroup("Range")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] float _range;

    [FoldoutGroup("FireRate")][SerializeField] float _initFireRate;
    [FoldoutGroup("FireRate")][SerializeField] float _maxFireRate;
    [FoldoutGroup("FireRate")][SerializeField] AnimationCurve _fireRateCurve;
    [FoldoutGroup("FireRate")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] float _fireRate;

    [FoldoutGroup("Speed")][SerializeField] float _initSpeed;
    [FoldoutGroup("Speed")][SerializeField] float _maxSpeed;
    [FoldoutGroup("Speed")][SerializeField] AnimationCurve _speedCurve;
    [FoldoutGroup("Speed")][GUIColor(0.7f, 1f, 0.25f)][SerializeField] float _speed;

    [FoldoutGroup("OnBattle")][SerializeField] int _spawnCost;
    [FoldoutGroup("OnBattle")][SerializeField] bool _armor;
    [FoldoutGroup("OnBattle")][SerializeField] bool _regeneration;
    [FoldoutGroup("OnBattle")][SerializeField] bool _spawnAnyWhere;

    [SerializeField] int _typeID;
    [DisableInEditMode]
    [SerializeField] float _power;

    public int Level
    {
        get => _level;
        set
        {
            if(value > _maxLevel) { return; }
            _level = value;
            UpdateStats();
            CalculateUnitPower();
            OnValueChange?.Invoke();
        }
    }

    public int LevelCost { get => _levelCost; }
    public int Health { get => _health; }
    public int Damage { get => _damage; }
    public float Range { get => _range; }
    public float FireRate { get => _fireRate; }
    public float Speed { get => _speed; }
    public GameObject UnitPrefab { get => _unitPrefab; set => _unitPrefab = value; }
    public int SpawnCost { get => _spawnCost; set => _spawnCost = value; }
    public bool Armor { get => _armor; set => _armor = value; }
    public bool Regeneration { get => _regeneration; set => _regeneration = value; }
    public bool SpawnAnyWhere { get => _spawnAnyWhere; set => _spawnAnyWhere = value; }
    public int TypeID { get => _typeID; }
    public float Power { get => _power; }
    public int MaxLevel { get => _maxLevel; }

    public virtual void UpdateStats()
    {
        _levelCost = Mathf.RoundToInt(Mathf.Abs(_levelCostCurve.Evaluate(Level)));
        _health = Mathf.RoundToInt(Mathf.Abs(_healthCurve.Evaluate(Level)));
        _damage = Mathf.RoundToInt(Mathf.Abs(_damageCurve.Evaluate(Level)));
        _range = Mathf.Abs(_rangeCurve.Evaluate(Level));
        _fireRate = Mathf.Abs(_fireRateCurve.Evaluate(Level));
        _speed = Mathf.Abs(_speedCurve.Evaluate(Level));
    }

    public virtual void SetCurveKeys()
    {
        SetBaseCurveKeys(_levelCostCurve, _initLevelCost, _maxLevelCost);
        SetBaseCurveKeys(_healthCurve, _initHealth, _maxHealth);
        SetBaseCurveKeys(_damageCurve, _initDamage, _maxDamage);
        SetBaseCurveKeys(_rangeCurve, _initRange, _maxRange);
        SetBaseCurveKeys(_fireRateCurve, _initFireRate, _maxFireRate);
        SetBaseCurveKeys(_speedCurve, _initSpeed, _maxSpeed);

    }



    public void SetBaseCurveKeys(AnimationCurve myCurve, float initValue, float maxValue)
    {
        // Borra todas las claves existentes
        myCurve.keys = new Keyframe[0];

        // Agrega las nuevas claves con el modo de tangente en "Auto"
        Keyframe key1 = new Keyframe(1, initValue);
        key1.inTangent = 0; // Establece el modo de tangente en "Auto"
        key1.outTangent = 0; // Establece el modo de tangente en "Auto"

        Keyframe key2 = new Keyframe(_maxLevel, maxValue);
        key2.inTangent = 0; // Establece el modo de tangente en "Auto"
        key2.outTangent = 0; // Establece el modo de tangente en "Auto"

        myCurve.AddKey(key1);
        myCurve.AddKey(key2);
    }

    [Button]
    public void ClickUpdateStats()
    {
        SetCurveKeys();
        UpdateStats();
    }

    [Button]
    public virtual float CalculateUnitPower()
    {
        // Calculamos el DPS
        float dps = Damage / FireRate;

        // Asignamos pesos a cada variable. Ajusta estos valores según las necesidades del juego.
        float healthWeight = 0.4f;
        float dpsWeight = 1f;
        float rangeWeight = 1.5f;

        // Calculamos el poder de la unidad usando los pesos asignados.
        float power = ((Health / 3) * healthWeight) + (dps * dpsWeight) + (Range * rangeWeight + Level);

        _power = Mathf.RoundToInt(power);
        return power;
    }

    public override JObject Serialize()
    {
        SaveData sd = new SaveData(Level, SpawnCost, Armor, Regeneration, SpawnAnyWhere);

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
    }

    public override void ResetData()
    {
        Level = 1;
    }

    private class SaveData
    {
        public int level;

        public int spawnCost;
        public bool armor;
        public bool regeneration;
        public bool spawnAnywhere;

        public SaveData(int level, int spawnCost, bool armor, bool regeneration, bool spawnAnywhere)
        {
            this.level = level;
            this.spawnCost = spawnCost;
            this.armor = armor;
            this.regeneration = regeneration;
            this.spawnAnywhere = spawnAnywhere;
        }
    }
}
