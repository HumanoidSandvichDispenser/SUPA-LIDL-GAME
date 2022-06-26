using Godot;
using System;

namespace SupaLidlGame.UI
{
    public class FloatingText : Position2D
    {
        private Label _label;
        private Tween _tween;

        [Export]
        public string Text { get; set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _label = GetNode<Label>("Label");
            _tween = GetNode<Tween>("Tween");

            Random rng = new Random();
            float randomFloat(float min, float max)
            {
                return (float)rng.NextDouble() * (max - min) + min;
            }

            var start = Position + new Vector2(randomFloat(-8, 8), 0);
            var end = start + new Vector2(0, randomFloat(-8, -16));
            var endColor = new Color(1, 1, 1, 0);
            var endScale = new Vector2(0.5f, 0.5f);

            _label.Text = Text;

            _tween.InterpolateProperty(this, "position", start, end, 0.5f,
                    Tween.TransitionType.Quint, Tween.EaseType.Out);

            _tween.InterpolateProperty(this, "modulate", Modulate, endColor,
                    0.5f, Tween.TransitionType.Quint, Tween.EaseType.Out, 1.5f);

            _tween.InterpolateProperty(this, "scale", Scale, endScale, 0.5f,
                    Tween.TransitionType.Linear, Tween.EaseType.Out, 1.5f);

            _tween.Start();

            GetNode<AnimationPlayer>("AnimationPlayer").Play("Flash");
        }

        public void _on_Tween_tween_all_completed()
        {
            QueueFree(); // remove from the world once the tween is finished
        }
    }
}
