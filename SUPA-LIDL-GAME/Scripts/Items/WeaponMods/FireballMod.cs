using Godot;

namespace SupaLidlGame.Items.WeaponMods
{
    public class FireballMod : WeaponMod
    {
        public override string Description => "Fires a fireball";

        public FireballMod()
        {
            SlotSpace = 1;
        }

        public override Utils.WeaponStats ModifyStats(Utils.WeaponStats stats)
        {
            return stats;
        }

        public override void OnWeaponFire(Weapons.Weapon weapon,
                KinematicBody2D attacker,
                Vector2 direction)
        {
            PackedScene fireballScene = GD.Load<PackedScene>("res://Scripts/Entities/Projectiles/Fireball.tscn");
            var instance = fireballScene.Instance<Entities.Projectiles.RayProjectile>();
            instance.GlobalPosition = weapon.GlobalPosition;
            instance.Inflictor = attacker;
            instance.Direction = direction;
            attacker.GetTree().Root.AddChild(instance);
        }
    }
}
