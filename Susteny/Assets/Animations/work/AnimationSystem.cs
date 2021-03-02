using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationSystem : MonoBehaviour
{
    static int animationState;
    static RuntimeAnimatorController animatorController;
    void Start()
    {
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
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
        if(animationBodyPart == AnimationBodyPart.WHOLE_BODY)
        {
            characterAnimator.SetInteger("bodyLevel", 1);
            characterAnimator.SetInteger("animationIndex", (int)animation);
            //characterAnimator.SetTrigger("endTrigger");
            characterAnimator.SetInteger("bodyLevel", 2);
            characterAnimator.SetInteger("animationIndex", (int)animation);
            characterAnimator.SetTrigger("endTrigger");
        }
        else characterAnimator.SetInteger("bodyLevel", (int)animationBodyPart);
        characterAnimator.SetInteger("animationIndex", (int) animation);
        characterAnimator.SetTrigger("endTrigger");
    }

    public static void resetAnimation(GameObject character)
    {
        Animator characterAnimator = prepare(character);
        characterAnimator.SetTrigger("endTrigger");
    }

    public static void stopAnimation(GameObject character)
    {
        animate(character, Animations.NULL, AnimationBodyPart.WHOLE_BODY);
    }

    // -------------------------------------------------------------------- TESTING AND DEBUGING -------------------------------------------------------------------- //

    public GameObject xbot;
    public GameObject ybot;
    public GameObject henry;

    void Update()
    {
        if (Input.GetKeyDown("q")) AnimationSystem.animate(ybot, Animations.IDLE_1, AnimationBodyPart.UPPER_BODY);
        if (Input.GetKeyDown("w")) AnimationSystem.animate(ybot, Animations.RUNNING_1, AnimationBodyPart.UPPER_BODY);
        if (Input.GetKeyDown("e")) AnimationSystem.animate(ybot, Animations.SITTING_1, AnimationBodyPart.UPPER_BODY);
        if (Input.GetKeyDown("r")) AnimationSystem.animate(ybot, Animations.DYING, AnimationBodyPart.UPPER_BODY);
        if (Input.GetKeyDown("t")) AnimationSystem.animate(ybot, Animations.SMOKING, AnimationBodyPart.UPPER_BODY);
        if (Input.GetKeyDown("space")) AnimationSystem.resetAnimation(ybot);
        if (Input.GetKeyDown("b")) AnimationSystem.stopAnimation(ybot);
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