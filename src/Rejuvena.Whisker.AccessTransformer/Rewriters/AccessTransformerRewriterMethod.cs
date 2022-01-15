using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Rejuvena.Whisker.Cecil.Rewriting;

namespace Rejuvena.Whisker.AccessTransformer.Rewriters
{
    public class AccessTransformerRewriterMethod : IRewriterMethod
    {
        public virtual float Priority => IRewriterMethod.DefaultPriority;

        public virtual AccessTransformerFile AccessTransformer { get; }

        public AccessTransformerRewriterMethod(AccessTransformerFile accessTransformer)
        {
            AccessTransformer = accessTransformer;
        }

        public virtual void Rewrite(AssemblyDefinition assembly)
        {
            foreach (TransformerNode node in AccessTransformer.Nodes)
            {
                List<TypeDefinition> types = new();

                foreach (TypeDefinition type in assembly.MainModule.Types)
                    CollectAllNested(type, types);

                foreach (TypeDefinition type in types)
                {
                    if (node.ObjectToTransform == type.FullName)
                    {
                        type.IsSealed = ReadonlyState(node, type.IsSealed);

                        if (type.IsSealed && type.IsAbstract)
                            type.IsSealed = false; // We don't want to ever make types static.
                    }
                    else
                    {
                        foreach (MethodDefinition method in type.Methods.Where(
                            method => node.ObjectToTransform == method.FullName)
                        )
                        {
                            method.IsFinal = ReadonlyState(node, type.IsSealed);

                            if (!StaticSafeOperation(node, method.IsStatic))
                                continue;

                            if (node.AccessorTransformation == AccessorTransformationType.Inherit)
                                continue;

                            if (node.AccessorTransformation == AccessorTransformationType.Internal)
                                method.IsAssembly = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.Private)
                                method.IsPrivate = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.Protected)
                                method.IsFamily = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.Public)
                                method.IsPublic = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.PrivateProtected)
                                method.IsFamilyAndAssembly = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.ProtectedInternal)
                                method.IsFamilyOrAssembly = true;
                        }

                        foreach (FieldDefinition field in type.Fields.Where(
                            field => node.ObjectToTransform == field.FullName)
                        )
                        {
                            field.IsInitOnly = ReadonlyState(node, field.IsStatic);

                            if (!StaticSafeOperation(node, field.IsStatic))
                                continue;

                            if (node.AccessorTransformation == AccessorTransformationType.Inherit)
                                continue;

                            if (node.AccessorTransformation == AccessorTransformationType.Internal)
                                field.IsAssembly = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.Private)
                                field.IsPrivate = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.Protected)
                                field.IsFamily = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.Public)
                                field.IsPublic = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.PrivateProtected)
                                field.IsFamilyAndAssembly = true;
                            else if (node.AccessorTransformation == AccessorTransformationType.ProtectedInternal)
                                field.IsFamilyOrAssembly = true;
                        }

                        /*foreach (PropertyDefinition property in type.Properties)
                        {
                            if (node.ObjectToTransform == property.FullName)
                            {
                                if (StaticSafeOperation(node, property.IsStatic))
                                {
                                }
                            }
                        }*/

                        /*foreach (EventDefinition @event in type.Events)
                        {
                            if (node.ObjectToTransform == @event.FullName)
                            {
                                if (!StaticSafeOperation(node, true))
                                    continue;

                                if (node.AccessorTransformation == AccessorTransformationType.Inherit)
                                    continue;

                                if (node.AccessorTransformation == AccessorTransformationType.Internal)
                                    @event.IsAssembly = true;
                                else if (node.AccessorTransformation == AccessorTransformationType.Private)
                                    @event.IsPrivate = true;
                                else if (node.AccessorTransformation == AccessorTransformationType.Protected)
                                    @event.IsFamily = true;
                                else if (node.AccessorTransformation == AccessorTransformationType.Public)
                                    @event.IsPublic = true;
                                else if (node.AccessorTransformation == AccessorTransformationType.PrivateProtected)
                                    @event.IsFamilyAndAssembly = true;
                                else if (node.AccessorTransformation == AccessorTransformationType.ProtectedInternal)
                                    @event.IsFamilyOrAssembly = true;
                            }
                        }*/
                    }
                }
            }
        }

        private static void CollectAllNested(TypeDefinition type, ICollection<TypeDefinition> types)
        {
            types.Add(type);

            if (!type.HasNestedTypes)
                return;

            foreach (TypeDefinition nested in type.NestedTypes)
                CollectAllNested(nested, types);
        }

        private static bool StaticSafeOperation(TransformerNode node, bool @static) =>
            node.AccessorTransformation.NoStatic && !@static || !node.AccessorTransformation.NoStatic;

        private static bool ReadonlyState(TransformerNode node, bool @readonly)
        {
            return node.ReadonlyTransformation == ReadonlyTransformationType.Inherit
                ? @readonly
                : node.ReadonlyTransformation == ReadonlyTransformationType.Readonly;
        }
    }
}