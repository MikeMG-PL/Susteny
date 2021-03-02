using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationSystem : MonoBehaviour
{
    static int animationState;
    static RuntimeAnimatorController animatorController;

    void Start()
    {
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
        prepare(xbot); // ---- do usunięcia
        prepare(ybot); // ---- do usunięcia
        prepare(henry); // ---- do usunięcia
    }

    public static Animator prepare(GameObject character)
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

        return characterAnimator;
    }

    public static void animate(GameObject character, Animations animation, AnimationBodyPart animationBodyPart)
    {
        Animator characterAnimator = prepare(character);

        switch(animationBodyPart)
        {
            case AnimationBodyPart.LOWER_BODY:
                characterAnimator.SetInteger("LowerAnimationIndex", (int)animation);
                characterAnimator.SetTrigger("LowerEndTrigger");
                break;

            case AnimationBodyPart.UPPER_BODY:
                characterAnimator.SetInteger("UpperAnimationIndex", (int)animation);
                characterAnimator.SetTrigger("UpperEndTrigger");
                break;

            case AnimationBodyPart.WHOLE_BODY:
                characterAnimator.SetInteger("UpperAnimationIndex", (int)animation);
                characterAnimator.SetTrigger("UpperEndTrigger");
                characterAnimator.SetInteger("LowerAnimationIndex", (int)animation);
                characterAnimator.SetTrigger("LowerEndTrigger");
                break;
        }
    }

    public static void resetAnimation(GameObject character)
    {
        Animator characterAnimator = prepare(character);
        characterAnimator.SetTrigger("UpperEndTrigger");
        characterAnimator.SetTrigger("LowerEndTrigger");
    }

    public static void stopAnimation(GameObject character)
    {
        Animator characterAnimator = prepare(character);
        characterAnimator.SetInteger("UpperAnimationIndex", (int) Animations.NULL);
        characterAnimator.SetTrigger("UpperEndTrigger");
        characterAnimator.SetInteger("LowerAnimationIndex", (int)Animations.NULL);
        characterAnimator.SetTrigger("LowerEndTrigger");
    }

    public GameObject xbot; // ---- do usunięcia
    public GameObject ybot; // ---- do usunięcia
    public GameObject henry; // ---- do usunięcia

    void Update()
    {
        // -------------------------------------------------------------------- TESTING AND DEBUGING -------------------------------------------------------------------- //
        if (Input.GetKeyDown("q"))
        {
            AnimationSystem.animate(ybot, Animations.WALKING_1, AnimationBodyPart.LOWER_BODY);
            AnimationSystem.animate(ybot, Animations.SMOKING, AnimationBodyPart.UPPER_BODY);
        }

        if (Input.GetKeyDown("w"))
        {
            AnimationSystem.animate(xbot, Animations.SMOKING, AnimationBodyPart.UPPER_BODY);
            AnimationSystem.animate(xbot, Animations.WALKING_1, AnimationBodyPart.LOWER_BODY);
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------- //
    }
}
    public enum Animations
{
    NULL = 0,
    DYING = 1,
    IDLE_1 = 2,
    IDLE_2 = 3,
    RUNNING_1 = 4,
    RUNNING_2 = 5,
    SITTING_1 = 6,
    SITTING_2 = 7,
    SMOKING = 8,
    WHEELCHAIR = 9,
    TALKING_1 = 10,
    TALKING_2 = 11,
    TALKING_3 = 12,
    TALKING_4 = 13,
    TALKING_ON_PHONE_1 = 14,
    TALKING_ON_PHONE_2 = 15,
    WALKING_1 = 16,
    WALKING_2 = 17,
    WALKING_WITH_BRIEFCASE = 18
}

public enum AnimationBodyPart
{
    UPPER_BODY = 1,
    LOWER_BODY = 2,
    WHOLE_BODY = 3
}