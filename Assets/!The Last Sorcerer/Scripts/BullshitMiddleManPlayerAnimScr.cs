using UnityEngine;

public class BullshitMiddleManPlayerAnimScr : MonoBehaviour
{
    public GameObject actualFuckingPlayer;
    public scr_playerController actualFuckingScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actualFuckingPlayer = GameObject.Find("player");
        actualFuckingScript = actualFuckingPlayer.GetComponent<scr_playerController>();
    }

    public void ActualFuckingAttack() { actualFuckingScript.Attack(); }
    public void ActuallEnableMove() { actualFuckingScript.EnableMovement(); }
    public void ActuallDisbleMove() { actualFuckingScript.DisableMovement(); }
    public void ActuallStun() { actualFuckingScript.StunMe(); }
    public void ActuallDeStun() { actualFuckingScript.DeStunMe(); }



}
