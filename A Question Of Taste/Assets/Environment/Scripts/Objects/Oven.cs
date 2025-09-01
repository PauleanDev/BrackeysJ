using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IInteractable
{
    private OvenAnim ovenAnim;

    [SerializeField] private FoodBank cookies;
    [SerializeField] private Transform cookieParent;
    [SerializeField] private LayerMask playerLayer;
    public int foodCondition { get; private set; } = 1;
    private GameObject foodObj;
    private Food atualCookie;

    [SerializeField] private float prepareTime;
    [SerializeField] private float burnTime;

    private Tastes atualTaste;

    public bool onOven { get; private set; } = false;
    public bool onOvenPrepared { get; private set; } = false;

    private void Awake()
    {
        ovenAnim = GetComponent<OvenAnim>();
    }
    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        if (playerScript.holdingObj)
        {
            foodObj = playerScript.DropObject(false);
            if (playerScript.objectKeeped.TryGetComponent<HoldableTray>(out HoldableTray holdableTray))
            {
                atualTaste = playerScript.currentObjectTaste;
                if (!onOven)
                {
                    for (int i = 0; i < cookies.foods.Length; i++)
                    {
                        if (atualTaste.sweetness == cookies.foods[i].taste.sweetness && atualTaste.spicy == cookies.foods[i].taste.spicy && atualTaste.salty == cookies.foods[i].taste.salty)
                        {
                            atualCookie = cookies.foods[i];
                            break;
                        }
                    }

                    if (ovenAnim != null)
                    {
                        ovenAnim.PlayOpenAnim(onOven);
                    }

                    onOven = true;

                    StartCoroutine("CookiePrepare");
                    holdableTray.ClearTray();
                }
            }
            else
            {
                HoldableFood holdable = foodObj.GetComponent<HoldableFood>();
                if (holdable.FoodCondition == 2 && onOvenPrepared)
                {
                    holdable.FoodCondition = foodCondition;
                    if (foodCondition == 4)
                    {
                        holdable.SetFood(cookies.foods[1]);
                        playerScript.HoldObject(foodObj);
                    }
                    else 
                    {
                        holdable.SetFood(atualCookie);
                        playerScript.HoldObject(foodObj, atualTaste);
                    }

                    if (ovenAnim != null)
                    {
                        ovenAnim.PlayOpenAnim(onOven);
                    }

                    onOvenPrepared = false;
                    onOven = false;

                    ovenAnim.StopAudio();

                    atualTaste = null;
                    foodObj = null;
                    StopAllCoroutines();
                }
            }
        }
    }

    private IEnumerator CookiePrepare()
    {
        foodCondition = 1;
        onOvenPrepared = false;

        yield return new WaitForSeconds(prepareTime);

        foodCondition = 3;
        onOvenPrepared = true;

        ovenAnim.PlayDoneAnim(false);

        yield return new WaitForSeconds(burnTime);

        foodCondition = 4;

        ovenAnim.PlayDoneAnim(true);
    }
}
