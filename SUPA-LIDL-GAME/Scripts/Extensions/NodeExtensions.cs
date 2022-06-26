using Godot;

namespace SupaLidlGame.Extensions
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Finds the first parent of the type <typeparamref name="T"/>
        /// </summary>
        public static T GetFirstParentOfClass<T>(this Node node) where T : class
        {
            Node _node;

            for (_node = node.GetParent();
                    !(_node is null || _node is T);
                    _node = _node.GetParent())
            {
                _node = _node.GetParent();
            }

            return _node as T;
        }
    }
}
