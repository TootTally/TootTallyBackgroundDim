using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TootTallyBackgroundDim
{
    public class BackgroundDimController : MonoBehaviour
    {
        private static readonly Vector2 DEFAULT_RESOLUTION = new Vector2(1920, 1080);

        private static GameObject _backgroundDimGameObject;
        private static Image _backgroundDimImage;
        private static CanvasScaler _backgroundDimCanvasScaler;
        private static float _alpha;
        private static GameController _currentGCInstance;


        [HarmonyPatch(typeof(GameController), nameof(GameController.Start))]
        [HarmonyPostfix]

        public static void BackgroundDim(GameController __instance)
        {
            GameObject bgGameObject = GameObject.Find("BGCameraObj");
            _alpha = Plugin.Instance.DimAmount.Value;

            _backgroundDimGameObject = Instantiate(new GameObject(), bgGameObject.transform);

            _backgroundDimGameObject.transform.position = new Vector3(0, 0, 100);

            _backgroundDimImage = _backgroundDimGameObject.AddComponent<Image>();
            _backgroundDimImage.color = new Color(0, 0, 0, _alpha);

            _backgroundDimCanvasScaler = _backgroundDimGameObject.AddComponent<CanvasScaler>();
            _backgroundDimCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _backgroundDimCanvasScaler.referenceResolution = DEFAULT_RESOLUTION;

            RectTransform bgDimRectTransform = _backgroundDimGameObject.GetComponent<RectTransform>();
            bgDimRectTransform.sizeDelta = DEFAULT_RESOLUTION;

            __instance.topbreath.transform.parent.parent.gameObject.SetActive(false);
            _currentGCInstance = __instance;
        }

        [HarmonyPatch(typeof(BGController), nameof(BGController.setUpBGControllerRefsDelayed))]
        [HarmonyPostfix]
        public static void OnLoadAssetBundleActivateBreathMeterCanvas()
        {
            _currentGCInstance.topbreath.transform.parent.parent.gameObject.SetActive(true);
        }
    }
}
