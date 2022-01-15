using Mono.Cecil;

namespace Rejuvena.Whisker.Cecil.Rewriting
{
    /// <summary>
    ///     Interface for installing in <see cref="IAssemblyRewriter"/>s.
    /// </summary>
    public interface IRewriterMethod
    {
        /// <summary>
        ///     Default rewriter method priority.
        /// </summary>
        public const float DefaultPriority = 1f;
        
        /// <summary>
        ///     The priority of the rewriter.
        /// </summary>
        float Priority { get; }

        /// <summary>
        ///     Performs operations on an assembly definition.
        /// </summary>
        /// <param name="assembly">The assembly to modify.</param>
        void Rewrite(AssemblyDefinition assembly);
    }
}