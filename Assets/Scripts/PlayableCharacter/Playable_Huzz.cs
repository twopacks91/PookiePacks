using System.Collections.Generic;
using UnityEngine;
public class Playable_Huzz : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.5f;
        Scale = 0.6f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Huzz");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}