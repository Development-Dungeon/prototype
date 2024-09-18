using System;
using Utilities;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack
{

	public static void AttemptAttack(CountdownTimer attackCooldownTimer, Transform transform, Camera playerCamera)
    {
        if (attackCooldownTimer.IsRunning) return;

        // if I have selected in my inventory, an item which can attack, attempt to use it in an attack
        var selectedItem = InventoryManagerNew.Instance.GetSelectedItem();

        if (selectedItem != null && selectedItem.actionType.Equals(ActionType.Attack))
        {

            // attempt an attack
            var weaponAttackRange = selectedItem.range;
            var weaponAttackDamage = selectedItem.damage;
            var weaponAttackWidth = selectedItem.damage;
            var weaponCooldown = selectedItem.cooldownInSeconds;
            var weaponAttackInnerCone = selectedItem.innerConeAngle;
            var weaponAttackMultiple = selectedItem.canAttackMultiple;

            // get all the enemies in a radius?
            List<GameObject> allGameObjectsWithinRadius = WorldUtils.DetectAllClosest(transform.position, weaponAttackRange, 3);

            if (allGameObjectsWithinRadius.Count == 0) return;


            allGameObjectsWithinRadius = allGameObjectsWithinRadius
                                    .FindAll((go) => go.GetComponent<Health>() != null)
                                    .FindAll((go) => !go.CompareTag("Player"))
                                    .FindAll((go) =>
                                    {
                                        // check if the go is within the viewing angle?
                                        var directionToEnemy = go.transform.position - transform.position;
                                        var angleToEnemy = Vector3.Angle(directionToEnemy, playerCamera.transform.forward);

                                        var targetWithinViewingAngle = angleToEnemy <= (weaponAttackInnerCone / 2);

                                        return targetWithinViewingAngle;


                                    });


            if (weaponAttackMultiple)
            {
                // at this point, all the objects which are left are within range and are within the angle and should all be attacked
                allGameObjectsWithinRadius.ForEach((i) => i.GetComponent<Health>().TakeDamage(weaponAttackDamage));
            }
            else
            {
                // find the closest one of all that remain
                GameObject closest = WorldUtils.DetectClosest(allGameObjectsWithinRadius, transform.position);
                closest.GetComponent<Health>().TakeDamage(weaponAttackDamage);
            }

            // if the attack cooldown is not running
            attackCooldownTimer.Reset(weaponCooldown);
            attackCooldownTimer.Start();

        }

        // otherwise do nothing
        // TODO set the icon in inventory to inactive color when the cooldown is there
    }
}
