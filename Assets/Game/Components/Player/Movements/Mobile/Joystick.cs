using UnityEngine;

namespace Game.Player
{
    public class Joystick : FixedJoystick
    {
        public Movements movements;
        private void Update()
        {
            movements.characterController.transform.Rotate(0, Horizontal * movements.rotateSpeed, 0);
            movements.curSpeed += movements.speed * Vertical;
            if (Input.touchCount == 2)
            {
                movements.Jump();
            }

            movements.Move();
        }
    }
}
