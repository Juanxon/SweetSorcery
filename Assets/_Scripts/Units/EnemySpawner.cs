using Obvious.Soap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : UnitBehabiourBase, IDamageable
{
    public GameObject objectToSpawn;
    public CampamentManager manager;
    public Transform spawnPoint;
    public BoolVariable gameStarted;

    private Coroutine EnemySpawnCo;
    private void OnEnable()
    {
        Health = unitData.Health;
        healthDisplay.SetHealthBar(unitData.Health);

        if (unitData.Regeneration) StartCoroutine(RegenerationCo());
        StartBuffsCheck();

        gameStarted.OnValueChanged += StartEnemySpawn;

    }

    private void OnDisable()
    {
        FunctionTimer.Create(manager.OnSpawnerDestroy, 0.1f);
        gameStarted.OnValueChanged -= StartEnemySpawn;
    }

    public IEnumerator SpawnEnemy(bool gameStarted)
    {
        while (gameStarted)
        {
            yield return new WaitForSeconds(Random.Range(unitData.FireRate -2, unitData.FireRate + 2));

            if(spawnPoint != null)
            {
                Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
        }
    }

    public void StartEnemySpawn(bool gameStarted)
    {
        if(gameStarted)
        {
            EnemySpawnCo = StartCoroutine(SpawnEnemy(gameStarted));
        }
        else
        {
            if(EnemySpawnCo != null)
            {
                StopCoroutine(EnemySpawnCo);
            }
        }
    }
}
