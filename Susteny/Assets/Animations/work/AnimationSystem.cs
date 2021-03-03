using UnityEngine;
using UnityEngine.Animations;

public class AnimationSystem : MonoBehaviour
{
    static int animationState;
    static RuntimeAnimatorController animatorController;

    void Start()
    {
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
        /*prepare(xbot); // ---- do usunięcia
        prepare(ybot); // ---- do usunięcia
        prepare(henry); // ---- do usunięcia
        AnimationSystem.startLookingAt(ybot, block); // ---- do usunięcia*/
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
        animate(character, Animations.NULL, AnimationBodyPart.WHOLE_BODY);
    }
    
    public static void startLookingAt(GameObject character, GameObject observed)
    {
        GameObject spine = character.transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").gameObject;
        GameObject head =  character.transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head").gameObject;

        if (head.GetComponent<LookAtConstraint>() == null) head.AddComponent<LookAtConstraint>().weight = 0.7f;
        if (spine.GetComponent<LookAtConstraint>() == null) spine.AddComponent<LookAtConstraint>().weight = 0.2f;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = observed.transform;
        source.weight = 1;

        head.GetComponent<LookAtConstraint>().AddSource(source);
        head.GetComponent<LookAtConstraint>().constraintActive = true;
        spine.GetComponent<LookAtConstraint>().AddSource(source);
        spine.GetComponent<LookAtConstraint>().constraintActive = true;
    }

    public static void stopLookingAt(GameObject character, GameObject observed)
    {
        GameObject spine = character.transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").gameObject;
        GameObject head = character.transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head").gameObject;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = observed.transform;
        LookAtConstraint headComponent = head.GetComponent<LookAtConstraint>();
        LookAtConstraint spineComponent = spine.GetComponent<LookAtConstraint>();

        for (int i=0; i< headComponent.sourceCount; i++)
        {
            if (headComponent.GetSource(i).Equals(source)) headComponent.RemoveSource(i);
        }

        for (int i = 0; i < spineComponent.sourceCount; i++)
        {
            if (spineComponent.GetSource(i).Equals(source)) spineComponent.RemoveSource(i);
        }
    }

    /*public GameObject xbot; // ---- do usunięcia
    public GameObject ybot; // ---- do usunięcia
    public GameObject henry; // ---- do usunięcia
    public GameObject block; // ---- do usunięcia*/

    /*void Update()
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
    }*/
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