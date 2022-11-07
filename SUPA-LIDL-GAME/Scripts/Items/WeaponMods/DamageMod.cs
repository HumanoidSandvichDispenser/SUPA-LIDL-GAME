namespace SupaLidlGame.Items.WeaponMods
{
    public class DamageMod : WeaponMod
    {
        public override string Description => $"+{Value}% damage";

        public DamageMod()
        {
            SlotSpace = 1;
        }

        public override Utils.WeaponStats ModifyStats(Utils.WeaponStats stats)
        {
            stats.Damage *= Value;
            return stats;
        }
    }
}
