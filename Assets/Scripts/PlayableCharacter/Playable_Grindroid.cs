using System.Collections.Generic;
using UnityEngine;
public class Playable_Grindroid : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.5f;
        Scale = 0.7f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Grindroid");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}