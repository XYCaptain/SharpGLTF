<p align="center">
<img src="build/Icons/glTF2Sharp.png" height=128 />
</p>

![GitHub](https://img.shields.io/github/license/XYCaptain/SharpGltf)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/SharpGltfTileExt)](https://www.nuget.org/packages/SharpGltfTileExt/0.0.4)
---

### Overview
Fork and Update from __SharpGLTF__(a 100% .NET Standard library), which designed to support
[Khronos Group glTF 2.0](https://github.com/KhronosGroup/glTF) file format.

### RoadMap
- [x] Update to Net 6.0
- [ ] Add Ext_feature_metadata




### Appendix A - [SharpGLTF](https://github.com/vpenades/SharpGLTF)
#### Quickstart

A simple example of loading a glTF file and saving it as GLB:

```c#
var model = SharpGLTF.Schema2.ModelRoot.Load("model.gltf");
model.SaveGLB("model.glb");
```
More examples can be found [here](examples) and in the Test project.

#### Nuget Packages

|Package|Version|
|-|-|
|[__SharpGLTF.Core__](https://www.nuget.org/packages/SharpGLTF.Core)|![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/SharpGLTF.Core)|
|[__SharpGLTF.Toolkit__](https://www.nuget.org/packages/SharpGLTF.Toolkit)|![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/SharpGLTF.Toolkit)|

The library is divided into two main packages:

- [__SharpGLTF.Core__](src/SharpGLTF.Core/README.md) provides read/write file support, and low level access to the glTF models.
- [__SharpGLTF.Toolkit__](src/SharpGLTF.Toolkit/README.md) provides convenient utilities to help create, manipulate and evaluate glTF models.

### Appendix B
- [Khronos Group glTF-CSharp-Loader](https://github.com/KhronosGroup/glTF-CSharp-Loader)
- [Khronos Group UnityGLTF](https://github.com/KhronosGroup/UnityGLTF)

