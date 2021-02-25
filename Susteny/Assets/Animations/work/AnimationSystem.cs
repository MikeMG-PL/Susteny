using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Animations
{
    RUN, DANCE, DIE
}

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

        int animationIndex = 0;
        switch (animation)
        {
            case Animations.RUN:
                animationIndex = 3;
                break;

            case Animations.DIE:
                animationIndex = 4;
                break;

            case Animations.DANCE:
                animationIndex = 5;
                break;
        }

        characterAnimator.SetInteger("animationIndex", animationIndex);
    }

    // -------------------------------------------------------------------- TESTING AND DEBUGING -------------------------------------------------------------------- //

    public GameObject xbot;
    public GameObject ybot;
    public GameObject henry;

    public void test()
    {
        AnimationSystem.animate(xbot, Animations.RUN);
        AnimationSystem.animate(ybot, Animations.DIE);
        AnimationSystem.animate(henry, Animations.DANCE);
    }
}

