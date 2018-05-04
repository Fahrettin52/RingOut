using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_PickUp_Shield : Code_PickUp {

    // Checks if the player already possess the same pickup
    public override bool CheckPlayerForPickUp(Code_Player player) {
        return player.GetDeathShield();
    }

    // Gives the player a DeathShield
    public override void ActivatePickUpEffect(Code_Player player) {
        player.SetDeathShield();
    }  
}
