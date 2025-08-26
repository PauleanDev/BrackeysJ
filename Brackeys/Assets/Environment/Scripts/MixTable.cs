using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class MixTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject cookie;
    [SerializeField] private Transform cookieParent;
    [SerializeField] private LayerMask playerLayer;
    private HoldableFood holdableFood;
    private List<GameObject> foodsAtTable = new List<GameObject>(); 
    private GameObject foodObj;

    [SerializeField] private float prepareTime;
    private float atualPrepareTime;
    [SerializeField] private int prepareSteps;
    private int prepareStepsRemains = 0;
    private int onTableMax = 5;
    private int onTableMin = 3;

    private bool playerNearTable = false;
    private bool mixed = false;
    private bool onTablePrepared = false;
    private bool onTablePreparing = false;

    private void FixedUpdate()
    {
        playerNearTable = Physics2D.OverlapCircle(transform.position, 2f, playerLayer);
    }

    private void Update()
    {
        DoughPrepare();
    }

    public void Interact()
    {
        if (prepareStepsRemains <= 0)
        {
            Player playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (playerScript.holdingObj)
            {
                if (foodsAtTable.Count <= onTableMax && !onTablePrepared)
                {
                    foodObj = playerScript.DropObject();
                    holdableFood = foodObj.GetComponent<HoldableFood>();

                    foodObj.transform.position = new Vector2(transform.position.x + Random.Range(-0.5f,0.5f), transform.position.y + Random.Range(-0.5f, 0.5f));

                    holdableFood.HoldDrop();
                    foodsAtTable.Add(foodObj);
                }
            }
            else 
            {
                Debug.Log("ta clickando de mão nua");
                if (onTablePrepared)
                {
                    onTablePrepared = false;

                    holdableFood.FoodCondition = true;
                    holdableFood.setPlayerParent();
                    playerScript.HoldObject(foodObj);
                    foodObj = null;
                    mixed = false;
                }
                else if (foodsAtTable.Count >= onTableMin && !onTablePreparing) 
                {
                    Debug.Log("foi");
                    onTablePreparing = true;
                    prepareStepsRemains = prepareSteps;
                    atualPrepareTime = prepareTime;
                } 
            }
        }
    }

    private void DoughPrepare()
    {
        if (prepareStepsRemains > 0)
        {
            Debug.Log("charging");
            if (playerNearTable)
            {
                atualPrepareTime -= Time.deltaTime;
            }

            if (atualPrepareTime <= 0)
            {
                prepareStepsRemains--;

                if (prepareStepsRemains <= 0)
                {
                    onTablePreparing = false;
                    onTablePrepared = true;
                    foodsAtTable.Clear();
                }

                foodObj.transform.localScale = Vector3.right * 1.25f + Vector3.up * 1.25f;
                atualPrepareTime = prepareTime;
            }
        }

        if (prepareStepsRemains == prepareSteps - 1 && !mixed)
        {
            Debug.Log(prepareStepsRemains);
            mixed = true;
            for (int i = 0; i < foodsAtTable.Count; i++)
            {
                Destroy(foodsAtTable[i]);
            }

            foodObj = Instantiate(cookie, transform.position, Quaternion.identity, cookieParent);
            holdableFood = foodObj.GetComponent<HoldableFood>();
        }
    }
}
