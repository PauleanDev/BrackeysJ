using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameScript : MonoBehaviour
{
    [Header("Game Setup")]
    [SerializeField] GameManager _gameManager;
    [SerializeField] SLevelEventsBank _levelEventsBank;
    [SerializeField] SLevelScriptsBank _levelScriptsBank;
    [SerializeField] NavMeshSurface _meshSurface;
    [SerializeField] Light2D _globalLight;
    [SerializeField] ClientSpawn _clientSpawner;

    SLevelEvent _currentLevelEvents;
    SLevelScript _currentLevelScript;
    UIManagement _uIManager;

    AudioSource source;

    [SerializeField] AudioClip storm;

    List<InteractableTool> currentTools;
    int currentEvent;
    int level;

    private void Awake()
    {
        currentTools = new List<InteractableTool>();

        level = PlayerPrefs.GetInt("Level");

        source = GetComponent<AudioSource>();
        _gameManager = GetComponent<GameManager>();
        _uIManager = GetComponent<UIManagement>();

        _currentLevelScript = _levelScriptsBank.levels[level];
        _uIManager.LevelSetup(_currentLevelScript.starsScore);

        Invoke("TurnOfTheLights", GameManager.startTime);
        Invoke("PlayLevelSong", GameManager.startTime);

        SetupGameScript();
    }

    private void Start()
    {
        _meshSurface.BuildNavMesh();
        _clientSpawner.Setup(_currentLevelScript.clientsSpawnPerTime);
        _gameManager.Setup(_currentLevelScript);
    }

    private void OnDestroy()
    {
        ClientInteraction.Avaliated -= OnAvaliated;
    }

    public void SetLevelSObjects(InteractableTool tool)
    {
        currentTools.Add(tool);
    }

    public void PlayLevelSong()
    {
        source.loop = true;
        source.clip = _levelScriptsBank.levels[level].music;
        source.Play();
    }

    private void SetupGameScript()
    {
        // Set the Level Objects
        for (int i = 0; i < _currentLevelScript.levelObject.Length; i++)
        {
            GameObject levelObject = Instantiate(_currentLevelScript.levelObject[i].objPrefab, _currentLevelScript.levelObject[i].objTransform, Quaternion.identity);

            if (levelObject.TryGetComponent<InteractableTool>(out InteractableTool tool))
            {
                SetLevelSObjects(tool);
            }
        
        }
        _globalLight.intensity = _currentLevelScript.isDarkLevel ? 0 : 1;

        // Set the Level Events
        for (int i = 0; i < _levelEventsBank.levelEvent.Length; i++)
        {
            if (_levelEventsBank.levelEvent[i].level == level)
            {
                _uIManager.SetInteractMessages(_levelEventsBank.levelEvent[level].pGetItMassages, currentTools.ToArray());

                _currentLevelEvents = _levelEventsBank.levelEvent[i];
                CandleEventSetup(_currentLevelEvents.levelEvents);

                if (_currentLevelEvents.finishOnAvaliated)
                {
                    ClientInteraction.Avaliated += OnAvaliated;
                }

                break;
            }
        }
    }


    public void CandleEventSetup(TLevelEvents levelEvents)
    {
        if (levelEvents.firstOnAwake)
        {
            _uIManager.OnPlayerGetIt();
            Invoke("NextLevelEvent", 2f);
        }

        for (int i = 0; i < currentTools.Count; i++)
        {
            currentTools[i].PlayerGetIt += NextLevelEvent;
        }
    }

    public void NextLevelEvent()
    {
        if (_currentLevelEvents.levelEvents.candleEventSetup[currentEvent].candlesPosition != null)
        {
            StartCoroutine(FlashingLight(3));

            for (int i = 0; i < _currentLevelEvents.levelEvents.candleEventSetup[currentEvent].candlesPosition.Length; i++)
            {
                Instantiate(_levelEventsBank.candlePrefab, _currentLevelEvents.levelEvents.candleEventSetup[currentEvent].candlesPosition[i], Quaternion.identity);
            }
        }
        currentEvent++;
    }

    private IEnumerator FlashingLight(int flashingTimes)
    {
        while (flashingTimes >= 0)
        {
            flashingTimes--;
            if (_globalLight.intensity < 1)
            {
                _globalLight.intensity = 1;
            }
            else
            {
                _globalLight.intensity = 0;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnAvaliated(int rating)
    {
        Invoke("ForceFinishLevel", 1f);
    }

    private void ForceFinishLevel()
    {
        _gameManager.FinishLevel();
    }

}
