namespace PlayerMovement;

using Godot;
using System;

public partial class Player : CharacterBody3D
{



    #region Exported properties

    [Export]
    public int MovementSpeed { get; set; } = 5;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    [Export]
    public float mouse_sensitivity = 0.002f;

    #endregion




    #region Private fields

    private Vector3 _targetVelocity = Vector3.Zero;

    #endregion





    public override void _Ready()
    {
        GD.Print("Player initialized");
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1.0f;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
        }

        _targetVelocity.X = direction.X * MovementSpeed;
        _targetVelocity.Z = direction.Z * MovementSpeed;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }


}
