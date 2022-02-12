using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using static UnityEngine.GUILayout;
using UnityEngine.Rendering;
using Steamworks;
using rail;
using xiaoye97;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace DSPCompass
{

    [BepInPlugin("Appun.DSP.plugin.Compass", "DSPCompass", "0.0.1")]
    [BepInProcess("DSPGAME.exe")]

    [HarmonyPatch]
    public class Main : BaseUnityPlugin
    {

        public static GameObject Arrow;

        public static bool arrowEnable = false;

        public void Start()
        {
            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        }


        public void Update()
        {
            if (arrowEnable)
            {
                if (!GameMain.data.mainPlayer.sailing)
                {
                    Arrow.gameObject.SetActive(true);
                    GameObject Player = GameMain.data.mainPlayer.gameObject;
                    Plane plane = new Plane(Player.transform.up, Player.transform.position);
                    var point = new Vector3(0, GameMain.data.localPlanet.realRadius, 0);
                    var planePoint = plane.ClosestPointOnPlane(point);

                    Arrow.transform.localPosition = new Vector3(0, 0.8f, 0);
                    var direction = planePoint - Player.transform.position;
                    Arrow.transform.forward = direction;
                }
                else
                {
                    Arrow.gameObject.SetActive(false);
                }
            }
        }

        public class CreateTriangleMesh : MonoBehaviour
        {

            void Start()
            {
                var mesh = new Mesh();

                var Vertices = new List<Vector3> {
                      new Vector3 (-0.5f,0, 4.3f),
                      new Vector3 (0, 0, 5.3f),
                      new Vector3 (0.5f, 0, 4.3f),
                      new Vector3 (-0.4f, 0, 5.5f),
                      new Vector3 (-0.4f, 0, 6.3f),
                      new Vector3 (-0.2f, 0, 5.5f),
                      new Vector3 (-0.2f, 0, 6.3f),
                      new Vector3 (0.4f, 0, 5.5f),
                      new Vector3 (0.4f, 0, 6.3f),
                      new Vector3 (0.2f, 0, 5.5f),
                      new Vector3 (0.2f, 0, 6.3f),
                       new Vector3 (0, 0, 4.6f),
                      new Vector3 (-0.2f, 0, 6),
                      new Vector3 (0.2f, 0, 5.8f),
                      new Vector3 (0.2f, 0, 6),
                      new Vector3 (-0.2f, 0, 5.8f),
               };
                mesh.SetVertices(Vertices);
                var triangles = new List<int> { 0, 1, 11, 11, 1, 0, 2, 1, 11, 11, 1, 2, 3, 4, 5, 5, 4, 3, 4, 6, 5, 5, 6, 4, 7, 9, 8, 8, 9, 7, 8, 9, 10, 10, 9, 8, 6, 9, 12, 6, 13, 9, 10, 15, 14, 14, 15, 5 };
                mesh.SetTriangles(triangles, 0);

                var meshFilter = GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                var renderer = GetComponent<MeshRenderer>();
                renderer.material.color = Color.red;
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.red);
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

        }

        [HarmonyPostfix, HarmonyPatch(typeof(Player), "SetAfterGameDataReady")]
        public static void Player_SetAfterGameDataReady_Patch()

        {
            GameObject Player = GameMain.data.mainPlayer.gameObject;

            Arrow = new GameObject("Arrow");
            Arrow.transform.parent = Player.transform;
            Arrow.AddComponent<CreateTriangleMesh>();
            Arrow.AddComponent<MeshRenderer>();
            Arrow.AddComponent<MeshFilter>();

            Arrow.transform.localPosition = new Vector3(0, 0, 0);
            Arrow.transform.localScale = new Vector3(1, 1, 1);

            arrowEnable = true;

        }

    }


    public class LogManager
    {
        public static ManualLogSource Logger;
    }

}