using System.Collections.Generic;
using UnityEngine;
public class Playable_Lesssgooo : PlayableCharacter
{
    protected override void Start()
    {
        x = -1.5f;
        Scale = 0.5f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Lesss Gooo");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}