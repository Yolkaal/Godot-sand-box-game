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
    public float MouseSensitivity = 0.002f;

    #endregion




    #region Private fields

    private Vector3 _targetVelocity = Vector3.Zero;
    private float _pitch = 0f;
    private Node3D _camera;
    private Node3D _yaw;

    #endregion


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);

            _pitch -= mouseMotion.Relative.Y * MouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));
            _camera.Rotation = new Vector3(_pitch, 0f, 0f);
        }
    }


    public override void _Ready()
    {
        GD.Print("Player initialized");
        _yaw = GetNode<Node3D>("Pivot");
        _camera = GetNode<Camera3D>("Pivot/Camera3D");

        Input.MouseMode = Input.MouseModeEnum.Captured;
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
            direction = Transform.Basis * direction.Normalized();
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
