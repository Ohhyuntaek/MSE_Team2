using System.Collections;
using UnityEngine;

namespace TbsFramework.Units.Highlighters
{
    public class AttackAnimation : UnitHighlighter
    {
        public float Magnitude = 1f;

        public override void Apply(Unit unit, Unit otherUnit)
        {
            StartCoroutine(AttackAnimationCoroutine(unit, otherUnit));
        }

        private IEnumerator AttackAnimationCoroutine(Unit unit, Unit otherUnit)
        {
            Debug.Log("Attack Animation");

            var StartingPosition = unit.transform.position;

            var heading = otherUnit.transform.localPosition - unit.transform.localPosition;
            var direction = heading / heading.magnitude * Magnitude;
            float startTime = Time.time;

            unit.transform.rotation = Quaternion.Euler(0, 90 - (Mathf.Atan2(heading.z, heading.x) * Mathf.Rad2Deg), 0); ;

            while (startTime + 0.25f > Time.time)
            {
                unit.transform.localPosition = Vector3.Lerp(unit.transform.localPosition, unit.transform.localPosition + (direction / 2.5f), ((startTime + 0.25f) - Time.time));
                yield return null;
            }
            startTime = Time.time;
            while (startTime + 0.25f > Time.time)
            {
                unit.transform.localPosition = Vector3.Lerp(unit.transform.localPosition, unit.transform.localPosition - (direction / 2.5f), ((startTime + 0.25f) - Time.time));
                yield return null;
            }

            unit.transform.localPosition = StartingPosition;
        }
    }
}
