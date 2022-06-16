using Godot;

namespace SupaLidlGame.BoundingBoxes
{
    public class Hitbox : Area2D
    {
        protected Utils.Stats _stats = null;

        [Signal]
        public delegate void ReceivedDamage(float damage);

        // godot doesn't like to pass my epic DamageInfo struct parameter when
        // emitting signals so now I have to do line break to prevent supa long
        // line
        /// <summary>
        /// Inflicts damage.
        /// <param name="knockbackVector">
        /// If <paramref name="knockbackOrigin"/> is <see langword="default"/>,
        /// then the knockback vector will be calculated by the <paramref
        /// name="knockbackVector"/>. If both are default, the knockback vector
        /// will be based on the position of the attacker.
        /// </param>
        /// </summary>
        public void InflictDamage(
                float damage,
                KinematicBody2D attacker,
                float knockback,
                Vector2 knockbackOrigin = default,
                Vector2 knockbackVector = default)
        {
            // The entity receiving damage should deal with their own method of
            // receiving damage.
            EmitSignal(
                    "ReceivedDamage",
                    damage,
                    attacker,
                    knockback,
                    knockbackOrigin,
                    knockbackVector);
        }
    }
}
