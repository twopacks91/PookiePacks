using System.IO;
using UnityEngine;
using System.Collections.Generic;

public abstract class PlayableCharacter : MonoBehaviour
{
    protected int Health;
    protected int MaxHealth;
    protected int AttackDamage;
    protected int Defence;
    protected float Scale;
    protected float RotationX = 90;
    protected float RotationY = 0;
    protected float RotationZ = 0;
    protected float x = 0, y = 0, z = 2.2f;

    protected Animator animator;


    protected virtual void Start()
    {
        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();

        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.position = new Vector3(x, y, z);
        transform.rotation = Quaternion.LookRotation(new Vector3(RotationX, RotationY, RotationZ));
    }

    protected void SetStats(List<int> Stats)
    {
        MaxHealth = Stats[0];
        AttackDamage = Stats[1];
        Defence = Stats[2];
        Health = MaxHealth;
    }
    public void SetOpponentPosition()
    {
        transform.position = new Vector3(x * -1, y, z);
        transform.rotation = Quaternion.LookRotation(new Vector3(-90, 0, 0));
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

    public void TakeDamage(int amount, bool penalty)
    {
        Debug.Log("Health before: " + Health);
        if (penalty)
        {
            Health -= amount;
        }
        else if (!penalty)
        {
            Health -= (amount / (1 + (Defence / 105)));
            Debug.Log("Non Pen Damage: " + (amount / (1 + (Defence / 105))));
        }

        Debug.Log("Health After: " + Health);

        if (Health < 0) Health = 0;

    }

    public int GetHealth()
    {
        return Health;
    }

    public int GetDefence()
    {
        return Defence;
    }
}

