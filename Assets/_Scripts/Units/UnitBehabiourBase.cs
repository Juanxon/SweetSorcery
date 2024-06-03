using System.Collections.Generic;
using UnityEngine;
using Obvious.Soap;
using UnityEngine.AI;
using AnnulusGames.LucidTools.Inspector;
using System.Collections;

public abstract class UnitBehabiourBase : MonoBehaviour, IDamageable
{
    public UnitData unitData;
    [SerializeField] protected DisplayUnitHealth healthDisplay;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected ScriptableListGameObject _targetList;
    [SerializeField] protected Animator _animator;
    protected Collider enemyCollider;

    [TitleHeader("Targets")]
    protected List<GameObject> targets;
    public List<Transform> firstPointsToGo;
    [SerializeField] protected GameObjectVariable enemyBase;

    [TitleHeader("Effects")]
    [SerializeField] GameObject damageBuffEffect;
    [SerializeField] GameObject regenerationBuffEffect;
    [SerializeField] GameObject armorBuffEffect;

    protected int _health;
    protected float nextAttackTime = 0.0f;

    protected int regenerationBuffCounter;
    protected bool armor;
    protected int armorBuffCounter;
    protected bool damageBuff;
    protected int damageBuffCounter;
    public virtual int Health 
    {   get => _health; 
        set
        {
            _health = Mathf.Clamp(value, 0, unitData.Health);
        }
    }



    public virtual void Attack()
    {
        if (targets[0].gameObject.TryGetComponent<IDamageable>(out IDamageable enemy))
        {
            this.Log("Ataco");
            if(damageBuff)
            {
                enemy.TakeDamage((int)ToolBox.AddPercentage(unitData.Damage, 25f));
            }
            else
            {
                enemy.TakeDamage(unitData.Damage);
            }
        }
    }
    public virtual void Behaviour()
    {
        if (targets != null && enemyCollider != null)
        {
            agent.stoppingDistance = unitData.Range - 0.1f;
            if (agent.remainingDistance - 0.3f <= agent.stoppingDistance && Time.time >= nextAttackTime)
            {
                if(_animator != null)
                {
                    _animator.SetBool("Attack", true);
                    _animator.SetBool("Run", false);
                }
                Attack();
                nextAttackTime = Time.time + 1 / unitData.FireRate;
            }
            else if (agent.remainingDistance - 0.3f <= agent.stoppingDistance)
            {
                if (_animator != null)
                {
                    _animator.SetBool("Run", false);
                    _animator.SetBool("Attack", false);
                }
                
            }
            else
            {
                if (_animator != null)
                {
                    _animator.SetBool("Run", true);
                    _animator.SetBool("Attack", false);
                }
                Vector3 destinationPoint = enemyCollider.bounds.ClosestPoint(transform.position);
                agent.destination = destinationPoint;
            }
        }
        else
        {
            if (firstPointsToGo.Count > 0)
            {
                if(_animator != null)
                {
                    _animator.SetBool("Run", true);
                    _animator.SetBool("Attack", false);
                }
                agent.stoppingDistance = 0.5f;
                agent.destination = firstPointsToGo[0].position;

                if (Vector3.Distance(transform.position, firstPointsToGo[0].position) < 2)
                {
                    firstPointsToGo.RemoveAt(0);
                }
            }
            else
            {
                if(enemyBase.Value != null)
                {
                    if(_animator != null)
                    {
                        _animator.SetBool("Run", true);
                        _animator.SetBool("Attack", false);
                    }
                    agent.destination = enemyBase.Value.transform.position;
                }
                else
                {
                    if (_animator != null)
                    {
                        _animator.SetBool("Run", false);
                        _animator.SetBool("Attack", false);
                    }
                }
            }
        }


    }

    public virtual void TakeDamage(int damage)
    {
        if(unitData.Armor || armor)
        {
            Health -= (int)(damage * 0.3f);
        }
        else
        {
            Health -= damage;
        }

        this.Log("Recibo daño");

        healthDisplay.UpdateHealthBar();
        if (Health <= 0)
        {
            Destroy(gameObject);

        }
    }


    public virtual List<GameObject> FindClosestEnemies(float range, int maxCount)
    {
        List<GameObject> closestEnemies = new List<GameObject>();
        List<float> distances = new List<float>();

        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);

        foreach (GameObject enemy in _targetList)
        {
            if (enemy != null)
            {
                Vector3 enemyPosition = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
                float enemyDistance = Vector3.Distance(currentPosition, enemyPosition);

                // Verificar si el enemigo está dentro del rango
                if (enemyDistance <= range)
                {
                    // Mantener la lista ordenada por distancia
                    int index = 0;
                    while (index < distances.Count && enemyDistance > distances[index])
                    {
                        index++;
                    }

                    distances.Insert(index, enemyDistance);
                    closestEnemies.Insert(index, enemy.gameObject);
                }
            }
        }
        if (closestEnemies.Count < 1) { return null; }

        // Limitar la cantidad de enemigos devueltos según maxCount
        if (closestEnemies.Count > maxCount)
        {
            closestEnemies = closestEnemies.GetRange(0, maxCount);
        }
        targets = closestEnemies;

        enemyCollider = closestEnemies[0].GetComponent<Collider>();
        return closestEnemies;
    }

    public virtual IEnumerator FindClosestEnemyCo(float range, int maxCount)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            targets = FindClosestEnemies(range, maxCount);
        }
    }

    public virtual IEnumerator RegenerationCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Health += (int)ToolBox.ApplyPercentage(unitData.Health, 5);
            healthDisplay.UpdateHealthBar();

        }
    }

    public virtual IEnumerator RegenerationBuff()
    {
        while (true)
        {
            if (regenerationBuffCounter > 0)
            {
                Health += (int)ToolBox.ApplyPercentage(unitData.Health, 5);
                regenerationBuffCounter--;
                healthDisplay.UpdateHealthBar();
                if(regenerationBuffEffect != null)
                regenerationBuffEffect.SetActive(true);
            }
            else
            {
                if (regenerationBuffEffect != null)
                regenerationBuffEffect.SetActive(false);
            }
            yield return new WaitForSeconds(1);

        }
    }

    public virtual void SetRegenerationBuff()
    {
        regenerationBuffCounter = 5;
    }

    public virtual IEnumerator ArmorBuff()
    {
        while (true)
        {
            if (armorBuffCounter > 0)
            {
                armor = true;
                armorBuffCounter--;
                if (armorBuffEffect != null)
                armorBuffEffect.SetActive(true);
            }
            else
            {
                armor = false;
                if (armorBuffEffect != null)
                armorBuffEffect.SetActive(false);
            }
            yield return new WaitForSeconds(1);

        }
    }


    public virtual void SetArmorBuff()
    {
        armorBuffCounter = 5;
    }

    public virtual IEnumerator DamageBuff()
    {
        while (true)
        {
            if(damageBuffCounter > 0)
            {
                damageBuff = true;
                damageBuffCounter--;
                if (damageBuffEffect != null)
                damageBuffEffect.SetActive(true);
            }
            else
            {
                damageBuff = false;
                if (damageBuffEffect != null)
                damageBuffEffect.SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }

    }
    public virtual void SetDamageBuff()
    {
        damageBuffCounter = 5;
    }

    public virtual void StartBuffsCheck()
    {
        StartCoroutine(DamageBuff());
        StartCoroutine(ArmorBuff());
        StartCoroutine(RegenerationBuff());
    }

    public virtual void SetFirstTargets(List<Transform> myTransforms)
    {
        firstPointsToGo = new List<Transform>(myTransforms);
    }
}
