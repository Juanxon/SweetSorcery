using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpell : MonoBehaviour
{
    private Vector3 target;
    [SerializeField] float speed;
    [SerializeField] int damage;
    bool slerp;
    [SerializeField] float radius;

    private void OnEnable()
    {
        Destroy(gameObject, 10);
    }
    public void InitializeFireBall(Vector3 target, bool slerp)
    {
        this.target = target;

        this.slerp = slerp;

    }

    private void Update()
    {
        if (target != null)
        {
            speed += Time.deltaTime * 40;
            if (!slerp)
            {
                // Mover la flecha hacia el objetivo utilizando Slerp
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, target, speed * Time.deltaTime);
            }

            // Si la flecha llega al objetivo, aplicar daño y destruir la flecha
            if (Vector3.Distance(transform.position, target) < 0.8f)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
                // Aplicar el daño al objetivo
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Enemy"))
                    {
                        collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageScript);

                        damageScript.TakeDamage(damage);
                    }


                }

                // Destruir la flecha
                Destroy(gameObject);
            }
        }
        else
        {
            // Destruir la flecha si no hay objetivo
            Destroy(gameObject);
        }
    }
}
