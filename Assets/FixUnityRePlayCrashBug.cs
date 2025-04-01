using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public static class FixUnityRePlayCrashBug
{
    /// <summary>
    /// ��ϷEnteredEditMode֮��ִ���л������ȴ���֡��
    /// </summary>
    private const int WATI_FRAMES = 2;

    /// <summary>
    /// ��ǰ�ѵȴ���֡��
    /// </summary>
    private static int currentWaitFrames = 0;

    /// <summary>
    /// ������ʱ�л������õĳ�����·��
    /// </summary>
    private const string tempScenePath = "Assets/Scenes/CartPoleTutorial.unity";

    /// <summary>
    /// ��Ϸ�����ĳ�����·��
    /// </summary>
    private const string launcherScenePath = "Assets/Manus/unity-plugin/Scenes/InteractionScene.unity";

    // register an event handler when the class is initialized
    static FixUnityRePlayCrashBug()
    {
        EditorApplication.playModeStateChanged += OnplayModeStateChanged;
    }

    /// <summary>
    /// playModeStateChanged����
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
    /// �л�����
    /// </summary>
    private static void ChangeScenes()
    {
        EditorSceneManager.OpenScene(tempScenePath);
        EditorSceneManager.OpenScene(launcherScenePath);
        Debug.Log("Rechange To Launcher Scene!");
        currentWaitFrames = 0;
    }
}
