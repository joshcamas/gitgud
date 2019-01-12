using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GitGud.UI
{
    public class GitAssetModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        static AssetMoveResult OnWillMoveAsset(string path)
        {
            OnAssetModify();

            return AssetMoveResult.DidNotMove;
        }

        static AssetDeleteResult OnWillDeleteAsset(string path)
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
            GitGudWindow.PlanRefresh();
        }
    }
}
