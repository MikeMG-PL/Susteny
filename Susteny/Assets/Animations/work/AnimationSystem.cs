using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationSystem : MonoBehaviour
{
    static int animationState;
    static RuntimeAnimatorController animatorController;
    void Start()
    {
        animatorController = GetComponent<Animator>().runtimeAnimatorController;
        GameObject ob = new GameObject("ob");
        ob.transform.position = new Vector3(1, 1, -1.5f);
        makeLookAt(ybot, ybot); // ----- usuń ----- //
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

    public static RigBuilder prepareRig(GameObject character)
    {
        RigBuilder rigBuilder = character.GetComponent<RigBuilder>();

        if (rigBuilder == null)
        {
            character.AddComponent<RigBuilder>();
            GameObject headRig = new GameObject("headRig");
            headRig.transform.SetParent(character.transform);
            GameObject bodyRig = new GameObject("bodyRig");
            bodyRig.transform.SetParent(character.transform);

            headRig.AddComponent<Rig>();
            bodyRig.AddComponent<Rig>().weight = 0.5f;

            MultiAimConstraint headConstraint = headRig.AddComponent<MultiAimConstraint>();
            MultiAimConstraint bodyConstraint = bodyRig.AddComponent<MultiAimConstraint>();

            foreach (Transform data in character.GetComponentInChildren<Transform>())
            {
                if (data.name == "mixamorig:Hips")
                {
                    headConstraint.data.constrainedObject = data.Find("mixamorig:Spine").Find("mixamorig:Spine1").Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head");
                    bodyConstraint.data.constrainedObject = data.Find("mixamorig:Spine");
                    break;
                }
            }

            rigBuilder = character.GetComponent<RigBuilder>();

            rigBuilder.layers.Add(new RigBuilder.RigLayer(headRig.GetComponent<Rig>(),true));
            rigBuilder.layers.Add(new RigBuilder.RigLayer(bodyRig.GetComponent<Rig>(), true));
            rigBuilder.Build();
        }

        return rigBuilder;
    }

    public static void animate(GameObject character, Animations animation)
    {
        Animator characterAnimator = prepare(character);
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
        animate(character, Animations.NULL);
    }

    public static void makeLookAt(GameObject character, GameObject observed)
    {
        prepare(character);
        RigBuilder rigBuilder = prepareRig(character);
        for (int i = 0; i < rigBuilder.layers.Count; i++)
        {
            WeightedTransform source = new WeightedTransform(observed.transform, 1);
            //rigBuilder.layers[i].rig.GetComponentInParent<MultiAimConstraint>().data.constrainedObject = observed.transform;
            rigBuilder.layers[i].rig.GetComponentInParent<MultiAimConstraint>().data.sourceObjects.Add(source);
            rigBuilder.Build();
        }
    }

    // -------------------------------------------------------------------- TESTING AND DEBUGING -------------------------------------------------------------------- //

    public GameObject xbot;
    public GameObject ybot;
    public GameObject henry;

    void Update()
    {
        if (Input.GetKeyDown("q")) AnimationSystem.animate(ybot, Animations.IDLE);
        if (Input.GetKeyDown("w")) AnimationSystem.animate(ybot, Animations.RUN);
        if (Input.GetKeyDown("e")) AnimationSystem.animate(ybot, Animations.DANCE);
        if (Input.GetKeyDown("r")) AnimationSystem.animate(ybot, Animations.DIE);
        if (Input.GetKeyDown("t")) AnimationSystem.animate(ybot, Animations.WALK_MEN);
        if (Input.GetKeyDown("space")) AnimationSystem.resetAnimation(ybot);
        if (Input.GetKeyDown("b")) AnimationSystem.stopAnimation(ybot);
    }
}

public enum Animations
{
    NULL = -1,
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
    SIT_AND_TALK_WOMEN = 13,
}