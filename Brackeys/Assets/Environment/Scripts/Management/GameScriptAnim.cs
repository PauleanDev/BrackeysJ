using UnityEngine;
using System.Collections;

public class GameScriptAnim : MonoBehaviour
{
    [SerializeField] private GameManagement gameManagement;
    [SerializeField] private GameObject lights;
    private AudioSource source;

    [SerializeField] AudioClip storm;

    [SerializeField] private MixTable mixTable;
    [SerializeField] private Oven oven;
    [SerializeField] private GameObject ovenLight;

    private int Stage = 0;
    private int flashingTimes = 3;
    [SerializeField] private SetCandles[] candlesDetectors;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        gameManagement = GetComponent<GameManagement>();
        Invoke("TurnOfTheLights", GameManagement.startGame);

        ClientInteraction.Avaliated += OnAvaliated;
    }

    private void Update()
    {
        switch (Stage)
        {
            case 0:
                if (candlesDetectors[0].isLit && candlesDetectors[1].isLit)
                {
                    flashingTimes = 2;
                    StartCoroutine("FlashingLight");

                    Stage = 1;
                }
                break;
            case 1:
                if (mixTable.onTablePrepared)
                {
                    flashingTimes = 2;
                    StartCoroutine("FlashingLight");
                    lights.SetActive(false);
                    ovenLight.SetActive(true);

                    Stage = 2;
                }
                break;
            case 2:
                if (oven.onOven)
                {
                    Debug.Log("Oven is on");
                    flashingTimes = 5;
                    StartCoroutine("FlashingLight");
                    candlesDetectors[0].UnlitCandles();
                    candlesDetectors[1].UnlitCandles();
                    candlesDetectors[1].LitInactive();
                    candlesDetectors[1].LitActive();
                    ovenLight.SetActive(false);

                    Stage = 3;
                }
                break;
            case 3:
                if (oven.onOvenPrepared)
                {
                    flashingTimes = 2;
                    StartCoroutine("FlashingLight");

                    Stage = 4;
                }
                break;
        }
    }

    private void TurnOfTheLights()
    {
        source.clip = storm;
        source.loop = true;
        source.Play();
        lights.SetActive(false);
    }
    private IEnumerator FlashingLight()
    {
        while (flashingTimes >= 0)
        {
            flashingTimes--;
            if (lights.activeSelf)
            {
                lights.SetActive(false);
            }
            else
            {
                lights.SetActive(true);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnAvaliated(int rating)
    {
        gameManagement.FinishLevel();
    }
}
