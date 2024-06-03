﻿using TbsFramework.Units;

namespace TbsFramework.Example1
{
    public class Attacker : MyUnit
    {
        protected override int Defend(Unit other, int damage)
        {
            var realDamage = damage;
            if (other is Buffer)
                realDamage *= 2;//Spearman deals double damage to paladin.

            return realDamage - DefenceFactor;
        }
    }
}
