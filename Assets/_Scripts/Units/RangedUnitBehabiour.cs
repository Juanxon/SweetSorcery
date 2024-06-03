using Drawing;
using UnityEngine;

public class RangedUnitBehabiour : UnitBehabiourBase
{
    [SerializeField] Transform shootPoint;
    private void OnEnable()
    {
        agent.speed = unitData.Speed;
        agent.stoppingDistance = unitData.Range - 0.3f;
        StartCoroutine(FindClosestEnemyCo(unitData.Range, (unitData as RangedUnitData).ArrowsPerAttack));
        Health = unitData.Health;
        healthDisplay.SetHealthBar(unitData.Health);

        if (unitData.Regeneration) StartCoroutine(RegenerationCo());
        StartBuffsCheck();

    }

    private void FixedUpdate()
    {
        Behaviour();
        Draw.WireSphere(transform.position, (unitData as RangedUnitData).Range, Color.green);
    }

    public override void Attack()
    {

        foreach (var target in targets)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > unitData.Range) return;

            // Instanciar la flecha
            GameObject arrow = Instantiate((unitData as RangedUnitData).Arrow, shootPoint.position, Quaternion.identity);

            // Obtener el componente de la flecha
            Arrow arrowComponent = arrow.GetComponent<Arrow>();

            if (arrowComponent != null && targets.Count > 0)
            {
                if(!damageBuff)
                {
                    arrowComponent.InitializeArrow(target.transform, (unitData as RangedUnitData).ArrowSpeed, (int)unitData.Damage, false);
                }
                else
                {
                    arrowComponent.InitializeArrow(target.transform, (unitData as RangedUnitData).ArrowSpeed, (int)ToolBox.AddPercentage(unitData.Damage, 25), false);
                }

            }

        }
    
    }
}
