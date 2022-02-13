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

    [BepInPlugin("Appun.DSP.plugin.Compass", "DSPCompass", "0.0.3")]
    [BepInProcess("DSPGAME.exe")]

    [HarmonyPatch]
    public class Main : BaseUnityPlugin
    {

        public static GameObject ArrowRed;
        public static GameObject ArrowBlue;
        public static GameObject ArrowBase;

        public static bool arrowEnable = false;

        public void Start()
        {
            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

        }


        public void Update()
        {

            //LogManager.Logger.LogInfo($"{GameMain.isNull}:{GameMain.isPaused}:{GameMain.isLoading}:{GameMain.isRunning}:{GameMain.isRunning}:{DSPGame.Game.isMenuDemo}");
            if (DSPGame.Game == null)
            {
                return;
            }
            if (!DSPGame.Game.isMenuDemo && GameMain.isRunning)
            {

                if (arrowEnable)
                {
                    if (GameMain.localPlanet != null && !GameMain.data.mainPlayer.sailing)
                    {
                        GameObject Player = GameMain.data.mainPlayer.gameObject;
                        if (Player.activeSelf)
                        {
                            Plane plane = new Plane(Player.transform.up, Player.transform.position);
                            var point = new Vector3(0, GameMain.data.localPlanet.realRadius, 0);
                            var planePoint = plane.ClosestPointOnPlane(point);

                            ArrowBase.transform.localPosition = new Vector3(0, 0.8f, 0);
                            var direction = planePoint - Player.transform.position;
                            ArrowBase.transform.forward = direction;
                            ArrowBase.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        ArrowBase.gameObject.SetActive(false);
                    }
                }else

                {
                    LogManager.Logger.LogInfo("---------------------------------------------------------Arrow created");

                    GameObject Player = GameMain.data.mainPlayer.gameObject;

                    ArrowBase = new GameObject("ArrowBase");
                    ArrowBase.transform.parent = Player.transform;
                    ArrowBase.transform.localPosition = new Vector3(0, 0, 0);
                    ArrowBase.transform.localScale = new Vector3(1, 1, 1);

                    ArrowRed = new GameObject("ArrowRed");
                    ArrowRed.transform.parent = ArrowBase.transform;
                    ArrowRed.AddComponent<CreateTriangleMeshRed>();
                    ArrowRed.AddComponent<MeshRenderer>();
                    ArrowRed.AddComponent<MeshFilter>();
                    ArrowRed.transform.localPosition = new Vector3(0, 0, 0);
                    ArrowRed.SetActive(true);

                    ArrowBlue = new GameObject("ArrowBlue");
                    ArrowBlue.transform.parent = ArrowBase.transform;
                    ArrowBlue.AddComponent<CreateTriangleMeshBlue>();
                    ArrowBlue.AddComponent<MeshRenderer>();
                    ArrowBlue.AddComponent<MeshFilter>();
                    ArrowBlue.SetActive(true);
                    ArrowBlue.transform.localPosition = new Vector3(0, 0, 0);
                    ArrowBlue.transform.localRotation = new Quaternion(0, 0, 180, 0);

                    arrowEnable = true;
                }
            }
        }

        public class CreateTriangleMeshRed : MonoBehaviour
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
               };
                mesh.SetVertices(Vertices);
                var triangles = new List<int> { 0, 1, 11, 11, 1, 2, 3, 4, 5, 4, 6, 5, 7, 9, 8, 8, 9, 10, 6, 9, 12, 6, 13, 9 };
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

        public class CreateTriangleMeshBlue : MonoBehaviour
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
               };
                mesh.SetVertices(Vertices);
                var triangles = new List<int> { 0, 1, 11, 11, 1, 2, 3, 4, 5, 4, 6, 5, 7, 9, 8, 8, 9, 10, 6, 9, 12, 6, 13, 9 };
                mesh.SetTriangles(triangles, 0);

                var meshFilter = GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;

                var renderer = GetComponent<MeshRenderer>();
                renderer.material.color = Color.blue; // new Color(1, 0.7f, 0, 1);
                renderer.material.EnableKeyword("_EMISSION");
                renderer.material.SetColor("_EmissionColor", Color.blue);// new Color(1, 0.7f, 0, 1));
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

        }

    }














    public class LogManager
    {
        public static ManualLogSource Logger;
    }

}