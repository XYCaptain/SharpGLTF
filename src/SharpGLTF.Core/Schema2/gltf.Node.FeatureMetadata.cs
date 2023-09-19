using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using SharpGLTF.Collections;
using SharpGLTF.IO;

namespace SharpGLTF.Schema2
{

	partial class FeatureID {

		public int? Attribute
		{
			get { return _attribute; }
			set { _attribute = value; }
		}

		public int FeatureCount {
			get { return Math.Max(_featureCount, _featureCountMinimum); }
			set { _featureCount = value; }
		}

		public string Label
		{
			get { return _label; }
			set { _label = value; }
		}


		public int? NullFeatureId
		{
			get { return _nullFeatureId.HasValue ? Math.Max(_nullFeatureIdMinimum, _nullFeatureId.Value): _nullFeatureId; }
			set { _nullFeatureId = value; } 
		}

		public int? PropertyTable
		{
			get { return _propertyTable; }
			set { _propertyTable = value.HasValue ? Math.Max(_propertyTableMinimum, value.Value):value; }
		}
    }

	partial class FeatureMetadataInstancer<T> : FeatureIDs
	{
		private readonly T _Owner;

		internal FeatureMetadataInstancer(T root)
		{
			_Owner = root;
			_featureIds = new();
		}

		void ClearAccessors()
		{
			_featureIds.Clear();
		}

		internal FeatureMetadataInstancer<T> SetFeatureData(FeatureID featureid)
		{
			_featureIds.Add(featureid);
			return this;
		}
	}

	partial class InstanceMetaInstancer<T> : FeatureMetadataInstancer<T>
	{
		internal InstanceMetaInstancer(T root) : base(root)
		{
		}
	}

	static class FeatureMetadataInstancerExt
	{
		public static InstanceMetaInstancer<Node> GeInstanceMetadata(this MeshGpuInstancing obj)
		{
			return obj.GetExtension<InstanceMetaInstancer<Node>>();
		}
		public static InstanceMetaInstancer<Node> GeInstanceMetadata(this Node obj)
		{
			return obj.GetExtension<InstanceMetaInstancer<Node>>();
		}
		public static InstanceMetaInstancer<Node> UseInstanceMetadata(this MeshGpuInstancing obj)
		{
			var ext = obj.GeInstanceMetadata();
			if (ext == null)
			{
				ext = new InstanceMetaInstancer<Node>(obj.LogicalParent);
				obj.LogicalParent.SetExtension(ext);
			}
			return ext;
		}


		public static InstanceMetaInstancer<Node> UseInstanceMetadata(this Node obj)
		{
			var ext = obj.GeInstanceMetadata();

			if (ext == null)
			{
				ext = new InstanceMetaInstancer<Node>(obj);
				obj.SetExtension(ext);
			}

			return ext;
		}
		public static FeatureMetadataInstancer<MeshPrimitive> GetFeatureMetadata(this MeshPrimitive obj)
		{
			return obj.GetExtension<FeatureMetadataInstancer<MeshPrimitive>>();
		}

		public static FeatureMetadataInstancer<Node> GetFeatureMetadata(this Node obj)
		{
			return obj.GetExtension<FeatureMetadataInstancer<Node>>();
		}

		public static FeatureMetadataInstancer<Node> UseFeatureMetadata(this Node obj)
		{
			var ext = obj.GetFeatureMetadata();
			if (ext == null)
			{
				ext = new FeatureMetadataInstancer<Node>(obj);
				obj.SetExtension(ext);
			}

			return ext;
		}

		public static void RemoveFeatureMetadata(this MeshGpuInstancing obj)
		{
			obj.RemoveExtensions<FeatureMetadataInstancer<MeshGpuInstancing>>();
		}

		public static void RemoveFeatureMetadata(this Node obj)
		{
			obj.RemoveExtensions<FeatureMetadataInstancer<Node>>();
		}

		public static void RemoveFeatureMetadata(this MeshPrimitive obj)
		{
			obj.RemoveExtensions<FeatureMetadataInstancer<MeshPrimitive>>();
		}
	}
}
