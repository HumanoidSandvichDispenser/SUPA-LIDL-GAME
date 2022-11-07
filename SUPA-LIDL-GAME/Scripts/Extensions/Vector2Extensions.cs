using Godot;

namespace SupaLidlGame
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Resizes a <see cref="Vector2"/> to the specified <paramref
        /// name="magnitude"/>.
        /// </summary>
        public static Vector2 Resized(this Vector2 vector2, float magnitude)
        {
            return vector2.Normalized() * magnitude;
        }
    }
}
