using UnityEngine;

public abstract  class PlayableCharacter : MonoBehaviour
{
    protected int Health;
    protected int MaxHealth;
    protected int AttackDamage;
    protected float Scale;

    protected Animator animator;


    protected virtual void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
    }

    public virtual int Attack()
    {
        return AttackDamage;
    }
    public void PlayAnimation(string animationName)
    {
        // Play the animation by name
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health < 0) Health = 0;
    }
}

