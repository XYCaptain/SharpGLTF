using SharpGLTF.CodeGen;
using SharpGLTF.SchemaReflection;
using System.Collections.Generic;

namespace SharpGLTF
{


	class StructuralMetadataSchema : SchemaProcessor
	{
		private static string RootSchemaUri => Constants.VendorExtensionPath("EXT_structural_metadata", "glTF.EXT_structural_metadata.schema.json");

		public override void PrepareTypes(CSharpEmitter newEmitter, SchemaType.Context ctx)
		{
			newEmitter.SetRuntimeName("EXT_structural_metadata glTF extension", "EXT_structural_metadata");
		}

		public override IEnumerable<(string TargetFileName, SchemaType.Context Schema)> Process()
		{
			yield return ("Ext.EXT_structural_metadata.g", ProcessRoot());
		}

		private static SchemaType.Context ProcessRoot()
		{
			var ctx = SchemaProcessing.LoadSchemaContext(RootSchemaUri);
			//ctx.IgnoredByCodeEmitter("glTF Property");
			//ctx.IgnoredByCodeEmitter("glTF Child of Root Property");
		
			return ctx;
		}
	}

	class MeshFeature : SchemaProcessor
	{
		private static string RootSchemaUri => Constants.VendorExtensionPath("EXT_mesh_features", "mesh.primitive.EXT_mesh_features.schema.json");

		public override void PrepareTypes(CSharpEmitter newEmitter, SchemaType.Context ctx)
		{
			newEmitter.SetRuntimeName("EXT_mesh_features glTF Mesh Primitive extension", "EXT_mesh_features");
		}

		public override IEnumerable<(string TargetFileName, SchemaType.Context Schema)> Process()
		{
			yield return ("Ext.EXT_mesh_features.g", ProcessRoot());
		}

		private static SchemaType.Context ProcessRoot()
		{
			var ctx = SchemaProcessing.LoadSchemaContext(RootSchemaUri);
			ctx.IgnoredByCodeEmitter("glTF Property");
			ctx.IgnoredByCodeEmitter("glTF Child of Root Property");
			ctx.IgnoredByCodeEmitter("Texture Info");

			return ctx;
		}
	}

	class PrimitiveStructuralMetadata : SchemaProcessor
	{
		private static string RootSchemaUri => Constants.VendorExtensionPath("EXT_structural_metadata", "mesh.primitive.EXT_structural_metadata.schema.json");

		public override void PrepareTypes(CSharpEmitter newEmitter, SchemaType.Context ctx)
		{
			newEmitter.SetRuntimeName("EXT_structural_metadata glTF extension", "EXT_structural_metadata");
		}

		public override IEnumerable<(string TargetFileName, SchemaType.Context Schema)> Process()
		{
			yield return ("Ext.EXT_structural_metadata.g", ProcessRoot());
		}

		private static SchemaType.Context ProcessRoot()
		{
			var ctx = SchemaProcessing.LoadSchemaContext(RootSchemaUri);
			ctx.IgnoredByCodeEmitter("glTF Property");
			ctx.IgnoredByCodeEmitter("glTF Child of Root Property");

			return ctx;
		}
	}


	class NodeInstanceMetadata : SchemaProcessor
	{
		private static string RootSchemaUri => Constants.VendorExtensionPath("EXT_instance_features", "Node.EXT_instance_features.schema.json");

		public override void PrepareTypes(CSharpEmitter newEmitter, SchemaType.Context ctx)
		{
			newEmitter.SetRuntimeName("EXT_instance_features glTF extension", "EXT_structural_metadata");
		}

		public override IEnumerable<(string TargetFileName, SchemaType.Context Schema)> Process()
		{
			yield return ("Ext.EXT_instance_features.g", ProcessRoot());
		}

		private static SchemaType.Context ProcessRoot()
		{
			var ctx = SchemaProcessing.LoadSchemaContext(RootSchemaUri);
			ctx.IgnoredByCodeEmitter("glTF Property");
			ctx.IgnoredByCodeEmitter("glTF Child of Root Property");

			return ctx;
		}
	}

}
