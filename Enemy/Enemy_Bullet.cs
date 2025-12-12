using UnityEngine;

public class Enemy_Bullet : Bullet
{

    protected override void OnCollisionEnter(Collision collision)
    {
        // Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        // if (enemy != null)
        //     return;

        CreateImpactFx(collision);
        ReturnBulletToPool();

        // Player player = collision.gameObject.GetComponentInParent<Player>();
    }
}
