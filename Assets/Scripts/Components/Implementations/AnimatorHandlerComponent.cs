using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandlerComponent : MonoBehaviour, IAnimatorHandler
{
    [SerializeField] private Dictionary<string, string> animationNames;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    public void StopAnimation(string animationName)
    {
        Debug.Log("Stop Animation");
    }
}
