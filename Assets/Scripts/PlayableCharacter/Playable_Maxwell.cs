using System.Collections.Generic;
using UnityEngine;
public class Playable_Maxwell : PlayableCharacter
{
    protected override void Start()
    {
        x = -1f;
        Scale = 1.5f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Maxwell");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}