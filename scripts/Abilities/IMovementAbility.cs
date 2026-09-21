namespace PlayerMovement;

public interface IMovementAbility
{
    void PhysicsUpdate(double delta, ref Godot.Vector3 velocity, Player player);
}
