using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnInfo
    {
        public string poolTag;
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;
        public Vector3 spawnArea;
        public float spawnTime;

    }

    public SpawnInfo[] spawnInfo;

    private void Start()
    {
        foreach (SpawnInfo info in spawnInfo)
        {
            InvokeRepeating("SpawnObject", 0, info.spawnTime);
        }
    }

    private System.Collections.IEnumerator SpawnCoroutine(SpawnInfo spawnInfo)
    {
        while (true)
        {
            Vector3 randomPosition = spawnInfo.spawnPosition + new Vector3(
                Random.Range(-spawnInfo.spawnArea.x / 2, spawnInfo.spawnArea.x / 2),
                Random.Range(-spawnInfo.spawnArea.y / 2, spawnInfo.spawnArea.y / 2),
                Random.Range(-spawnInfo.spawnArea.z / 2, spawnInfo.spawnArea.z / 2)
            );

            ObjectPoolingManager.Instance.SpawnFromPool(spawnInfo.poolTag, randomPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInfo.spawnTime);
        }
    }
}
