using UnityEngine;

public class FlameSpawner : MonoBehaviour
{
    public ParticleSystem flameParticlePrefab;
    public int numJets = 10;
    public float spawnRadius = 5f;
    public float flameHeight = 2f;
    public float spawnInterval = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            for (int i = 0; i < numJets; i++)
            {
                SpawnFlameJet();
            }
            timer = 0;
        }
    }

    void SpawnFlameJet()
    {
        Vector3 randomPos = transform.position + Random.onUnitSphere * spawnRadius;
        randomPos.y = transform.position.y; // Keep it on the lava plane

        // Assume lava flows along XZ slope (e.g., down volcano)
        Vector3 surfaceNormal = Vector3.up;
        Vector3 lavaFlow = new Vector3(1, -0.3f, 0).normalized; // simulate flow

        // Cross to find perpendicular vector
        Vector3 flameRight = Vector3.Cross(surfaceNormal, lavaFlow);
        Vector3 flameUp = Vector3.Cross(flameRight, surfaceNormal).normalized;

        // Final direction is slightly randomized upward flame
        Vector3 finalDirection = (flameUp + Random.insideUnitSphere * 0.2f).normalized;

        // Instantiate particle
        var ps = Instantiate(flameParticlePrefab, randomPos, Quaternion.LookRotation(finalDirection));
        ps.transform.forward = finalDirection;

        // Optional: Add force in that direction
        var main = ps.main;
        main.startSpeed = 2f;
    }
}
