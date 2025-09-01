using System.Collections;
using UnityEngine;

public class SetCandles : MonoBehaviour
{
    [SerializeField] private GameObject[] candles;
    [SerializeField] private LayerMask player;

    [SerializeField] private float collisionRadious;
    public GameObject dropedCandle;

    public bool isLit { get; private set; } = false;
    private bool playerInside = false;

    private void Update()
    {
        if (!playerInside)
        {
            playerInside = Physics2D.OverlapCircle(transform.position, collisionRadious, player);

            if (playerInside)
            {
                Debug.Log("Executed");
                isLit = true;
                for (int i = 0; i < candles.Length; i++)
                {
                    candles[i].SetActive(true);
                }
            }
        }
    }

    public void UnlitCandles()
    {
        for (int i = 0; i < candles.Length; i++)
        {
            Candle candle = candles[i].GetComponent<Candle>();
            candle.CandleUnlit();
        }
    }

    public void LitActive()
    {
        dropedCandle.SetActive(true);
    }

    public void LitInactive()
    {
        candles[0].SetActive(false);
    }
}
