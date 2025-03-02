using System.Collections.Generic;
using UnityEngine;
public class Playable_Crabby : PlayableCharacter
{
    protected override void Start()
    {
        x = -1;
        Scale = 0.5f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Crabby");

        SetStats(Stats);


        //PlayAnimation("idlecombat00");
    }
}