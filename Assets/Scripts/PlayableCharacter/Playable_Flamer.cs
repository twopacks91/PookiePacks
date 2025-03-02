using System.Collections.Generic;
using UnityEngine;
public class Playable_Flamer : PlayableCharacter
{
    protected override void Start()
    {
        x = -1;
        Scale = 0.6f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Flamer");

        SetStats(Stats);


        //PlayAnimation("idlecombat00");
    }
}