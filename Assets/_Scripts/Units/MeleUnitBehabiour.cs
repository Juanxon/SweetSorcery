using Drawing;
using UnityEngine;

public class MeleUnitBehabiour : UnitBehabiourBase
{
    private void OnEnable()
    {
        agent.speed = unitData.Speed;
        StartCoroutine(FindClosestEnemyCo(6, 1));

        Health = unitData.Health;
        healthDisplay.SetHealthBar(unitData.Health);
        agent.stoppingDistance = unitData.Range;

        if (unitData.Regeneration) StartCoroutine(RegenerationCo());
        StartBuffsCheck();
    }

    private void FixedUpdate()
    {
        Behaviour();
        Draw.WireSphere(transform.position, 6, Color.green);
    }
}
