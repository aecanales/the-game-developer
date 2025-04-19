using System;
using UnityEngine;

public class ShapeStatusUpdate : MonoBehaviour
{
    public Animator anim;
    public AbilitySelector Abs;

    private void Update()
    {
        SwitchShape();
    }

    void SwitchShape()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Abs.currentShape == AbilitySelector.Shapes.Swipe)
            {
                anim.Play("Beam");
                Abs.currentShape = AbilitySelector.Shapes.Beam;
            }           
            else if (Abs.currentShape == AbilitySelector.Shapes.Beam)
            {
                anim.Play("Shield");
                Abs.currentShape = AbilitySelector.Shapes.Shield;
            }       
            else if (Abs.currentShape == AbilitySelector.Shapes.Shield)
            {
                anim.Play("Swipe");
                Abs.currentShape = AbilitySelector.Shapes.Swipe;
            }
        }
    }
}
