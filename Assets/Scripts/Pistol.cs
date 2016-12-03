using UnityEngine;
using System.Collections;
using System;

public class Pistol : BulletObject
{
    public int weaponDamage = 10;
    public int range = 2;

    private Vector2 direction;
    private float rot;

	// Use this for initialization
	protected override void Start ()
    {
        rot = GetComponent<Transform>().rotation.eulerAngles.z;
        direction = rotToDir(rot);
        base.Start();
        base.BulletFired<Enemy>((int)direction.x, (int)direction.y, range);
    }

    protected override void somethingHit<T>(T component)
    { 
        Enemy hitEnemy = component as Enemy;
        hitEnemy.DamageEnemy(weaponDamage);
    }
    private Vector2 rotToDir(float z)
    {
        Vector2 dir;
        if (z % 360 == 0f)
            dir = new Vector2(-1, 0);
        else if (z % 360 == 90f)
            dir = new Vector2(0, -1);
        else if (z % 360 == 180f)
            dir = new Vector2(1, 0);
        else
            dir = new Vector2(0, 1);
        return dir;
    }
}
