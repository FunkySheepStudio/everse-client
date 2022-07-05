using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public Game.Player.Inputs.Manager inputsManager;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            inputsManager.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            inputsManager.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            inputsManager.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            inputsManager.SprintInput(virtualSprintState);
        }
        
    }

}
