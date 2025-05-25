using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public float timeToDestroy;
    private float shootTimer;

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
