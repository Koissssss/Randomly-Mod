using Randomly.content.projectiles.weapons.magicPros;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Randomly.content.items.weapons.magics
{
	public class Magic_IcyBurst : ModItem
	{
        public override void SetDefaults() {
			// Start by using CloneDefaults to clone all the basic item properties from the vanilla Last Prism.
			// For example, this copies sprite size, use style, sell price, and the item being a magic weapon.
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.reuseDelay = 0;
			Item.shootSpeed = 30f;
			Item.width = 60;
			Item.height = 60;
			Item.UseSound = null;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Magic;
			Item.channel = true;
			Item.mana = 50;
			Item.damage = 12;
			Item.shoot = ModContent.ProjectileType<MagicPros_IcyBurst>();
			Item.knockBack = 0f;
		}

        // Because this weapon fires a holdout projectile, it needs to block usage if its projectile already exists.
        public override bool CanUseItem(Player player) {
			return player.ownedProjectileCounts[ModContent.ProjectileType<MagicPros_IcyBurst>()] <= 0;
		}
	}
}