using Godot;
using SupaLidlGame.Exceptions;
using SupaLidlGame.Entities.AI;

namespace SupaLidlGame.Entities
{
    public abstract class Enemy : HumanoidKinematicBody2D
    {
        [Export]
        public AIType Behavior { get; set; } = AIType.Basic;

        protected Utils.Stats _stats;

        protected AnimationPlayer _animationPlayer;

        protected Node _deathParticles;

        public override void _Ready()
        {
            // initialize with the correct components
            _stats = GetNode<Utils.Stats>("Stats");

            if (_stats == null)
            {
                throw new System.Exception(
                        "Enemy initialized without a Stats node.");
            }

            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

            _deathParticles = GetNode("DeathParticles");
            if (!(_deathParticles is Particles2D || _deathParticles is
                        CPUParticles2D) || _deathParticles is null)
            {
                throw new System.Exception("DeathParticles is not a Particles2D " +
                        "or CPUParticles2D node.");
            }

            base._Ready();
        }

        public override void _Process(float delta)
        {
            if (!IsDead)
            {
                switch (Behavior)
                {
                    case AIType.Basic:
                        ThinkBasic(delta);
                        break;
                    case AIType.JumpKing:
                        break;
                }
            }
        }

        public void _on_EnemyHitbox_ReceivedDamage(
                float damage,
                KinematicBody2D attacker,
                float knockback,
                Vector2 knockbackOrigin = default,
                Vector2 knockbackVector = default)
        {
            if (_stats != null)
            {
                _stats.Health -= damage;

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
                if (_stats.Health <= 0)
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

        protected virtual void ThinkBasic(float delta)
        {
            // move towards the player
            PlayerKinematicBody2D player = GlobalState.Player;
            Vector2 moveTo = player.GlobalPosition - GlobalPosition;
            Direction = moveTo.Normalized();
        }
        
        protected virtual void ThinkJumpKing(float delta)
        {
            
        }

        protected virtual void ThinkBounce(float delta)
        {
            throw new System.NotImplementedException();
            KinematicCollision2D collision = GetLastSlideCollision();

            if (collision != null)
            {
                Vector2 bounce = _previousVelocity.Bounce(collision.Normal);
                ApplyImpulse(bounce.Normalized() * MaxSpeed * 2);
            }
        }

        protected virtual void Think(float delta) =>
                throw new BehaviorNotImplementedException(
                "Implement custom behavior with `override void Think()`. " +
                "Overriden method should not call the base method.");
    }
}
