using System.Collections;
using UnityEngine;

public class SetCandles : MonoBehaviour
{
    [SerializeField] private GameObject[] candles;
    public GameObject dropedCandle;

    public bool isLit { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isLit = true;
            for (int i = 0; i < candles.Length; i++)
            {
                candles[i].SetActive(true);
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
