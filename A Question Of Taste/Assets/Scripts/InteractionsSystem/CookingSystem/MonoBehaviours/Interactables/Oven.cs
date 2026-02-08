using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : InteractableTool
{
    [Header("Setup")]
    [SerializeField] SFoodBank _foodBank;
    [SerializeField] Transform _cookieParent;
    [SerializeField] LayerMask _playerLayer;
    SFoodBank _cookies;
    TFoodState _foodCondition = TFoodState.raw;
    OvenAnim _ovenAnim;

    [Header("Modifiers")]
    [SerializeField] float _prepareTime;
    [SerializeField] float _burnTime;

    // Auxiliaries
    GameObject _foodObj;
    SFood _currentFood;

    // States
    public bool _onOven { get; private set; } = false;
    public bool _onOvenPrepared { get; private set; } = false;

    // Other
    public override event Action PlayerGetIt;

    protected override void Awake()
    {
        base.Awake();

        _ovenAnim = GetComponent<OvenAnim>();
        _cookies = Instantiate(_foodBank);
    }
    public override void Interact()
    {
        base.Interact();

        if (_playerScript.GetObjectKeeped() != null)
        {
            _foodObj = _playerScript.DropObject(false);
            if (_playerScript.GetObjectKeeped().TryGetComponent<HoldableTray>(out HoldableTray holdableTray) && holdableTray.currentFood != null)
            {
                if (!_onOven)
                {
                    for (int i = 0; i < _cookies.foods.Length; i++)
                    {
                        if (holdableTray.currentFood.foodTaste == _cookies.foods[i].foodTaste)
                        {
                            _currentFood = Instantiate(_cookies.foods[i]);
                            break;
                        }
                    }

                    if (_ovenAnim != null)
                    {
                        _ovenAnim.PlayOpenAnim(_onOven);
                    }

                    _onOven = true;

                    StartCoroutine("CookiePrepare");
                    holdableTray.ClearTray();
                }
            }
            else if(_foodObj.TryGetComponent<HoldableFood>(out HoldableFood holdablePackage))
            {
                if (holdablePackage.FoodCondition == TFoodState.package && _onOvenPrepared)
                {
                    holdablePackage.FoodCondition = _foodCondition;
                    if (_foodCondition == TFoodState.burnt)
                    {
                        holdablePackage.SetFood(_cookies.foods[1]);
                        _playerScript.HoldObject(_foodObj);
                    }
                    else 
                    {
                        holdablePackage.SetFood(_currentFood);
                        _playerScript.HoldObject(_foodObj, _currentFood);
                    }

                    if (_ovenAnim != null)
                    {
                        _ovenAnim.PlayOpenAnim(_onOven);
                    }

                    _onOvenPrepared = false;
                    _onOven = false;

                    _ovenAnim.StopAudio();

                    _currentFood = null;
                    _foodObj = null;
                    StopAllCoroutines();
                }
            }
        }
    }

    private IEnumerator CookiePrepare()
    {
        _foodCondition = TFoodState.raw;
        _onOvenPrepared = false;

        yield return new WaitForSeconds(_prepareTime);

        _foodCondition = TFoodState.cooked;
        _onOvenPrepared = true;

        _ovenAnim.PlayDoneAnim(false);

        PlayerGetIt?.Invoke();

        yield return new WaitForSeconds(_burnTime);

        Debug.Log("Burnt in Oven");
        _foodCondition = TFoodState.burnt;

        _ovenAnim.PlayDoneAnim(true);
    }
}
