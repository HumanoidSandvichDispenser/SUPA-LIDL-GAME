using SupaLidlGame.Extensions;

namespace SupaLidlGame.Items.Weapons
{
    public class BaseSword : Weapon
    {
        protected BoundingBoxes.Damagebox _damageBox;
        protected string test;

        public override void _Ready()
        {
            SetInflictor();
        }

        public void SetInflictor()
        {
            if (_damageBox is null)
                _damageBox = GetNode<BoundingBoxes.Damagebox>("Sprite/Damagebox");

            var inflictor = _damageBox.GetFirstParentOfClass<HumanoidKinematicBody2D>();
            if (!(inflictor is null))
            {
                _damageBox.InflictorBody = inflictor;
            }
        }

        public override void Equip()
        {
            SetInflictor();
        }

        public override void Unequip()
        {
            _damageBox.InflictorBody = null;
        }
    }
}
