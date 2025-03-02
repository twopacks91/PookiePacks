using UnityEngine;
using System.Collections.Generic;

public class Playable_DemonGirl : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.5f;
        Scale = 0.7f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Demon Girl");

        SetStats(Stats);


        PlayAnimation("idlecombat00");
    }

    public override int Attack()
    {
        animator.SetTrigger("Attack");
        return AttackDamage;
    }
}