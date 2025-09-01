using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Candle : MonoBehaviour
{
    private Animator animator;
    private Light2D lightCandle;


    private void Awake()
    {
        lightCandle = GetComponentInChildren<Light2D>();
        animator = GetComponent<Animator>();
    }

    public void CandleUnlit()
    {
        lightCandle.intensity = 0;
        animator.SetBool("Melted", true);
    }
}
