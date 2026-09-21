namespace PlayerMovement;

using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{



    #region Exported properties

    [Export]
    public int MovementSpeed { get; set; } = 5;

    [Export]
    public int FallAcceleration { get; set; } = 35;

    [Export]
    public float MouseSensitivity = 0.002f;

    #endregion

    #region Private fields

    private Vector3 _targetVelocity = Vector3.Zero;
    private float _pitch = 0f;
    private Node3D _camera;
    private Node3D _yaw;
    private List<IMovementAbility> _abilities = [];

    #endregion




    // # Perform Camera Rotation
    private void PerformRotation(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);

            _pitch -= mouseMotion.Relative.Y * MouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));
            _camera.Rotation = new Vector3(_pitch, 0f, 0f);
        }
    }

    // # Perform Movement initialisation
    private void InitializeCamera()
    {
        _yaw = GetNode<Node3D>("Pivot");
        _camera = GetNode<Camera3D>("Pivot/Camera3D");

        Input.MouseMode = Input.MouseModeEnum.Captured;
        GD.Print("Player Camera initialized");
    }

    // # Get Player Movement Input
    private Vector3 GetInput()
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

        return direction;
    }

    // # Perform Player Movement
    private void PerformMovement(double delta)
    {
        var direction = GetInput();



        _targetVelocity.X = direction.X * MovementSpeed;
        _targetVelocity.Z = direction.Z * MovementSpeed;

        if (IsOnFloor() && _targetVelocity.Y < 0)
            {
                _targetVelocity.Y = -0.1f;
            }
            else if (!IsOnFloor())
            {
                _targetVelocity.Y -= FallAcceleration * (float)delta;
            }

        foreach (var ability in _abilities)
        {
            ability.PhysicsUpdate(delta, ref _targetVelocity, this);
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    private void InitializeAbilities()
    {
        _abilities.Add(new JumpAbility(jumpVelocity: 10f, maxJumps: 1));
    }




    // ? MAIN PROGRAM
    public override void _Input(InputEvent @event)
    {
        PerformRotation(@event);
    }

    public override void _Ready()
    {
        InitializeCamera();
        InitializeAbilities();
    }

    public override void _PhysicsProcess(double delta)
    {
        PerformMovement(delta);
    }
}
