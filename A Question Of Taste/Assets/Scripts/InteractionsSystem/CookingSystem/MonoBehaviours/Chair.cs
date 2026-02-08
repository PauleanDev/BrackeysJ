using System;
using UnityEngine;

public class Chair : MonoBehaviour
{
    // States
    bool _empty = true;

    // Events
    public event Action<Chair> PlayerLeft;

    public bool Empty
    {
        get
        {
            return _empty;
        }
        set
        {
            if (value)
            {
                PlayerLeft(this);
            }

            _empty = value;
        }
    }
}
