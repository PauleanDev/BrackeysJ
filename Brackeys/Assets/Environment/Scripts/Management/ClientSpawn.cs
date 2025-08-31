using UnityEngine;

public class ClientSpawn : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    private float atualSpawnTime;

    [SerializeField] private Chair[] chairs;
    [SerializeField] private GameObject clientPrefab;

    [SerializeField] private bool perTime = false;


    private void Awake()
    {
        ClientInteraction.Avaliated += OnAvaliated;
    }

    private void Start()
    {
        atualSpawnTime = spawnTime;
        Spawn();
    }

    private void Update()
    {
        if (perTime)
        {
            SpawnPerTime();
        }
    }

    private void SpawnPerTime()
    {
        atualSpawnTime -= Time.deltaTime;

        if (atualSpawnTime <= 0)
        {
            GameObject clientObj = Instantiate(clientPrefab, transform.position, Quaternion.identity);
            Client client = clientObj.GetComponent<Client>();

            for (int i = 0; i < chairs.Length; i++)
            {
                if (!chairs[i].empty && i == chairs.Length)
                {
                    Invoke("Spawn", spawnTime / 2);
                }
                else if (chairs[i].empty)
                {
                    client.Setup(chairs[i].transform.position);
                    break;
                }
            }
        }
    }

    public void Spawn()
    {
        GameObject clientObj = Instantiate(clientPrefab, transform.position, Quaternion.identity);

        Client client = clientObj.GetComponent<Client>();

        client.Setup(chairs[Random.Range(0, chairs.Length - 1)].transform.position);
    }

    public void OnAvaliated(int rating)
    {
        Spawn();
    }
}
