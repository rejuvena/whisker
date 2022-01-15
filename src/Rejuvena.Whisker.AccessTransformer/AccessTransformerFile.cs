using System.IO;
using System.Linq;
using System.Text;

namespace Rejuvena.Whisker.AccessTransformer
{
    /// <summary>
    ///     Basic class for reading and writing access transformer files. Instances store transformation nodes.
    /// </summary>
    public class AccessTransformerFile
    {
        public virtual TransformerNode[] Nodes { get; }

        public AccessTransformerFile(params TransformerNode[] nodes)
        {
            Nodes = nodes;
        }

        public static AccessTransformerFile ReadFile(string path)
        {
            string[] lines = File.ReadAllLines(path);

            return new AccessTransformerFile(
                (
                    from line in lines
                    where !string.IsNullOrWhiteSpace(line) && !line.StartsWith('#')
                    select TransformerNode.Parse(line)
                ).ToArray()
            );
        }

        public virtual void Write(StreamWriter writer) => Write(this, writer);

        public virtual void WriteFile(string path) => WriteFile(this, path);

        public static void Write(AccessTransformerFile file, StreamWriter writer) => writer.Write(file.ToString());

        public static void WriteFile(AccessTransformerFile file, string filePath) =>
            File.WriteAllText(filePath, file.ToString());

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach (TransformerNode node in Nodes)
                sb.AppendLine(node.ToString());

            return sb.ToString();
        }
    }
}