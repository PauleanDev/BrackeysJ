using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class MixTable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject cookie;
    [SerializeField] private Transform cookieParent;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private ToolSounds MixerSound;

    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    private HoldableFood holdableFood;
    private List<HoldableFood> foodsAtTable = new List<HoldableFood>(); 
    private GameObject foodObj;

    private HoldableTray tray;
    private Vector2 trayStartPos;

    [SerializeField] private float prepareTime;
    [SerializeField] private int prepareSteps;
    private float atualPrepareTime;

    [SerializeField] private Tastes atualTaste = new Tastes();
    private bool[] currentIngredients = new bool[3] { false, false, false};

    public int prepareStepsRemains { get; private set; } = 0;
    private int onTableMax = 4;
    private int onTableMin = 3;

    private bool playerNearTable = false;
    public bool onTablePrepared { get; private set; } = false;
    private bool onTablePreparing = false;

    private Animator playerAnim;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        tray = GetComponentInChildren<HoldableTray>();
        trayStartPos = tray.transform.position;
    }

    private void FixedUpdate()
    {
        playerNearTable = Physics2D.OverlapCircle(transform.position, 1.5f, playerLayer);
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
            playerAnim = playerScript.GetComponent<Animator>();

            if (playerScript.holdingObj)
            {
                foodObj = playerScript.DropObject(false);
                holdableFood = foodObj.GetComponent<HoldableFood>();

                if (playerScript.objectKeeped.TryGetComponent<HoldableTray>(out HoldableTray holdableTray))
                {
                    playerScript.DropObject(true);
                    holdableTray.HoldDrop();
                    holdableTray.transform.position = trayStartPos;
                    foodObj.transform.SetParent(transform);
                }
                else if (foodsAtTable.Count <= onTableMax && !onTablePrepared && holdableFood.FoodCondition == 0)
                {
                    audioSource.PlayOneShot(clip);
                    if (holdableFood.food.isIngredient) 
                    {
                        int ingredient = holdableFood.food.ingredientID;

                        if (!currentIngredients[holdableFood.food.ingredientID])
                        {
                            currentIngredients[ingredient] = true;

                            AddTable(playerScript);
                        }
                    }
                    else
                    {
                        atualTaste.sweetness += holdableFood.taste.sweetness;
                        atualTaste.spicy += holdableFood.taste.spicy;
                        atualTaste.salty += holdableFood.taste.salty;

                        AddTable(playerScript);
                    }
                }
            }
            else 
            {
                if (onTablePrepared)
                {
                    onTablePrepared = false;

                    holdableFood.FoodCondition = 1;
                    currentIngredients = new bool[3] { false, false, false };

                    holdableFood.setPlayerParent();

                    foodObj = tray.gameObject;

                    playerScript.HoldObject(foodObj, atualTaste);
                    playerAnim.SetBool("Working", false);

                    atualTaste = new Tastes();
                    foodObj = null;
                }
                else if (foodsAtTable.Count >= onTableMin && !onTablePreparing) 
                {
                    onTablePreparing = true;
                    atualPrepareTime = prepareTime;
                    prepareStepsRemains = prepareSteps;

                    for (int i = 0; i < foodsAtTable.Count; i++)
                    {
                        Destroy(foodsAtTable[i].gameObject);
                    }

                    MixerSound.PlayAudio(0, true);
                } 
            }
        }
    }

    private void AddTable(Player playerScript)
    {
        playerScript.DropObject(true);
        foodObj.transform.position = new Vector2(transform.position.x + Random.Range(-0.05f, 0.05f), transform.position.y + 0.25f + Random.Range(-0.05f, 0.05f));

        foodsAtTable.Add(holdableFood);
        holdableFood.HoldDrop();
    }


    private void DoughPrepare()
    {
        if (prepareStepsRemains > 0)
        {
            if (playerNearTable)
            {
                if (playerAnim != null)
                {
                    playerAnim.SetBool("Working", true);
                }
                    
                atualPrepareTime -= Time.deltaTime;
            }
            else
            {
                if (playerAnim != null)
                {
                    playerAnim.SetBool("Working", false);
                }
            }

            if (atualPrepareTime <= 0)
            {
                if (prepareStepsRemains == 3)
                {
                    MixerSound.StopAudio();
                }

                prepareStepsRemains--;

                if (prepareStepsRemains <= 0)
                {
                    tray.KeepTaste(atualTaste);
                    holdableFood = tray;

                    onTablePreparing = false;
                    onTablePrepared = true;
                    foodsAtTable.Clear();
                }

                    
                atualPrepareTime = prepareTime;
            }
        }
    }
}
