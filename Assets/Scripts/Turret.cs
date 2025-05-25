using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 2.0f;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float projectileSpeed = 10f;
    public Transform shootPoint;

    private float shootTimer;

    void Update()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        
        Vector3 forward = transform.forward;
        Vector3 cross = Vector3.Cross(forward, toPlayer.normalized);

        float dot = Vector3.Dot(forward.normalized, toPlayer.normalized);
        float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle * rotationSpeed * Time.deltaTime, cross.normalized);
        transform.rotation = rotation * transform.rotation;

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 shootDir = (player.position - shootPoint.position).normalized;
        rb.linearVelocity = shootDir * projectileSpeed;

    }
}
