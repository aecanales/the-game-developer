using UnityEngine;

public class IsAbilitySelected : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isSelected;

    public void Played()
    {
        Debug.Log("Played");
    }

    public void Stopped()
    {
        Debug.Log("Stopre");
    }
    
}


