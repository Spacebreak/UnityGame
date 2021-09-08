using System.Collections;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Elevator[] elevators;    //The collection of elevators in this connected group.

    private int floorSelected;      //The desired floor to take the elevator to.
    private int floorDestination;   //The floor that the elevator last moved to.
    private bool elevatorCooldown;  //Tracks if the elevator has moved recently.

    //Selects the next floor in the series. Will not select the same floor that the player is on.
    public void ChangeFloorSelection(int currentFloor)
    {
        floorSelected++;

        if (floorSelected == currentFloor)
        {
            floorSelected++;
        }

        if (floorSelected >= elevators.Length)
        {
            if (currentFloor == 0)
            {
                floorSelected = 1;
            }
            else
            {
                floorSelected = 0;
            }
        }

        Debug.Log(floorSelected + " <- Floor Selected");
    }

    public Elevator ReturnDestinationElevatorClass()
    {
        return elevators[floorDestination];
    }

    public Transform ReturnFloorPlatformLocation()
    {
        return elevators[floorSelected].liftPlatform;
    }

    public void SetFloorDestination()
    {
        floorDestination = floorSelected;
    }

    public int GetFloorSelected()
    {
        return floorSelected;
    }

    public IEnumerator SetElevatorCooldown()
    {
        elevatorCooldown = true;

        //Needs a long cooldown to prevent the player activating the lift and warping behind the doors.
        yield return new WaitForSeconds(7.0f);
        elevatorCooldown = false;
    }

    public bool GetElevatorCooldown()
    {
        return elevatorCooldown;
    }
}
