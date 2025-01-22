using UnityEngine;
public class Playable_DemonGirl : PlayableCharacter
{
    protected override void Start()
    {
        base.Start();  // Call the base Start method
        // Custom initialization for DemonGirl
        MaxHealth = 100;  // Example: setting the max health for DemonGirl
        AttackDamage = 25; // Example: setting attack damage for DemonGirl
        Health = MaxHealth;  // Set initial health to max health
        Scale = 0.7f;
        transform.localScale = new Vector3(Scale, Scale, Scale);
        transform.position = new Vector3(-0.5f, 0, 2.2f);
        transform.rotation = Quaternion.LookRotation(new Vector3(90, 0, 0));

        PlayAnimation("idlecombat00");
    }

    public override int Attack()
    {
        animator.SetTrigger("Attack");
        return AttackDamage;
    }
}