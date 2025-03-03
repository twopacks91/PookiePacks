using System.Collections.Generic;
using UnityEngine;
public class Playable_Frederick : PlayableCharacter
{
    protected override void Start()
    {
        x = -1.5f;
        y = 1f;
        Scale = 3f;
        RotationY = 300f;

        base.Start();  // Call the base Start method

        List<int> Stats = PlayerData.GetCharacterStats("Frederick");

        SetStats(Stats);



        //PlayAnimation("idlecombat00");
    }
}