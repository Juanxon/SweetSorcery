using UnityEngine;

[CreateAssetMenu(menuName = "ScripteableObjects/Data/LevelDificulty")]
public class DifficultLevel : Data
{
    public enum Difficulty
    {
        Easy, Normal, Hard
    }

    public Difficulty currentDifficulty;
}
