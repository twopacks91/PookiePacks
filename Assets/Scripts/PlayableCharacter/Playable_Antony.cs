using System.Collections.Generic;
using UnityEngine;
public class Playable_Antony : PlayableCharacter
{
    protected override void Start()
    {
        x = -1f;
        Scale = 0.7f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Antony");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}