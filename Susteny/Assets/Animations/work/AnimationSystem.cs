using UnityEngine;
using UnityEngine.Animations;

public class AnimationSystem : MonoBehaviour
{
    static int animationState;
    static RuntimeAnimatorController animatorController;

    void Start()
    {
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
    }

    // prepares character to be animated (gives Animator and sets everything to work)
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

    // gives character animation and asign it to the specific body part (it does not have to be "prepared")
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

    // refresh all animations
    public static void resetAnimation(GameObject character)
    {
        Animator characterAnimator = prepare(character);
        characterAnimator.SetTrigger("UpperEndTrigger");
        characterAnimator.SetTrigger("LowerEndTrigger");
    }

    // stop all animations and make T-pose
    public static void stopAnimation(GameObject character)
    {
        animate(character, Animations.NULL, AnimationBodyPart.WHOLE_BODY);
    }
    
    // make character look at some game object
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

    // stop looking at some game object
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

    // stop looking at every game object
    public static void stopLookingAtAll(GameObject character)
    {
        GameObject spine = character.transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").gameObject;
        GameObject head = character.transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head").gameObject;

        LookAtConstraint headComponent = head.GetComponent<LookAtConstraint>();
        LookAtConstraint spineComponent = spine.GetComponent<LookAtConstraint>();

        for (int i = 0; i < headComponent.sourceCount; i++)
        {
            headComponent.RemoveSource(i);
        }

        for (int i = 0; i < spineComponent.sourceCount; i++)
        {
            spineComponent.RemoveSource(i);
        }
    }

    // returns speed of all "move" animations character is performing
    public static float getMoveAnimationSpeed(GameObject character)
    {
        Animator characterAnimator = prepare(character);
        return characterAnimator.GetFloat("animationSpeed");
    }

    // sets speed of all "move" animations character is performing
    public static void setMoveAnimationSpeed(GameObject character, float newSpeed)
    {
        Animator characterAnimator = prepare(character);
        characterAnimator.SetFloat("animationSpeed",newSpeed);
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