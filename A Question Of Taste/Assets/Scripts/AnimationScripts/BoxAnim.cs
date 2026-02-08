using UnityEngine;

public class BoxAnim : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private AnimationClip openBoxAnim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    public void PlayOpenAnim()
    {
        anim.Rebind();
        anim.Play(openBoxAnim.name);
    }
}
