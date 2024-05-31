using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using _3._Scripts.UI.Panels;
using DG.Tweening;
using UnityEngine;


namespace _3._Scripts.UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private UIScreen currentScreen;
        [Header("Screens")] [SerializeField] private List<UIScreen> screens = new();
        private UIWidget[] widgets = Array.Empty<UIWidget>();

        public UIScreen CurrentScreen => currentScreen;
        private bool onTransition;
        public bool Active { get; set; }
        protected override void OnAwake()
        {
            base.OnAwake();
            if(Instance != null && Instance != this)
                Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
            
            InitializeScreens();
            InitializeWidgets();
            currentScreen.Open();
        }

        private void Start()
        {
            GetPanel<ButtonsPanel>().Enabled = true;
        }

        public void SetScreen(string id, TweenCallback onCloseComplete = null, TweenCallback onOpenComplete = null)
        {
            if (currentScreen.ID == id) return;
            if (onTransition) return;

            var screen = screens.FirstOrDefault(s => s.ID == id);
            if (screen == null) return;

            onTransition = true;
            StartCoroutine(ChangeScreen(screen, onCloseComplete, onOpenComplete));
        }

        public T GetWidget<T>(int siblingIndex = -1) where T : UIWidget
        {
            var widget = (T)widgets.FirstOrDefault(w => w is T);
            if (widget == null) return default;

            widget.SetScreen(currentScreen, siblingIndex);

            return widget;
        }

        public T GetPanel<T>() where T : UIPanel
        {
            var panel = currentScreen.GetPanel<T>();
            if (panel == null)
                throw new InvalidCastException($"{currentScreen.ID} screen dont have {typeof(T)}");

            return panel;
        }

        private void InitializeScreens()
        {
            if (!screens.Contains(currentScreen)) screens.Add(currentScreen);
            foreach (var screen in screens)
            {
                screen.Initialize();
            }
        }

        private void InitializeWidgets()
        {
            widgets = GetComponentsInChildren<UIWidget>();
            foreach (var widget in widgets)
            {
                widget.Initialize();
                widget.ForceClose();
            }
        }

        private void MoveWidgetsToScreen(UIScreen screen)
        {
            foreach (var widget in widgets.Where(w => w.Enabled))
            {
                widget.SetScreen(screen);
                widget.transform.localScale = Vector3.one;
            }
        }


        private IEnumerator ChangeScreen(UIScreen screen, TweenCallback onCloseComplete = null,
            TweenCallback onOpenComplete = null)
        {
            yield return new WaitUntil(() => currentScreen.Opened);
            currentScreen.Close(onComplete: () =>
            {
                currentScreen = screen;
                MoveWidgetsToScreen(screen);
                currentScreen.Open(onComplete: () =>
                {
                    onTransition = false;
                    onOpenComplete?.Invoke();
                });
                onCloseComplete?.Invoke();
            });
        }
    }
}