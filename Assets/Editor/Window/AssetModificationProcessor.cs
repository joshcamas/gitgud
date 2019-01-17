using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GitGud.UI
{
    public class GitAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            OnAssetModify();

            return AssetMoveResult.DidNotMove;
        }

        static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            OnAssetModify();

            return AssetDeleteResult.DidNotDelete;
        }

        static void OnWillCreateAsset(string path)
        {
            OnAssetModify();
        }

        static string[] OnWillSaveAssets(string[] paths)
        {
            if (paths.Length == 0)
                return paths;

            OnAssetModify();

            return paths;
        }

        static void OnAssetModify()
        {
            if (GitGudSettings.GetBool("refresh_on_edit") == false)
                return;

            //Wait for one frame (ugly)
            EditorApplication.update -= FrameDelay;
            EditorApplication.update += FrameDelay;
        }

        static void FrameDelay()
        {
            EditorApplication.update -= FrameDelay;
            GitEvents.TriggerOnLocalChange();
        }
    }
}
