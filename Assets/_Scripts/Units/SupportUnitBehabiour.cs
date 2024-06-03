using Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportUnitBehabiour : UnitBehabiourBase
{
    public ParticleSystem healPulseParticles;
    public string myTeamTag;
    public GameObject closestAlly;
    Coroutine healPulseCo;

    void OnEnable()
    {
        agent.speed = unitData.Speed;
        agent.stoppingDistance = unitData.Range;
        StartCoroutine(FindClosestAllyCo(7));
        Health = unitData.Health;
        healthDisplay.SetHealthBar(unitData.Health);

        if (unitData.Regeneration) StartCoroutine(RegenerationCo());

        healPulseCo = StartCoroutine(HealPulse());
        StartBuffsCheck();
    }

    private void OnDisable()
    {
        StopCoroutine(HealPulse());
    }

    void Update()
    {
        Draw.WireSphere(transform.position, (unitData as SupportUnitData).HealPulseRange, Color.green);
    }

    private void FixedUpdate()
    {
        Behaviour();
    }

    public IEnumerator HealPulse()
    {
        while (true)
        {
            yield return new WaitForSeconds((unitData as SupportUnitData).HealPulseTime);

            Collider[] allys = Physics.OverlapSphere(transform.position, (unitData as SupportUnitData).HealPulseRange);
            healPulseParticles.Play();
            _animator.SetTrigger("Heal");
            foreach (Collider collider in allys)
            {
                if(collider.gameObject.CompareTag(myTeamTag) && collider.gameObject.TryGetComponent<UnitBehabiourBase>(out UnitBehabiourBase unit))
                {
                    unit.TakeDamage((int)(-ToolBox.ApplyPercentage(unit.unitData.Health, (unitData as SupportUnitData).HealAmount)));

                    if((unitData as SupportUnitData).GiveArmor)
                    {
                        unit.SetArmorBuff();
                    }
                    if ((unitData as SupportUnitData).GiveRegeneration)
                    {
                        unit.SetRegenerationBuff();
                    }
                }
            }
            
        }
    }

    public override void Attack()
    {
        return;
    }

    public override void Behaviour()
    {
        if (closestAlly != null)
        {
            agent.destination = closestAlly.GetComponent<Collider>().ClosestPoint(transform.position);
            
        }
        else
        {
            //Que se ponga a temblar
        }
    }
    public GameObject FindClosestAlly(float range)
    {
        GameObject closestObject = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject obj in _targetList)
        {
            if (obj.CompareTag("Player"))
                continue;
            if (obj.GetComponent<SupportUnitBehabiour>())
                continue;

            Vector3 objPosition = obj.transform.position;
            objPosition.y = 0f; // Ignorar el eje Y

            float distance = Vector3.Distance(transform.position, objPosition);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    public IEnumerator FindClosestAllyCo(float range)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (closestAlly == null)
            closestAlly = FindClosestAlly(range);
        }
    }

}
