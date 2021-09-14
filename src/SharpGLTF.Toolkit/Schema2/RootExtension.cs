using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpGLTF.Schema2
{
    public static partial class Toolkit
    {
        public static FeatureMetadata WithShcema(this FeatureMetadata metadata, string shcemaSTRING)
        {
            metadata.SetShcema(shcemaSTRING);
            return metadata;
        }

        public static unsafe FeatureMetadata WithFeatureAccessor<T>(this FeatureMetadata metadata, string table, string attribute, IReadOnlyList<T> values)
               where T : unmanaged
        {
            Guard.NotNull(metadata, nameof(metadata));
            Guard.NotNull(values, nameof(values));

            var root = metadata.LogicalParent;
            var view = root.CreateBufferView(values);
            var accessor = root.CreateAccessor();

            if (typeof(T) == typeof(int))
            {
                accessor.SetIndexData(view, 0, values.Count, IndexEncodingType.UNSIGNED_INT);
            }
            else
            {
                var dt = DimensionType.CUSTOM;
                if (typeof(T) == typeof(Single)) dt = DimensionType.SCALAR;
                if (typeof(T) == typeof(Vector2)) dt = DimensionType.VEC2;
                if (typeof(T) == typeof(Vector3)) dt = DimensionType.VEC3;
                if (typeof(T) == typeof(Vector4)) dt = DimensionType.VEC4;
                if (typeof(T) == typeof(Quaternion)) dt = DimensionType.VEC4;
                if (typeof(T) == typeof(Matrix4x4)) dt = DimensionType.MAT4;

                if (dt == DimensionType.CUSTOM) throw new ArgumentException(typeof(T).Name);

                accessor.SetVertexData(view, 0, values.Count, dt, EncodingType.FLOAT, false);
            }

            metadata.SetAccessor(table, attribute, accessor);

            return metadata;
        }

        public static FeatureMetadata WithFeatureAccessors<T>(this FeatureMetadata metadata, IReadOnlyList<T> instances)
        {
            Guard.NotNull(metadata, nameof(metadata));
            Guard.NotNull(instances, nameof(instances));
            var tablekey = instances.GetType().GenericTypeArguments.First().Name;

            foreach (var pop in typeof(T).GetProperties())
            {
                if (pop.PropertyType == typeof(int))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Int32>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }

                if (pop.PropertyType == typeof(Single))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Single>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }

                if (pop.PropertyType == typeof(Vector2))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Vector2>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }

                if (pop.PropertyType == typeof(Vector3))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Vector3>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }

                if (pop.PropertyType == typeof(Vector4))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Vector4>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }

                if (pop.PropertyType == typeof(Quaternion))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Quaternion>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }

                if (pop.PropertyType == typeof(Matrix4x4))
                {
                    var pops = instances.Select(item => Unsafe.Unbox<Matrix4x4>(pop.GetValue(item))).ToList();
                    metadata.WithFeatureAccessor(tablekey, pop.Name, pops);
                }
            }

            return metadata;
        }

        //public static dynamic Cast<T>(this Type type, object data)
        //{
        //    var DataParam = Expression.Parameter(typeof(object), "data");
        //    var Body = Expression.Block(type, Expression.Convert(Expression.Convert(DataParam, data.GetType()), type));
        //    var ff = typeof(Unsafe).GetMethod("Unbox");
        //    var Run = Expression.Lambda(type, Body, DataParam).Compile();
        //    var ret = Run.DynamicInvoke(data);
        //    UnaryExpression expression = Expression.Unbox(DataParam, type);
        //    return ret;
        //}
    }
}