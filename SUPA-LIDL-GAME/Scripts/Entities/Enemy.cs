using Godot;
using System;
using SupaLidlGame.Exceptions;
using SupaLidlGame.Entities.AI;

namespace SupaLidlGame.Entities
{
    public abstract class Enemy : HumanoidKinematicBody2D
    {
        //protected Utils.Stats _stats;
        public Utils.Stats Stats;

        protected AnimationPlayer _animationPlayer;

        protected OneShotParticles _deathParticles;

        protected Thinker _thinker;

        protected Sprite _sprite;

        public override void _Ready()
        {
            // initialize with the correct components
            Stats = GetNode<Utils.Stats>("Stats");

            if (_thinker is null)
            {
                throw new BehaviorNotImplementedException("No thinker found.");
            }

            _sprite = GetNode<Sprite>("Sprite");

            if (_sprite is null)
            {
                throw new System.Exception(
                        "Enemy initialized without a Sprite node.");
            }

            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

            _deathParticles = GetNode<OneShotParticles>("DeathParticles");
            if (_deathParticles is null)
            {
                throw new System.Exception("DeathParticles is not a Particles2D");
            }

            base._Ready();
        }

        public override void _Process(float delta)
        {
            if (!IsDead)
            {
                _thinker.Think(delta, this);
            }

            if (Direction.x != 0 && !IsMovementFrozen)
            {
                Vector2 scale = _sprite.Scale;
                if (Direction.x < 0)
                {
                    scale.x = -Math.Abs(scale.x);
                }
                else if (Direction.x > 0)
                {
                    scale.x = Math.Abs(scale.x);
                }
                _sprite.Scale = scale;
            }
        }

        public void _on_EnemyHitbox_ReceivedDamage(
                float damage,
                KinematicBody2D attacker,
                float knockback,
                Vector2 knockbackOrigin = default,
                Vector2 knockbackVector = default)
        {
            if (!(Stats is null))
            {
                ShowDamageText(damage);
                Stats.Health -= damage;

                // to apply knockback, there must be an attacker,
                // the knockback force must be non-zero
                // and this object must not be massless
                if (!(attacker is null) && knockback != 0 && Mass > 0)
                {
                    // if knockbackOrigin is default, then use knockbackVector.
                    // if both are default, then find direction from attacker's
                    // position to our position

                    Vector2 direction;
                    if (knockbackOrigin == default)
                    {
                        if (knockbackVector == default)
                        {
                            // direction to player
                            direction = (GlobalPosition -
                                    attacker.GlobalPosition).Normalized();
                        }
                        else
                        {
                            direction = knockbackVector;
                        }
                    }
                    else
                    {
                        direction = knockbackOrigin - GlobalPosition;
                    }
                    ApplyImpulse(direction * knockback, true);
                }
                if (Stats.Health <= 0)
                {
                    Die();
                }
            }
        }

        public override void Die()
        {
            base.Die();
            QueueFree();
        }
    }
}
