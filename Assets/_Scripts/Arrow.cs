using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;
    private bool slerp;

    private void OnEnable()
    {
        Destroy(gameObject,2);
    }
    public void InitializeArrow(Transform target, float speed, int damage, bool slerp)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.slerp = slerp;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.position);
            if(!slerp)
            {
                // Mover la flecha hacia el objetivo utilizando Slerp
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            }
            else
            {
                transform.position = Vector3.Slerp(transform.position, target.position, speed * Time.deltaTime);
            }

            // Si la flecha llega al objetivo, aplicar daño y destruir la flecha
            if (Vector3.Distance(transform.position, target.position) < 0.8f)
            {
                // Aplicar el daño al objetivo
                if(target.gameObject.TryGetComponent<IDamageable>(out IDamageable enemy))
                {
                    enemy.TakeDamage(damage);
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
