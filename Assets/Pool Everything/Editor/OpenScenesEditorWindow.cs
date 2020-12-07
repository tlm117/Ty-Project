#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: PoolEverything
     
*/
#endregion

using UnityEditor;
using UnityEngine;
using System.Linq;
using PoolEverything;
using TryItCompatibilityEditor;

namespace PoolEverythingEditor
{
    /// <summary>
    /// Opens example scene picker
    /// </summary>
    [InitializeOnLoad]
    internal sealed class OpenScenesEditorWindow
    {
        static OpenScenesEditorWindow()
        {
            //Debug.Log("OpenScenesEditorWindow UpdateShowPrompt");
            ExampleScenesWindow.newsletterSignupUrl = "http://eepurl.com/bPDq3P";
            ExampleScenesWindow.isFullVersion = !PoolSettings.Instance.GetTryIt();
            ExampleScenesWindow.packageName = "Pool Everything";
            ExampleScenesWindow.buttonTexts = new SceneButton[]
            {
                new SceneButton("Open Example Scene", "Example Scene", "Assets/Pool Everything/Scenes/Example Empty.unity",
                    "This scene, Example Empty, is a quick show how to utilize the  component system.  There are 2 Samples that are included that has detailed scenes for your experimentation."),
                new SceneButton("Open Example Multi Pool Scene", "Example Multi Pool Scene", "Assets/Pool Everything/Scenes/Example Multi Pool Empty.unity",
                    "This scene, Example Multi Pool Empty, is a quick show how to utilize the  multi-pool script with an example.  There is an Example Empty scene that is included that shows the component usage."),
                new SceneButton("Open Snake Head Sample", "Snake Head Sample", "Assets/Pool Everything/Samples/Snake Head/Scenes/No Coding Pool.unity",
                    "Snake Head is a game just waiting to happen! There is a Sentinel Cube that sweeps its view between two adjustable angles. There is a Snake Head Sphere that move like the old Centipede game. Your task make the snake head spawn new links and destroy those links with bullets fired from the sentinel. This can all be done using the built-in components that Pool Everything™ provides. No coding is needed to do this. The rest will be left up to your imagination.  Use Pool Everything to start a simple game using built-in components.  The Spawner and Recycler components are used to spawn bullets and snake links.  Advanced components are used to reduce armor on the snake links and spawn particles on bullet impacts."),
                new SceneButton("Open Candy Hunt Sample", "Candy Hunt Sample", "Assets/Pool Everything/Samples/Candy Hunt/Scenes/Pool Integration.unity",
                    "Candy Hunt is an example of Pool Everything™ being integrated into an existing project. There are few lines of code added to accomplish this showing how to implement Pool Everything™ in your own projects. All objects that were created at runtime was replaced with the object pooling system. There are two scenes with this project that illustrates the differences, without pooling and with pooling.  Follow along on YouTube https://youtu.be/SIf8PXevWm4 as we integrate Pool Everything into an existing project.  Just a few lines of code lets us use an object pool to manage multiple GameObjects.  Pool Everything employs property attributes to make the selection of pooled objects easy with a popup menu.  The API is fully commented with IntelliSense summaries to guide you as you code."),
            };

            ExampleScenesWindow.fullVersionUrl = "http://u3d.as/jJw";
        }

        [MenuItem("Window/Pool Everything/Example Scenes")]
        internal static void Init()
        {
            ExampleScenesWindow.Init(height: 280, width: 280);
        }
    }
}
