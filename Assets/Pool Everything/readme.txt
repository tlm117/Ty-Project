Pool Everything Plugin
--------------------------------------------------------------------------------------------------------
Unity Game Engine

Object Pooling Components

Written in Unity Version 4.6.0f2

*Requires Unity 4.6.0 or higher

Version 1.0.0	Release Date: 09/21/2014

Initial Release


DOCUMENTATION, TUTORIALS
--------------------------------------------------------------------------------------------------------
[Documentation](http://www.dyamondrose.com/callowcreation/products/unity/pooleverything/documentation/v1_0_0/html/G_PoolEverything.htm)

[Samples](https://www.assetstore.unity3d.com/#!/content/46276)

[Slide Tutorial](https://docs.google.com/presentation/d/15jIfNjDXkKY55hJoKbm3pwoSDUBeAiz66bkH_YbohiU/edit?usp=sharing)

[Video Tutorials](https://www.youtube.com/playlist?list=PLTEnuRWq5EdZ-VofuVNjSSQizf_h276cs)

WHAT IT DOES
--------------------------------------------------------------------------------------------------------
Pool Everything is a no scripting required object pooling system.  Pooling GameObject(s) provide a few basic benefits including the limiting of runtime garbage collection.  Pool Everything uses a component-based system that instantiates all objects identified by the Pool Manager(s) in the scene.  Creating all objects to be used in this manner provides immediate access to available objects during game play without the need for instantiating another GameObject.  When an object is no longer needed, by traditional means, the GameObject would be destroyed and the potential for the garbage-collection process to start is risked.  At this point, when an object has outlived its use, Pool Everything’s recyclers will place it on standby awaiting the next request.

The Pool(s), Recycler(s) and Spawner(s) communicate via components attached to pooled objects at creation time.  This pipeline is designed to maximize efficiency and can be further optimized by adjusting the values on the exposed properties on each component in the inspector.  These built-in component suite provides a comprehensive toolset to pool, spawn and reuse any Unity GameObject.  Major components like the Pool Manager are available in a prefab form to facilitate ease of use.

All major components can be extended to accommodate project specific use-cases.  However a combined set of recycler and spawner components have been provided to help integrate Pool Everything into existing projects.  If imported into a new project the built-in components provides all functionality required to pool, spawn and recycle GameObject(s) without any scripting.

GENERAL USAGE NOTES
--------------------------------------------------------------------------------------------------------
**NOTE**: All Pool Everything components can be placed on any scene view GameObject and perform as expected.  This allows for grouping them in the hierarchy to keep things organized and clean.

1. Add Pool Manager:  A pool manager is the component responsible for managing the scene pools.
>+ Add component behavior PoolManager to any GameObject.
>+ Use the PoolManager prefab provided Assets/PoolEverything/Prefabs/Pool Manager.prefab.
2. Add Pool Spawner: A pool spawner spawns pooled object at runtime.
>+ Add a component behavior that derives from PoolSpawner to any GameObject.
>+ Select a pool manager from the popup
>+ Select an object to spawn from the provided popup.
3. Add Pool Recycler: A pool recycler places pooled object on standby, deactivates it, for reuse.
>+ Add a component behavior that derives from PoolRecycler to any GameObject.
>+ Select a pool manager from the popup
>+ Select an object to recycle from the provided popup.

EXTRAS
--------------------------------------------------------------------------------
Features Include: (*but not limited to*)

+ Collider Spawner (abstract)
+ Trigger Spawner (concrete) - Spawns a pooled object when the Spawners trigger collider is interacted with (all trigger events can be monitored)
+ Collision Spawner (concrete) - Spawns a pooled object when the Spawners collision collider is interacted with (all collision events can be monitored)
+ Conditional Spawner - Spawns a pooled object depending on field and property values on any scene view GameObject
+ Input Spawner (abstract)
+ Input Mouse Spawner (concrete) - Replaces Click Spawner, allows for all mouse input click events
+ Input Button Spawner (concrete) - Uses Input manager axes key bindings
+ Input Key Spawner (concrete) - Allows for KeyCode access for Input GetKey functions
+ Input Touch Spawner - Detection of touch specific inputs to spawn pooled objects.
+ Value Recycler - Now used to recycle depending on pooled objects field and property values
+ Conditional Recycler - Replaces the functionality of the Value Recycler with the addition of property accessibility, like the Conditional Spawner any scene view GameObject can be monitored.
+ Collider Recycler (abstract)
+ Trigger Recycler (concrete) - Recycles a pooled object when it’s collider interacts with another trigger collider (all trigger events can be monitored)
+ Collision Recycler (concrete) - Recycles a pooled object when a collision interaction with another collision collider (all collision events can be monitored)

CONTACT
--------------------------------------------------------------------------------------------------------
Questions, Comments, Concerns, Requests:

email: 	callowcreation@gmail.com

web: 	www.callowcreation.com

[Customer Support](http://goo.gl/forms/dUw3c7KPRC)