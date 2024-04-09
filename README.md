# HomaTest

1. Perform an analysis of the structure and performance of the project
Project Architecture: Examine the setup of the Unity Project and Game Scenes as well as their overall configuration.
System Architecture: Deep dive into the codebase to understand and evaluate the current architecture. Identify and suggest improvements or alternatives to the design patterns and practices in use.
Code Quality: Examine the code quality of the project. Share some tips that could improve the general code quality.
Performance: Identify performance issues or bottlenecks as well as optimization opportunities regarding fields like Rendering, Scripting, Memory Usage, Asset Optimization, etc.



Project Architecture: Project setup overall is ok, I don't see major problems with it. About the GameScene I usually have a gameObject that contains all the Managers(game,pool,audio,particles etc).
I prefer not to have UI element inside a not Transform/parent, I prefer to have the UIManager separated from the ManagerContainer.

System Architecture: About the code on itself, I wouldn't have used a simple static class for the FxPool, but I would have created a Singleton(plus gameobject) where to store the PS, 
and because there is already a pooler active I wouldn't create all those partcicles on the GameManager Awake, that's exactly why the pooler was created, so that we don't have object waiting before we need them.

Code Quality: I also found in Tower.cs that when we intsanciate all the tiles we do a new() of the array that will contain all the tiles by floor, in that case if we use .Clear() the game will not create another List<List<TowerTile>> every time we enter the method.
in the same method we also instantiate all the tiles and we are not using a support variable for that, so it means that every time we cicle that part of code the game is creating space for a new instance 
at the same time loosing the reference to the previous gameObject, in a much bigger game the GC would have pass at the end of the frame to clean all the unreferenced gameObjects.
https://discussions.unity.com/t/is-new-list-t-worse-than-using-clear/6801

Performance: This is the part that I love the most. 
In 1_Graphics>UI we have all the UI asset and they should be in a SpriteAtlas in order to reduce DrawCalls.
In 1_Graphics>FBX there are 2 different types of cylinder one with 12 faces one with 24, in this case we could use the 12 faces instead of the 24 ones, like this we reduce the tris by 9k and vertex by 11k.
I saw that we have around 280+ batches from the frame debugger and most of them are because the material of the tiles is not shared, so I created a materialManager that gives the right sharedMaterial to the right Tile,
reduceing the batches from 280+ to 90. (the same process can be done with the disables tiles to reduce the number even more) 
Having a SpriteAtlas for the UI also reduces the ram usage because the SpriteAtlas is a multiple of 2 and so it can be compressed.(I used the Crunched DXT5 with 100% quality)
I was also thinking abut the Addressables.LoadSceneAsync for the scene loading, I think if we have many small scenese this would allow to use less memory because most of the asset will be part of the assetBoundle,
so every time we load a scene we load just the gameObjects of that scene, and when the scene is unloaded we unload all the gameOnject of that scene, in this way we don't leave unused prefabs/gameObject on the RAM.
 
https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/LoadSceneAsync.html




2. Update the initialization sequence
In this task, we’re going to soon add multiple SDKs to the project, but currently, the initialization lacks structure and sequencing allowing us to plug in different mechanisms during the process (and for example hold on the startup until some systems are ready).
Prepare a code solution that would streamline the initialization process of the game. There is no need to dive deep and rework the entire app. Showcase the general idea via code and briefly explain the pros and cons. Build a plan to migrate the initialization workflow to your idea.

For this task I thought about what usually we have in all games, so I created a quick Loading page. On Start() I just call the coroutine that will handle the loading of the GameScene.
In this coroutine I also started all the SDK initialization that we need and we force the coroutine to wait until all the SDKs are initilaized, I did 2 quick example of how we can wait for the SDKs, 
using "yield return SdkCallback()" in case is needed befor the scene load, otherwise I started some coroutines to just initialize the SKDs needed and I am waiting for the SKDs to do their callback.
    