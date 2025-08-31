using UnityEngine;

public class OvenLights : MonoBehaviour
{
    [SerializeField] private Oven oven;
    [SerializeField] private GameObject ovenLight;

    private void Update()
    {
        if (oven.onOven)
        {
            ovenLight.SetActive(true);
        }
    }
}
