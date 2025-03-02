using System.Collections.Generic;
using UnityEngine;
public class Playable_Pengting : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.5f;
        Scale = 0.1f;
        Rotation = 180;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Pengting");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}