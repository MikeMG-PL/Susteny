using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{
    static int animationState;
    static RuntimeAnimatorController animatorController;
    void Start()
    {
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
        test(); // -- testowanie --
    }

    void Update()
    {
    }

    public static void animate(GameObject character, Animations animation)
    {
        Animator characterAnimator;

        if (character.GetComponent<Animator>() == null)
        {
            character.AddComponent<Animator>();
            characterAnimator = character.GetComponent<Animator>();
            characterAnimator.runtimeAnimatorController = animatorController;
        }
        else
        {
            characterAnimator = character.GetComponent<Animator>();
        }

        characterAnimator.SetInteger("animationIndex", (int) animation);
    }

    // -------------------------------------------------------------------- TESTING AND DEBUGING -------------------------------------------------------------------- //

    public GameObject xbot;
    public GameObject ybot;
    public GameObject henry;

    public void test()
    {
        AnimationSystem.animate(xbot, Animations.RUN);
        AnimationSystem.animate(ybot, Animations.DIE);
        AnimationSystem.animate(henry, Animations.ARGUE1);
    }
}

public enum Animations
{
    IDLE = 0,
    RUN = 1,
    DANCE = 2,
    DIE = 3, 
    WALK_MEN = 4, 
    WALK_WOMEN = 5,
    WHEELCHAIR1 = 6, 
    WHEELCHAIR2 = 7, 
    WALK_WITH_BRIEFCASE = 8,
    SMOKE = 9,
    ARGUE1 = 10,
    ARGUE2 = 11,
    SIT_AND_TALK_MEN = 12,
    SIT_AND_TALK_WOMEN = 13
}