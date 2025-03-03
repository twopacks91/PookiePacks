using System.Collections.Generic;
using UnityEngine;
public class Playable_Croc : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.5f;
        Scale = 1f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Jawski");

        SetStats(Stats);


        //PlayAnimation("idlecombat00");
    }
}