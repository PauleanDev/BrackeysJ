using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ScorePanelAnim : UIComponentAnim
{
    public override void AnimIn()
    {
        anim.SetBool("AnimOut", false);
        anim.SetBool("AnimIn", true);
    }

    public override void AnimOut()
    {
        anim.SetBool("AnimIn", false);
        anim.SetBool("AnimOut", true);
    }
}
