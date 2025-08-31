using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Candle : MonoBehaviour
{
    private Animator animator;
    private Light2D light;


    private void Awake()
    {
        light = GetComponentInChildren<Light2D>();
        animator = GetComponent<Animator>();
    }

    public void CandleUnlit()
    {
        light.enabled = false;
        animator.SetBool("Melted", true);
    }
}
