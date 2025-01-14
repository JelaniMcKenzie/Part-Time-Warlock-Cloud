using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float Health { set; get; }
    public bool Targetable { set; get; }
    public bool Invincible { set; get; }
    public SimpleFlash FlashEffect { set; get; }
    public void OnHit(float damage, Vector2 knockback);
    public void ApplyKnockback(Vector2 knockback);
    public void OnHit(float damage);
    public void OnObjectDestroyed();
}
