using Mono.Cecil;

namespace Rejuvena.Whisker.Cecil.Rewriting
{
    /// <summary>
    ///     Interface for rewriting assemblies with <c>Mono.Cecil</c>.
    /// </summary>
    public interface IAssemblyRewriter
    {
        /// <summary>
        ///     Installs a rewriter method to this assembly rewriter.
        /// </summary>
        /// <param name="rewriterMethod">The rewriter instance.</param>
        void AddRewriterMethod(IRewriterMethod rewriterMethod);
        
        /// <summary>
        ///     Perform an assembly rewriting job based on the installed rewriter methods.
        /// </summary>
        void Rewrite(AssemblyDefinition assembly);
    }
}