using UnityEngine;

public abstract class UIComponentAnim : MonoBehaviour
{
    protected Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public abstract void AnimIn();

    public abstract void AnimOut();
}
