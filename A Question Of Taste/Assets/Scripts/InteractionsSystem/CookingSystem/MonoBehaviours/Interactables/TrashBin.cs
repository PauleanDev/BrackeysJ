using UnityEngine;

public class TrashBin : InteractableTool
{
    [Header("Setup")]
    GameObject _foodObj;

    public override void Interact()
    {
        base.Interact();

        if (_playerScript.GetObjectKeeped().TryGetComponent<HoldableTray>(out HoldableTray tray))
        {
            tray.ClearTray();
        }
        else
        {
            _foodObj = _playerScript.DropObject(true);
            Destroy(_foodObj);
        }
    }
}
