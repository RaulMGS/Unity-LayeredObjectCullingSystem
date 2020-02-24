# Layered Object Culling System
A simple Unity3D culling system that takes advantage of layered culling API available in newer Unity versions.

# About

The LayeredObjectCulling system takes advantage of API that was added in unity recently, but was not implemented into the engine as a standalone feature. It allows for culling objects and shadowcasting independently and based on renderlayers, regardless of the LOD System culling. 

This technique is great for optimization of scenes that have lots of detail objects that should not render and/or cast shadows until close enough to the camera. 

# How to use

  1) Add the Culling System prefab to the scene
     (Alternatively add the LayeredObjectCulling component to one of the scene's objects)
  2) Add the LayeredObjectCullingCamera component to all of the cameras that should be affected by culling
  3) Add the LayeredObjectCullingLight component to all of the shadow casting lights whose shadows should be affected by culling
  4) Set up the layers to be used for the system in the layers panel of your project
  5) Add the layers to the LayeredObjectCulling component and set the desired render distances. Values of 0 mean default culling.
  
  **! Only one Culling System can be used at once (by design). Having more than one Culling System will lead to undefined behavior.**

  **! Support for Odin Inspector plugin included (and recommended)** 

# Inspired by Unity's Book of the Dead tech demo:

https://blogs.unity3d.com/2018/06/29/book-of-the-dead-quixel-wind-scene-building-and-content-optimization-tricks/
