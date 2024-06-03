using Drawing;
using UnityEngine;
using UnityEngine.Events;

public class MainBase : UnitBehabiourBase
{
    public UnityEvent OnDestroyEvent;
    public GameObject spawnPoint;
    private void OnEnable()
    {
        Health = unitData.Health;
        healthDisplay.SetHealthBar(unitData.Health);
        StartCoroutine(FindClosestEnemyCo(unitData.Range * gameObject.transform.localScale.x, (unitData as RangedUnitData).ArrowsPerAttack));
        if (unitData.Regeneration) StartCoroutine(RegenerationCo());
    }

    private void OnDisable()
    {
        OnDestroyEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        Behaviour();
        Draw.WireSphere(transform.position, (unitData as RangedUnitData).Range * gameObject.transform.localScale.x, Color.green);
    }

    public override void Attack()
    {
        foreach (var target in targets)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > unitData.Range * gameObject.transform.localScale.x) return;

            // Instanciar la flecha
            GameObject arrow = Instantiate((unitData as RangedUnitData).Arrow, spawnPoint.transform.position, Quaternion.identity);

            // Obtener el componente de la flecha
            Arrow arrowComponent = arrow.GetComponent<Arrow>();

            if (arrowComponent != null && targets.Count > 0)
            {

                arrowComponent.InitializeArrow(target.transform, (unitData as RangedUnitData).ArrowSpeed, (int)unitData.Damage, true);

            }

        }

    }

    public override void Behaviour()
    {
        if (targets != null && enemyCollider != null)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1 / unitData.FireRate;
            }
        }
    }
}
