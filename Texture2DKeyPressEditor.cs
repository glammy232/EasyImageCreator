using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[InitializeOnLoad]
public class Texture2DKeyPressEditor : Editor
{
    static Texture2DKeyPressEditor()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
    }

    private static void OnProjectWindowItemGUI(string guid, Rect selectionRect)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.C)
        {
            Object selectedObject = Selection.activeObject;

            if (selectedObject is Texture2D)
            {
                CreateImageFromTexture2D(selectedObject as Texture2D);
            }
        }
    }

    private static void CreateImageFromTexture2D(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogWarning("Selected object is not a Texture2D.");
            return;
        }

        GameObject imageGO = new GameObject("Image");
        Image image = imageGO.AddComponent<Image>();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
        image.SetNativeSize();
        image.preserveAspect = true;

        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null)
        {
            imageGO.transform.parent = GameObject.FindFirstObjectByType<Canvas>().transform;
            imageGO.transform.localScale = new Vector2(1, 1);
            Camera sceneCamera = sceneView.camera;
            Vector3 spawnPosition = sceneCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
            imageGO.transform.position = spawnPosition;
        }

        Undo.RegisterCreatedObjectUndo(imageGO, "Create Image from Texture2D");
        Selection.activeObject = imageGO;
    }
}
