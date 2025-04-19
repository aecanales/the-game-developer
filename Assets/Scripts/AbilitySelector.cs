using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelector : MonoBehaviour
{
    public Animator shapeAnim;
    public List<DOTweenAnimation> GrowTweens = new();
   // public List<DOTweenAnimation> ShrinkTweens = new();
    public List<GameObject> Skills = new();
    public Animator shapeSelectorAnimator;
    public enum Shapes
    {
        Beam,Swipe,Shield
    }
    public enum Abilities
    {
        Push,Pull,Charm,Slash,Freeze
    }

    public Shapes currentShape;
    public Abilities currentAbility;
    private void Update()
    {
        CheckForSwitcherInput();
    }

    private void CheckForSwitcherInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PickAbility(0);
            currentAbility = Abilities.Push;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PickAbility(1);
            currentAbility = Abilities.Pull;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PickAbility(2);
            currentAbility = Abilities.Charm;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PickAbility(3);
            currentAbility = Abilities.Slash;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PickAbility(4);
            currentAbility = Abilities.Freeze;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CycleShapes();
        }
    }

    private void CycleShapes()
    {
        
    }

    private void PickAbility(int index)
    {
        for (int i = 0; i < Skills.Count; i++)
        {
            if (Skills[i].GetComponent<IsAbilitySelected>().isSelected == true)
            {
                Skills[i].GetComponent<IsAbilitySelected>().isSelected = false;
                GrowTweens[index].CreateTween(true,false);
                GrowTweens[i].DOPlayBackwardsById("1");
                GrowTweens[i].transform.SetSiblingIndex(i);
            }
        }    
        Debug.Log("Index " + index);
        if(Skills[index].GetComponent<IsAbilitySelected>().isSelected == false)
        {
            Skills[index].GetComponent<IsAbilitySelected>().isSelected = true;
            GrowTweens[index].CreateTween(true,false);
            GrowTweens[index].DOPlayById("1");
            GrowTweens[index].transform.SetSiblingIndex(4);
        }

    }
}
