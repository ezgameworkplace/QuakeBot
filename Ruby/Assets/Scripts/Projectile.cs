using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Recycle();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Projectile Collision with " + collision.gameObject);
        EnemyController robot = collision.gameObject.GetComponent<EnemyController>();
        if (robot != null) { robot.Fix(); }

        Destroy(gameObject);
    }

    void Recycle()
    {
        //Debug.Log("gameObject.transform.position.magnitude::: " + gameObject.transform.position.magnitude);
        if (gameObject.transform.position.magnitude > 50f)
        {
            Destroy(gameObject);
        }
    }
}
