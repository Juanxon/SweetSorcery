using UnityEngine;

public class LevelScreenManager : MonoBehaviour
{
    [SerializeField] DifficultLevel _difficultChosen;
    [SerializeField] LevelData _chosenLevelData;
    [SerializeField] CoinsData _coinsData;
    [SerializeField] SaveObjectsManager _saveObjectsManager;

    [SerializeField] UnitData[] _enemyUnits;

    [SerializeField] bool _resetLevelDataOnStart = false;

    public DifficultLevel DifficultChosen { get => _difficultChosen; }
    public LevelData ChosenLevelData { get => _chosenLevelData; }

    void Start()
    {
        if (_resetLevelDataOnStart) _chosenLevelData = null;
    }

    public void SetLevelData(LevelData levelData)
    {
        _chosenLevelData = levelData;
    }

    public void LoadChosenLevel()
    {
        foreach (UnitData unitData in _enemyUnits)
        {
            if (_difficultChosen.currentDifficulty == DifficultLevel.Difficulty.Easy)
            {
                unitData.Level = _chosenLevelData.OnEasyEnemyLevel;
            }
            else if (_difficultChosen.currentDifficulty == DifficultLevel.Difficulty.Normal)
            {
                unitData.Level = _chosenLevelData.OnNormalEnemyLevel;
            }
            else if (_difficultChosen.currentDifficulty == DifficultLevel.Difficulty.Hard)
            {
                unitData.Level = _chosenLevelData.OnHardEnemyLevel;
            }
        }

        if (_chosenLevelData != null)
        {
            ToolBox.LoadScene(_chosenLevelData.SceneName);
        }
    }

    public void SetDifficultToEasy()
    {
        _difficultChosen.currentDifficulty = DifficultLevel.Difficulty.Easy;
    }

    public void SetDifficultyToNormal()
    {
        _difficultChosen.currentDifficulty = DifficultLevel.Difficulty.Normal;
    }

    public void SetDifficultyToHard()
    {
        _difficultChosen.currentDifficulty = DifficultLevel.Difficulty.Hard;
    }

    public void OnLevelComplete()
    {
        if (_difficultChosen.currentDifficulty == DifficultLevel.Difficulty.Easy)
        {
            _chosenLevelData.IsEasyDifficultComplete = true;
            _coinsData.Value += _chosenLevelData.CoinsOnCompleteEasy;
        }
        else if (_difficultChosen.currentDifficulty == DifficultLevel.Difficulty.Normal)
        {
            _chosenLevelData.IsNormalDifficultComplete = true;
            _coinsData.Value += _chosenLevelData.CoinsOnCompleteNormal;
        }
        else if (_difficultChosen.currentDifficulty == DifficultLevel.Difficulty.Hard)
        {
            _chosenLevelData.IsHardDifficultComplete = true;
            _coinsData.Value += _chosenLevelData.CoinsOnCompleteHard;
        }

        _saveObjectsManager.Save();
    }

    public void UpgradeUnit(UnitData unitData)
    {
        if (_coinsData.Value >= unitData.LevelCost && unitData.Level < unitData.MaxLevel)
        {
            _coinsData.Value -= unitData.LevelCost;
            unitData.Level++;
            _saveObjectsManager.Save();
        }

    }
}
