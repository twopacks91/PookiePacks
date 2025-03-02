using System.Collections.Generic;
using UnityEngine;
public class Playable_Alexa : PlayableCharacter
{
    protected override void Start()
    {
        x = -0.7f;
        Scale = 0.7f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Alexa");

        SetStats(Stats);


        //PlayAnimation("idlecombat00");
    }
}