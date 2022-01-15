using System;
using System.Collections.Generic;
using Mono.Cecil;
using NUnit.Framework;
using Rejuvena.Whisker.AccessTransformer.Rewriters;
using Rejuvena.Whisker.Cecil.Rewriting;

namespace Rejuvena.Whisker.AccessTransformer.Tests
{
    public static class Tests
    {
        private class TestAssemblyRewriter : IAssemblyRewriter
        {
            private List<IRewriterMethod> Methods = new();

            public void AddRewriterMethod(IRewriterMethod rewriterMethod) => Methods.Add(rewriterMethod);

            public void Rewrite(AssemblyDefinition assembly)
            {
                // No priority sorting
                foreach (IRewriterMethod method in Methods)
                    method.Rewrite(assembly);
            }
        }

        [Test]
        public static void TransformDummyTest()
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly("DummyProject.dll");

            IAssemblyRewriter rewriter = new TestAssemblyRewriter();
            rewriter.AddRewriterMethod(new AccessTransformerRewriterMethod(new AccessTransformerFile(
                TransformerNode.Parse("public = System.String DummyProject.MySimpleClass/MyNestedClass::MyNestedField")
            )));
            rewriter.Rewrite(assembly);

            assembly.Write("DummyProject_Transformed.dll");
        }

        [Test]
        public static void PrintNodeTest()
        {
            PrintEquality(
                new TransformerNode(
                    "My.Project.MyClass",
                    AccessorTransformationType.Internal,
                    ReadonlyTransformationType.Readonly).ToString(),
                "internal +r My.Project.MyClass"
            );

            PrintEquality(
                new TransformerNode(
                    "My.Project.MyOtherClass",
                    AccessorTransformationType.Internal,
                    ReadonlyTransformationType.ReadWrite).ToString(),
                "internal -r My.Project.MyOtherClass"
            );
        }

        public static void PrintEquality(object valueOne, object valueTwo, bool equal = true)
        {
            bool valuesEqual = valueOne.Equals(valueTwo);

            Console.WriteLine(
                $"{valueOne} {(equal ? "==" : "!=")} {valueTwo} - {(equal ? valuesEqual : !valuesEqual)}"
            );

            if (equal ? valuesEqual : !valuesEqual)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("PASSED");
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("FAILED");
                Console.ResetColor();
                Assert.Fail();
            }
        }
    }
}