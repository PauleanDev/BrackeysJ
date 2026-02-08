using System.Collections;
using UnityEngine;

public class CandleManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject[] _candles;

    [Header("Modifiers")]
    [SerializeField] LayerMask _player;
    [SerializeField] float _collisionRadius;

    bool _playerInside = false;

    private void Update()
    {
        if (!_playerInside)
        {
            _playerInside = Physics2D.OverlapCircle(transform.position, _collisionRadius, _player);

            if (_playerInside)
            {
                for (int i = 0; i < _candles.Length; i++)
                {
                    _candles[i].SetActive(true);
                }
            }
        }
    }
}
