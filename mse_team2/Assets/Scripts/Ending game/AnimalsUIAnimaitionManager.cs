using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsUIAnimaitionManager : MonoBehaviour
{
    public bool isWin;
    public List<GameObject> animals;

    void OnEnable()
    {
        StartCoroutine(PlayAnimationsWithInterval());

        
    }


    private IEnumerator PlayAnimationsWithInterval()
    {
        yield return new WaitForSeconds(0.5f);

        if (isWin)
        {
            foreach (GameObject animal in animals)
            {
                Animator animator = animal.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("Jump", 0);

                    animator.Play("Eyes_Happy", 1);

                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Debug.LogWarning("Prefab " + animal.name + " does not have an Animator component.");
                }
            }
        }
        else
        {
            foreach (GameObject animal in animals)
            {
                Animator animator = animal.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("Death", 0);

                    animator.Play("Eyes_Spin", 1);

                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    Debug.LogWarning("Prefab " + animal.name + " does not have an Animator component.");
                }
            }
        }
    }
}
