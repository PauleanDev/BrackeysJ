using UnityEngine;

public class Box : InteractableTool
{
    [Header("Stup")]
    [SerializeField] GameObject _foodPrefab;
    [SerializeField] SFood _food;
    [SerializeField] Transform _foodParent;

    // animation
    BoxAnim _boxAnim;

    protected override void Awake()
    {
        base.Awake();

        _boxAnim = GetComponent<BoxAnim>();
    }

    public override void Interact()
    {
        base.Interact();

        if (_playerScript.GetObjectKeeped() == null)
        {
            GameObject ingredientObj = Instantiate(_foodPrefab, transform.position, Quaternion.identity, _foodParent);

            HoldableFood holdableObject = ingredientObj.GetComponent<HoldableFood>();
            holdableObject.SetFood(_food);
            holdableObject.setPlayerParent();

            if (_boxAnim != null)
            {
                _boxAnim.PlayOpenAnim();
            }
        }
    }
}
