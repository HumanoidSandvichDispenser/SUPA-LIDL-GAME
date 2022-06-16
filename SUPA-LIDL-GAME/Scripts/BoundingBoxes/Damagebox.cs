using Godot;
using System.Collections.Generic;

namespace SupaLidlGame.BoundingBoxes
{

    public class Damagebox : Area2D
    {
        private HashSet<Hitbox> _ignoreList = new HashSet<Hitbox>();

        [Export]
        public float Damage { get; set; } = 0;

        [Export]
        public float Knockback { get; set; } = 0;

        /// <summary>
        /// The KinematicBody2D node inflicting damage through this damagebox
        /// </summary>
        [Export]
        public KinematicBody2D InflictorBody { get; set; }

        /// <summary>
        /// Relative path to the owner's (attacker's) node. If InflictorBody is
        /// null, then it will look for a node based on the specified path.
        /// </summary>
        [Export]
        public string InflictorBodyPath { get; set; }

        public override void _Ready()
        {
            if (InflictorBody is null)
            {
                InflictorBody = GetNode<KinematicBody2D>(InflictorBodyPath);
            }
        }

        public void _on_Damagebox_area_entered(Area2D area)
        {
            if (area is Hitbox hitbox)
            {
                if (!_ignoreList.Contains(hitbox))
                {
                    _ignoreList.Add(hitbox);
                    Utils.DamageInfo damageInfo = new Utils.DamageInfo
                    {
                        Damage = Damage,
                        KnockbackForce = Knockback,
                        Owner = InflictorBody,
                    };
                    hitbox.InflictDamage(Damage, InflictorBody, Knockback);
                }
            }
        }

        /// Resets the ignore list. Call this method whenever the attack cycle/animation finishes.
        public void ResetIgnoreList() => _ignoreList.Clear();
    }
}
