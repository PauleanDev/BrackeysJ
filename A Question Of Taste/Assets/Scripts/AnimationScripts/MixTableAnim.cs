using UnityEngine;

public class MixTableAnim : MonoBehaviour
{
    private MixTable mixTable;

    [SerializeField] private Animator mixerAnim;
    [SerializeField] private Animator boardAnim;

    [SerializeField] private AnimationClip mixerMixing;
    [SerializeField] private AnimationClip boardRolling;
    [SerializeField] private AnimationClip boardCutting;

    private bool idle = true;

    private void Awake()
    {
        mixTable = GetComponent<MixTable>();
    }

    private void Update()
    {
        switch (mixTable._prepareStepsRemains)
        {
            case 0:
                if (!idle)
                {
                    boardAnim.Rebind();
                    mixerAnim.Rebind();

                    idle = true;
                }
                break;
            case 1:
                idle = false;
                boardAnim.Play(boardCutting.name);
                mixerAnim.Rebind();
                break;
            case 2:
                idle = false;
                boardAnim.Play(boardRolling.name);
                mixerAnim.Rebind();
                break;
            case 3:
                idle = false;
                mixerAnim.Play(mixerMixing.name);
                boardAnim.Rebind();
                break;
        }
    }



}
