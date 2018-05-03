using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_PickUp_Shield : Code_PickUp {

    // Gives the player a DeathShield
    public override void ActivatePickUpEffect(Code_Player player) {
        player.SetDeathShield();
    }
}
