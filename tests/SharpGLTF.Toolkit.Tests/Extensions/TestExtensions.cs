using Newtonsoft.Json;
using NUnit.Framework;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.Parametric;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace SharpGLTF.Extensions
{
    internal class TestExtensions
    {
        [Test]
        public void TestGltf()
        {
            var scene = new SharpGLTF.Scenes.SceneBuilder();

            var material = new MaterialBuilder("material1")
                .WithDoubleSide(false)
                .WithMetallicRoughnessShader()
                .WithBaseColor(new Vector4(0.5f, 0.3f, 0.4f, 0.8f));

            var material1 = new MaterialBuilder("material1")
                .WithDoubleSide(false)
                .WithMetallicRoughnessShader()
                .WithBaseColor(new Vector4(0.6f, 0.8f, 0.7f, 1f));

            var mesh = new MeshBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>();
            mesh.AddCube(material, Matrix4x4.CreateScale(10));

            var root = new NodeBuilder("root");
            scene.AddRigidMesh(mesh, root);
           
            var node1 = new NodeBuilder("instance");
            scene.AddRigidMesh(mesh, node1);

            var model = scene.ToGltf2();

            //List<AffineTransform> ass = new List<AffineTransform>();
            //var rand = new Random();
            //List<int> fids = new List<int>();
            //List<building> bs = new List<building>();

            //for (int i = 0; i < 4; i++)
            //{
            //    fids.Add(i);
            //    bs.Add(new building() { height = i });
            //    ass.Add(AffineTransform.CreateFromAny(null, new Vector3(1 * (float)rand.NextDouble() + 0.5f, 1 * (float)rand.NextDouble() + 0.5f, 1 * (float)rand.NextDouble() + 0.5f), null, new Vector3(10 * (float)rand.NextDouble(), 10 * (float)rand.NextDouble(), 10 * (float)rand.NextDouble())));
            //}

            //model.LogicalNodes.Where(x => x.Name == "instance").FirstOrDefault()
            //    .UseGpuInstancing().WithInstanceAccessors(ass)
            //    .WithInstanceAccessor("_FEATURE_ID_0", fids).UseInstanceMetadata()
            //    .SetFeatureData(new FeatureID() { FeatureCount = 4, PropertyTable = 0, Label = "Builidngs" });

            ////Top-Po-tables
            //var f = model.UseStructuralMetadata();
            //var text = "{\"classes\":{\"building\":{\"name\":\"building\",\"properties\":{\"height\":{\"name\":\"height\",\"type\":\"SCALAR\",\"componentType\":\"FLOAT32\"}}}}}";
            //f.SetShcema(text);
            //f.WithFeatureAccessors(bs);
            //model.SetExtension(f);

            model.SaveGLTF(@$"preview.gltf", new WriteSettings() { MergeBuffers = false , JsonIndented = true});
            model.SaveGLB(@$"preview.glb", new WriteSettings() { MergeBuffers = false });
        }
    }

    public class building
    {
        [JsonProperty("height")]
        public float height { get; set; }
        //[JsonProperty("id")]
        //public int id { get; set; }
    }
}
