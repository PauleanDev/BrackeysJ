using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject cookie;
    [SerializeField] private Transform cookieParent;
    [SerializeField] private LayerMask playerLayer;
    private HoldableFood holdableFood;
    private GameObject foodObj;

    [SerializeField] private float prepareTime;
    [SerializeField] private float burnTime;

    private bool onOven = false;
    private bool onOvenPrepared = false;

    public void Interact()
    {
        Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        if (playerScript.holdingObj)
        {
            foodObj = playerScript.DropObject(false);
            holdableFood = foodObj.GetComponent<HoldableFood>();

            if (holdableFood.FoodCondition == 1 && !onOven)
            {
                onOven = true;
                StartCoroutine("CookiePrepare");

                playerScript.DropObject(true);
                foodObj.transform.position = transform.position;

                holdableFood.HoldDrop();
            }
        }
        else
        {
            if (onOvenPrepared)
            {
                onOvenPrepared = false;
                onOven = false;

                holdableFood.FoodCondition = 2;
                holdableFood.setPlayerParent();
                playerScript.HoldObject(foodObj);
                foodObj = null;
            }
        }

    }

    private IEnumerator CookiePrepare()
    {
        onOvenPrepared = false;

        yield return new WaitForSeconds(prepareTime);

        holdableFood.FoodCondition = 2;
        onOvenPrepared = true;

        yield return new WaitForSeconds(burnTime);

        holdableFood.FoodCondition = 3;
    }
}
