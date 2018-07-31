using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//将所有的colider的大小全部改掉
public class ColliderSizeFloatPrecision {

    [MenuItem("Custom/Collider Size Float Precision")]

    static void ChangeSize() {

        List<string> filePaths = new List<string>();

        filePaths = GetFile(Application.dataPath, filePaths);

        bool IsChange = false;

        foreach (string path in filePaths) {

            //             GameObject origin = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            // 
            //             GameObject obj = PrefabUtility.GetPrefabObject(origin) as GameObject;
            //GameObject obj = PrefabUtility.InstantiatePrefab(origin) as GameObject;
            GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            Collider2D[] childs = obj.transform.GetComponentsInChildren<Collider2D>(true);
            //对所有的collider2d的精度进行缩减
            foreach (Collider2D col in childs) {

                col.offset = TransV2(col.offset);

                if (col is BoxCollider2D) {

                    BoxCollider2D boxCollider2D = col as BoxCollider2D;

                    boxCollider2D.size = TransV2(boxCollider2D.size);

                    boxCollider2D.edgeRadius = TransFloat(boxCollider2D.edgeRadius);

                    boxCollider2D.size = TransV2(boxCollider2D.size);

                }
                else if (col is CircleCollider2D) {

                    CircleCollider2D circleCollider2D = col as CircleCollider2D;

                    circleCollider2D.radius = TransFloat(circleCollider2D.radius);

                }
                else if (col is PolygonCollider2D) {

                    PolygonCollider2D polygonCollider2D = col as PolygonCollider2D;

                    for(int i = 0; i < polygonCollider2D.points.Length; i++) {

                        polygonCollider2D.points[i] = TransV2(polygonCollider2D.points[i]);

                    }

                }
                else if (col is CapsuleCollider2D) {

                    CapsuleCollider2D capsuleCollider2D = col as CapsuleCollider2D;

                    capsuleCollider2D.size = TransV2(capsuleCollider2D.size);

                }

                IsChange = true;

            }

            //PrefabUtility.ReplacePrefab(origin, obj, ReplacePrefabOptions.ConnectToPrefab);
            EditorUtility.SetDirty(obj);

        }

        if (IsChange) {

            AssetDatabase.SaveAssets();

        }

        Debug.Log("change Size");

    }

    /// <summary>
    /// 转换float 只保存小数点2位
    /// </summary>
    /// <param name="va"></param>
    /// <returns></returns>
    static float TransFloat(float va) {

        va = ((float)((int)(va * 100))) / 100;

        return va;

    }

    /// <summary>
    /// 转换v2
    /// </summary>
    /// <param name="va"></param>
    /// <returns></returns>
    static Vector2 TransV2(Vector2 va) {

        va.x = TransFloat(va.x);

        va.y = TransFloat(va.y);

        return va;

    }

    static Vector3 TransV3(Vector3 va) {

        va.x = TransFloat(va.x);

        va.y = TransFloat(va.y);

        va.z = TransFloat(va.z);

        return va;

    }

    /// <summary>
    /// 获取路径下所有文件以及子文件夹中文件
    /// </summary>
    /// <param name="path">全路径根目录</param>
    /// <param name="FileList">存放所有文件的全路径</param>
    /// <param name="RelativePath"></param>
    /// <returns></returns>
    public static List<string> GetFile(string path, List<string> FileList) {

        DirectoryInfo dir = new DirectoryInfo(path);

        FileInfo[] fil = dir.GetFiles();

        DirectoryInfo[] dii = dir.GetDirectories();

        foreach (FileInfo f in fil) {

            if (f.Extension == ".prefab") {

                string fullPath = f.FullName.Replace("\\", "/");

                int index = fullPath.IndexOf("Assets/");

                if (index < 0) {

                    FileList.Add(fullPath);//添加文件路径到列表中

                }
                else {

                    FileList.Add(fullPath.Substring(index));//添加文件路径到列表中

                }

            }

        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo d in dii) {

            GetFile(d.FullName, FileList);

        }

        return FileList;

    }

}
