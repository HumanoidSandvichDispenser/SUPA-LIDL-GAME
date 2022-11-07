using Godot;

namespace SupaLidlGame.Items.Weapons
{
    public class BaseSword : Weapon
    {
        protected BoundingBoxes.Damagebox _damageBox;

        protected AnimationPlayer _animationPlayer;

        protected AnimationTree _animationTree;

        protected AnimationNodeStateMachinePlayback _animationPlayback;

        public override void _Ready()
        {
            //WeaponStats = GetNode<Utils.WeaponStats>("WeaponStats");
            //WeaponStats = GD.Load<Utils.WeaponStats>(
            //        "res://Scripts/Items/Weapons/BaseSword.tres");

            _damageBox = GetNode<BoundingBoxes.Damagebox>("Sprite/Damagebox");
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            _animationTree = GetNode<AnimationTree>("AnimationTree");
            _animationPlayback = _animationTree.Get("parameters/playback")
                    as AnimationNodeStateMachinePlayback;

            base._Ready();

            // testing only
            //Mods.Add(new WeaponMods.DamageMod { Value = 1.5f });
            //Mods.Add(new WeaponMods.FireballMod());
        }

        public override void _Process(float delta)
        {
            if (Inflictor is HumanoidKinematicBody2D humanoid)
            {
                _animationTree.Set("parameters/Use/blend_position",
                        humanoid.TargetBlend);
            }

            base._Process(delta);
        }

        public override void Equip()
        {
            base.Equip();

            if (!(Inflictor is null))
            {
                _damageBox.InflictorBody = Inflictor;
            }
        }

        public override void Unequip()
        {
            base.Unequip();
        }

        protected void SetStats()
        {
            var coll = _damageBox.GetNode<CollisionShape2D>("CollisionShape2D");
            var circle = coll.Shape as CircleShape2D;

            Utils.WeaponStats modifiedStats = WeaponStats.Duplicate()
            as Utils.WeaponStats;

            foreach (WeaponMods.WeaponMod mod in Mods)
            {
                mod.ModifyStats(modifiedStats);
            }

            _damageBox.Knockback = modifiedStats.Knockback;
            _damageBox.Damage = modifiedStats.Damage;
            circle.Radius = modifiedStats.Range;

            _damageTimeLeft = WeaponStats.DamageTime;
        }

        protected override void StartAttack(Vector2 direction)
        {
            _damageBox.ResetIgnoreList();
            _damageBox.Monitoring = true;

            SetStats();

            if (Inflictor is HumanoidKinematicBody2D humanoid)
            {
                if (Inflictor is PlayerKinematicBody2D player)
                {
                    // TODO: put this stuff in an event/signal
                    player.IsAttacking = true;
                    Vector2 velocity = player.Velocity;
                    velocity.x = 0;
                    player.Velocity = velocity;
                    player.Push(-75);
                }

                float blend = humanoid.TargetBlend;
                if (blend < 0)
                {
                    _damageBox.Position = new Vector2(-4, -8);
                }
                else if (blend > 0)
                {
                    _damageBox.Position = new Vector2(-4, 8);
                }
                else
                {
                    _damageBox.Position = new Vector2(4, 0);
                }
            }

            foreach (WeaponMods.WeaponMod mod in Mods)
            {
                mod.OnWeaponFire(this,
                                 Inflictor,
                                 direction);
            }

            _animationPlayback.Travel("Use");
        }

        protected override void StopAttack()
        {
            if (Inflictor is PlayerKinematicBody2D player)
            {
                player.IsAttacking = false;
            }
        }

        protected override void StopDamage()
        {
            _damageBox.Monitoring = false;
        }
    }
}
