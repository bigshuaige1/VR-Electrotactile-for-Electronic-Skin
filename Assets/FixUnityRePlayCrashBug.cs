using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public static class FixUnityRePlayCrashBug
{
    /// <summary>
    /// 游戏EnteredEditMode之后执行切换场景等待的帧数
    /// </summary>
    private const int WATI_FRAMES = 2;

    /// <summary>
    /// 当前已等待的帧数
    /// </summary>
    private static int currentWaitFrames = 0;

    /// <summary>
    /// 用来临时切换场景用的场景的路径
    /// </summary>
    private const string tempScenePath = "Assets/Scenes/CartPoleTutorial.unity";

    /// <summary>
    /// 游戏启动的场景的路径
    /// </summary>
    private const string launcherScenePath = "Assets/Manus/unity-plugin/Scenes/InteractionScene.unity";

    // register an event handler when the class is initialized
    static FixUnityRePlayCrashBug()
    {
        EditorApplication.playModeStateChanged += OnplayModeStateChanged;
    }

    /// <summary>
    /// playModeStateChanged监听
    /// </summary>
    /// <param name="state"></param>
    private static void OnplayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            currentWaitFrames = 0;
            EditorApplication.update += Update;
        }
    }

    private static void Update()
    {
        currentWaitFrames++;
        if (currentWaitFrames >= WATI_FRAMES)
        {
            ChangeScenes();

            EditorApplication.update -= Update;
        }
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    private static void ChangeScenes()
    {
        EditorSceneManager.OpenScene(tempScenePath);
        EditorSceneManager.OpenScene(launcherScenePath);
        Debug.Log("Rechange To Launcher Scene!");
        currentWaitFrames = 0;
    }
}
