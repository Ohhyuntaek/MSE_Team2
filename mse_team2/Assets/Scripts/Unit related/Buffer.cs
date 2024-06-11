using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Example1
{
    public class Buffer : MyUnit
    {
        protected override int Defend(Unit other, int damage)
        {
            var realDamage = damage;
            if (other is Tanker)
                realDamage *= 2;

            return realDamage - DefenceFactor;
        }

        
    }
}
