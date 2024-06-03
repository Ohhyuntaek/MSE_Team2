using TbsFramework.Units;

namespace TbsFramework.Example1
{
    public class Buffer : MyUnit
    {
        protected override int Defend(Unit other, int damage)
        {
            var realDamage = damage;
            if (other is Tanker)
                realDamage *= 2;//Archer deals double damage to spearman.

            return realDamage - DefenceFactor;
        }
    }
}
