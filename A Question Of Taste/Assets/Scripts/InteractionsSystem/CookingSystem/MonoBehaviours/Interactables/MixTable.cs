using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class MixTable : InteractableTool
{
    [Header("Setup")]
    [SerializeField] LayerMask _playerLayer;
    PlayerAnimAudio _playerAnimAudio;
    ToolSounds _soundPlayer;
    HoldableTray _tray;
    Vector2 _trayStartPos;

    [Header("Cooking Modifiers")]
    [SerializeField] float _prepareTime;
    [SerializeField] int _prepareSteps;
    float _currentPrepareTime;
    int _onTableMax = 4;
    int _onTableMin = 3;

    [Header("Audio")]
    [SerializeField] AudioClip[] _stateAudios;

    // Foods / Ingredients / Tastes
    HoldableFood _currentHFood;
    GameObject _currentFoodObj;
    List<HoldableFood> _ingredientsAtTable = new List<HoldableFood>();
    List<SFood> _currentIngredients = new List<SFood>();
    SFood _currentSpecialIngredient;

    // States
    public bool _onTablePrepared { get; private set; } = false;
    public int _prepareStepsRemains { get; private set; } = 0;
    bool _playerNearTable = false;
    bool _playerBaking = false;
    bool _onTablePreparing = false;

    // Level event
    public override event Action PlayerGetIt;


    protected override void Awake()
    {
        base.Awake();

        _soundPlayer = GetComponent<ToolSounds>();
        _playerAnimAudio = _playerScript.gameObject.GetComponent<PlayerAnimAudio>();

        _tray = GetComponentInChildren<HoldableTray>();
        _trayStartPos = _tray.transform.position;
    }

    private void FixedUpdate()
    {
        _playerNearTable = Physics2D.OverlapCircle(transform.position, 1.5f, _playerLayer);
    }

    private void Update()
    {
        DoughPrepare();
    }

    public override void Interact()
    {
        base.Interact();

        if (_prepareStepsRemains <= 0)
        {
            if (_playerScript.GetObjectKeeped() != null)
            {
                _currentFoodObj = _playerScript.GetObjectKeeped();
                _currentHFood = _playerScript.GetObjectKeeped().GetComponent<HoldableFood>();

                if (_playerScript.GetObjectKeeped().TryGetComponent<HoldableTray>(out HoldableTray holdableTray))
                {
                    if (holdableTray.currentFood == null) 
                    {
                        _playerScript.DropObject(true);
                        holdableTray.HoldDrop();
                        holdableTray.transform.position = _trayStartPos;
                        _currentFoodObj.transform.SetParent(transform);
                    }
                }
                else if (_ingredientsAtTable.Count <= _onTableMax && !_onTablePrepared)
                {
                    _soundPlayer.PlayAudio(_stateAudios[0]);

                    if (_currentHFood.currentFood.foodState == TFoodState.ingredient) 
                    {
                        if (!_currentIngredients.Contains(_currentHFood.currentFood))
                        {
                            _currentIngredients.Add(_currentHFood.currentFood);

                            AddTable();
                        }
                    }
                    else if (_currentHFood.currentFood.foodState == TFoodState.specialIngredient)
                    {
                        if (_currentSpecialIngredient == null)
                        {
                            _currentSpecialIngredient = Instantiate(_currentHFood.currentFood);
                            AddTable();
                        }
                    }
                }
            }
            else 
            {
                if (_onTablePrepared)
                {
                    _onTablePrepared = false;
                    _currentIngredients.Clear();
                    _currentSpecialIngredient = null;
                    _tray.setPlayerParent();
                    _playerScript.HoldObject(_tray.gameObject, _tray.currentFood);
                    _currentFoodObj = null;

                    if (_playerBaking)
                    {
                        _playerBaking = false;
                        _playerAnimAudio.PlayBakeAnim();
                    }

                     PlayerGetIt?.Invoke();
                }
                else if (_ingredientsAtTable.Count >= _onTableMin && !_onTablePreparing) 
                {
                    _onTablePreparing = true;
                    _currentPrepareTime = _prepareTime;
                    _prepareStepsRemains = _prepareSteps;

                    for (int i = 0; i < _ingredientsAtTable.Count; i++)
                    {
                        Destroy(_ingredientsAtTable[i].gameObject);
                    }

                    _soundPlayer.PlayAudio(_stateAudios[1], true);
                } 
            }
        }
    }

    private void AddTable()
    {
        _playerScript.DropObject(true);
        _currentHFood.HoldDrop();

        _currentFoodObj.transform.position = new Vector2(transform.position.x + UnityEngine.Random.Range(-0.05f, 0.05f), transform.position.y + 0.25f + UnityEngine.Random.Range(-0.05f, 0.05f));
        _ingredientsAtTable.Add(_currentHFood);
    }

    private void DoughPrepare()
    {
        if (_prepareStepsRemains > 0)
        {
            if (_playerNearTable)
            {
                if (_playerAnimAudio != null && !_playerBaking)
                {
                    _playerBaking = true;
                    _playerAnimAudio.PlayBakeAnim();
                }
                    
                _currentPrepareTime -= Time.deltaTime;
            }
            else
            {
                if (_playerAnimAudio != null && _playerBaking)
                {
                    _playerBaking = false;
                    _playerAnimAudio.PlayBakeAnim();
                }
            }

            if (_currentPrepareTime <= 0)
            {
                if (_prepareStepsRemains == 3)
                {
                    _soundPlayer.StopAudio();
                }

                _prepareStepsRemains--;

                if (_prepareStepsRemains <= 0)
                {
                    if (_currentSpecialIngredient != null)
                    {
                        _tray.SetFood(_currentSpecialIngredient.foodTaste);
                    }
                    else
                    {
                        _tray.SetFood(TTaste.noTaste);
                    }
                    _currentHFood = null;

                    _onTablePreparing = false;
                    _onTablePrepared = true;
                    _ingredientsAtTable.Clear();

                    if (_playerAnimAudio != null && !_playerBaking)
                    {
                        _playerBaking = false;
                        _playerAnimAudio.PlayBakeAnim();
                    }
                }
                _currentPrepareTime = _prepareTime;
            }
        }
    }
}
