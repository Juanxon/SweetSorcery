using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ShowGainCoins : MonoBehaviour
{
    public LevelScreenManager levelManager;
    [SerializeField] TextMeshProUGUI _text;

    private void OnEnable()
    {
        if (levelManager.DifficultChosen.currentDifficulty == DifficultLevel.Difficulty.Easy)
        {
            _text.text = "+" + levelManager.ChosenLevelData.CoinsOnCompleteEasy;
        }
        else if (levelManager.DifficultChosen.currentDifficulty == DifficultLevel.Difficulty.Normal)
        {
            _text.text = "+" + levelManager.ChosenLevelData.CoinsOnCompleteNormal;

        }
        else if (levelManager.DifficultChosen.currentDifficulty == DifficultLevel.Difficulty.Hard)
        {
            _text.text = "+" + levelManager.ChosenLevelData.CoinsOnCompleteHard;
        }
    }
}
