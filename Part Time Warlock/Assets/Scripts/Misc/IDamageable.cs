using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public float Health { set; get; }
    public bool Targetable { set; get; }
    public bool Invincible { set; get; }
    public SimpleFlash FlashEffect { set; get; }
    public void OnHit(float damage, KnockbackData knockback, DamageType damageType);
    public void ApplyKnockback(KnockbackData knockbackData);
    public void OnHit(float damage);
    public void OnObjectDestroyed();

    public enum DamageType { Projectile, Melee, AOE }
    public struct KnockbackData
    {
        public Vector2 Direction; // The direction of the knockback
        public float Force;       // The intensity of the knockback
        public float Duration;    // How long the knockback lasts

        public KnockbackData(Vector2 direction, float force, float duration)
        {
            Direction = direction.normalized;
            Force = force;
            Duration = duration;
        }
    }

}
