using System.Collections;
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

        protected override void InjectBuff()
        {
            Debug.Log("InjectBuff");

            if (Cell._Property == habitat_property)
            {
                for (int i = 0; i < FindObjectsOfType<Unit>().Length; i++)
                {
                    if (FindObjectsOfType<Unit>()[i].PlayerNumber == this.PlayerNumber)
                    {
                        HealingBuff healingBuff = new HealingBuff(-1, 5);
                        FindObjectsOfType<Unit>()[i].AddBuff(healingBuff);
                        FindObjectsOfType<Unit>()[i].OnBufferEffect();
                    }
                }
            }
        }


    }
}
