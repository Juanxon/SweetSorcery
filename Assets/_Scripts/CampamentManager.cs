using System;
using UnityEngine;
using UnityEngine.Events;

public class CampamentManager : MonoBehaviour
{
    public GameObject[] spawners;
    public GameObject spawnArea;

    public Action OnSpawnerDestroy;

    public UnityEvent CampCompleteEvent;

    private void OnEnable()
    {
        OnSpawnerDestroy += IsCampComplete;
    }

    private void OnDisable()
    {
        OnSpawnerDestroy -= IsCampComplete;
    }
    private void IsCampComplete()
    {
        int activeSpawners = 0;
        foreach (var spawner in spawners)
        {
            if (spawner != null)
            {
                activeSpawners++;
            }
        }

        if (activeSpawners < 1)
        {
            spawnArea.SetActive(true);
            InvokeUnityEvent();
        }
    }

    private void InvokeUnityEvent()
    {
        CampCompleteEvent.Invoke();
    }
}
