using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class CompositeAnimation
{
    public Sprite[] skinSprites;
    public Sprite[] hairSprites;
}

public class ClientAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer _hairRenderer;

    NavMeshAgent _agent;
    Animator _animator;
    [SerializeField] CompositeAnimation _compositeAnimations;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (_agent.remainingDistance != 0) 
        {
            _animator.SetBool("Walking", true);
        }
        else
        {
            _animator.SetBool("Walking", false);
        }
    }

    public void SetupSkin(RuntimeAnimatorController animator)
    {
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = animator;
    }

    public void SetupHair(Sprite[] sprites)
    {
        _compositeAnimations.hairSprites = sprites;
    }

    /// <summary>
    /// Animates the client's body components components based on the pixel displacement
    /// </summary>
    /// <param name="pixel">say the y-axis to follow the character displacement with the idle lower sprite frame base</param>
    public void ComponentsAnimate(int pixel)
    {
        _hairRenderer.sprite = _compositeAnimations.hairSprites[pixel];
    }
}
