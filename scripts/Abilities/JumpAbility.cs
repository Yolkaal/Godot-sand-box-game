namespace PlayerMovement;

using Godot;

public class JumpAbility : IMovementAbility
{
    private readonly float _jumpVelocity;
    private readonly int _maxJumps;
    private int _jumpsRemaining;

    public JumpAbility(float jumpVelocity, int maxJumps)
    {
        _jumpVelocity = jumpVelocity;
        _maxJumps = maxJumps;
    }

    public void PhysicsUpdate(double delta, ref Vector3 velocity, Player player)
    {
        if (player.IsOnFloor())
            _jumpsRemaining = _maxJumps;

        if (Input.IsActionJustPressed("jump") && _jumpsRemaining > 0)
        {
            velocity.Y = _jumpVelocity;
            _jumpsRemaining--;
        }
    }
}
