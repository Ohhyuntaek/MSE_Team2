using TbsFramework.Units;

namespace TbsFramework.Example1
{
    public class Attacker : MyUnit
    {
        protected override int Defend(Unit other, int damage)
        {
            var realDamage = damage;
            if (other is Buffer)
                realDamage *= 2;

            return realDamage - DefenceFactor;
        }
    }
}
