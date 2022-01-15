using System;
using System.Collections.Generic;
using System.Linq;

namespace Rejuvena.Whisker.AccessTransformer
{
    /// <summary>
    ///     Access transformer node.
    /// </summary>
    public class TransformerNode
    {
        public readonly string ObjectToTransform;
        public readonly AccessorTransformationType AccessorTransformation;
        public readonly ReadonlyTransformationType ReadonlyTransformation;

        public TransformerNode(
            string objectToTransform,
            AccessorTransformationType? accessorTransformation,
            ReadonlyTransformationType? readonlyTransformation
        )
        {
            ObjectToTransform = objectToTransform;
            AccessorTransformation = accessorTransformation ?? AccessorTransformationType.Inherit;
            ReadonlyTransformation = readonlyTransformation ?? ReadonlyTransformationType.Inherit;
        }

        public override string ToString()
        {
            List<string> values = new();

            if (AccessorTransformation != AccessorTransformationType.Inherit)
                values.Add(AccessorTransformation.Value);

            if (ReadonlyTransformation != ReadonlyTransformationType.Inherit)
                values.Add(ReadonlyTransformation.Value);

            values.Add(ObjectToTransform);

            return string.Join(" ", values);
        }

        public static TransformerNode Parse(string value)
        {
            string[] elements = value.Split(' ', 3);

            return new TransformerNode(
                elements[2],
                AccessorTransformationType.Parse(elements[0]),
                ReadonlyTransformationType.Parse(elements[1])
            );
        }
    }
}