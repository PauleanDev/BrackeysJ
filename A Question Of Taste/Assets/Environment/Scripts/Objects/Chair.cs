using UnityEngine;

public class Chair : MonoBehaviour
{
    private bool emp;

    public bool empty
    {
        get
        {
            return emp;
        }
        set
        {
            emp = value;
        }
    }
    private void Awake()
    {
        empty = true;
    }
}
