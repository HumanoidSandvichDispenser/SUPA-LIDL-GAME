using Godot;

/// <summary>
/// A particle system that deletes itself when <see cref="Emit()" is called/>
/// and all particles have finished emitting.
/// </summary>
public class OneShotParticles : Particles2D
{
    private bool _isEmitting = false;

    public void Emit()
    {
        Emitting = _isEmitting = true;
    }

    public override void _Process(float delta)
    {
        if (_isEmitting && !Emitting)
        {
            QueueFree();
        }
    }
}
