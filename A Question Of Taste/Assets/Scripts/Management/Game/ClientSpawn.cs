using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientSpawn : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] SDialogueBank _dialogueBank;
    [SerializeField] GameObject _clientPrefab;
    [SerializeField] Chair[] _chairs;

    List<Chair> _currentEmptyChairs = new List<Chair>();

    [Header("Modifiers")]
    [SerializeField] float _spawnTime;
    float _currentSpawnTime;
    bool _spawnPerTime = false;

    private void Awake()
    {
        _currentSpawnTime = _spawnTime;
        StartCoroutine(SpawnPerTime());

        ClientInteraction.Avaliated += OnAvaliated;
        for (int i = 0; i < _chairs.Length; i++)
        {
            _chairs[i].PlayerLeft += OnPlayerLeft;
        }
    }

    private void OnDestroy()
    {
        ClientInteraction.Avaliated -= OnAvaliated;
    }

    public void Setup(bool spawnPerTime)
    {
        for (int i = 0; i < _chairs.Length; i++)
        {
            _currentEmptyChairs.Add(_chairs[i]);
        }

        Spawn();

        _spawnPerTime = spawnPerTime;
    }

    IEnumerator SpawnPerTime()
    {
        while (_currentSpawnTime > 0)
        {
            _currentSpawnTime -= Time.deltaTime;
            yield return null;
        }

        Spawn();

        _currentSpawnTime = _spawnTime;
        StartCoroutine(SpawnPerTime());
    }

    public void Spawn()
    {
        if (_currentEmptyChairs.Count > 0)
        {
            GameObject clientObj = Instantiate(_clientPrefab, transform.position, Quaternion.identity);
            ClientDisplacement client = clientObj.GetComponent<ClientDisplacement>();

            Chair currentChair = _currentEmptyChairs[Random.Range(0, _currentEmptyChairs.Count)];
            client.Setup(currentChair);

            _currentEmptyChairs.Remove(currentChair);
        }
    }

    public void OnAvaliated(int rating)
    {
        if (!_spawnPerTime)
        {
            Spawn();
        }
    }

    public void OnPlayerLeft(Chair chair)
    {
        if (!_currentEmptyChairs.Contains(chair))
        {
            _currentEmptyChairs.Add(chair);
        }
    }
}
