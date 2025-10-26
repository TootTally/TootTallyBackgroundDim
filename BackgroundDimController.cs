using HarmonyLib;
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
            if (bgGameObject == null) { return; }
            _alpha = Plugin.Instance.DimAmount.Value;

            _backgroundDimGameObject = new GameObject("BGDimObject");
            _backgroundDimGameObject.transform.SetParent(bgGameObject.transform);
            _backgroundDimGameObject.transform.position = new Vector3(0, 0, 100);
            _backgroundDimGameObject.transform.localScale = new Vector3(2f, 2f, 2f);

            _backgroundDimImage = _backgroundDimGameObject.AddComponent<Image>();
            _backgroundDimImage.color = new Color(0, 0, 0, _alpha);

            _backgroundDimCanvasScaler = _backgroundDimGameObject.AddComponent<CanvasScaler>();
            _backgroundDimCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _backgroundDimCanvasScaler.referenceResolution = DEFAULT_RESOLUTION;

            _backgroundDimGameObject.GetComponent<Canvas>().sortingOrder = -3;

            RectTransform bgDimRectTransform = _backgroundDimGameObject.GetComponent<RectTransform>();
            bgDimRectTransform.sizeDelta = DEFAULT_RESOLUTION;

            __instance.topbreath.transform.parent.parent.gameObject.SetActive(false);
            _currentGCInstance = __instance;
        }

        [HarmonyPatch(typeof(BGController), nameof(BGController.setUpBGControllerRefsDelayed))]
        [HarmonyPostfix]
        public static void OnLoadAssetBundleActivateBreathMeterCanvas(BGController __instance)
        {
            if (__instance == null || _currentGCInstance == null) return;
            if (OptionalGameplayUIReducer.ShouldHideBreathMeter()) return;
            _currentGCInstance.topbreath.transform.parent.parent.gameObject.SetActive(true);
        }
    }
}
