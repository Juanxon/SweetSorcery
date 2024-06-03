using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLevelInfo : MonoBehaviour
{
    [SerializeField] LevelData _levelData;

    [SerializeField] GameObject _easyMedal, _normalMedal, _hardMedal;
    private void OnEnable()
    {
        if(_levelData.IsEasyDifficultComplete)
        {
            _easyMedal.SetActive(true);
        }
        if(_levelData.IsNormalDifficultComplete)
        {
            _normalMedal.SetActive(true);
        }
        if(_levelData.IsHardDifficultComplete)
        {
            _hardMedal.SetActive(true);
        }
    }
}
