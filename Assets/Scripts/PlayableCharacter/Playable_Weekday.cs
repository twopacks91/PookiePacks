using System.Collections.Generic;
using UnityEngine;
public class Playable_Weekday : PlayableCharacter
{
    protected override void Start()
    {
        x = -1.0f;
        z = z-1.0f;
        Scale = 0.5f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Weekday Duo");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}