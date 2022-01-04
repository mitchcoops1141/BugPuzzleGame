using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bug
{
    [SerializeField] private int health;
    public void SetHealth(int health) { this.health = health; }
    public int GetHealth() { return this.health; }

    [SerializeField] private bool canBeSteppedOn;
    public void SetCanBeSteppedOn(bool canBeSteppedOn) { this.canBeSteppedOn = canBeSteppedOn; }
    public bool GetCanBeSteppedOn() { return this.canBeSteppedOn; }


    [SerializeField] private bool canBeShot;
    public void SetCanBeShot(bool canBeShot) { this.canBeShot = canBeShot; }
    public bool GetCanBeShot() { return this.canBeShot; }

    [SerializeField] private int moveDistance;
    public void SetMoveDistance(int moveDistance) { this.moveDistance = moveDistance; }
    public int GetMoveDistance() { return this.moveDistance; }
}
