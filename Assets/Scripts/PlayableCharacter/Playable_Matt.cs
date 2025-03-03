using System.Collections.Generic;
using UnityEngine;
public class Playable_Matt : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.5f;
        Scale = 50f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Matt");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}