using UnityEngine;

public class Enemy_Axe : MonoBehaviour
{
    [SerializeField] private GameObject impactFx;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisual;

    private Vector3 direction;
    private Transform player;
    private float flySpeed;
    private float rotationSpeed;
    // 일정 시간동안만 direction 갱신 - 이후에는 마지막에 캐싱된 방향으로 날라감
    private float timer = 1;

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        rotationSpeed = 1600f;
        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }

    private void Update()
    {
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        this.timer -= Time.deltaTime;

        if (timer > 0)
            direction = this.player.position + Vector3.up - transform.position;


        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if (bullet != null || player != null)
        {
            GameObject newFx = ObjectPool.instance.GetObject(impactFx);
            newFx.transform.position = transform.position;

            ObjectPool.instance.ReturnObject(this.gameObject);
            ObjectPool.instance.ReturnObject(newFx, 1f);
        }
    }
}
