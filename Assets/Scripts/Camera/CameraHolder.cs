using Events;
using Player;
using UnityEngine;
using Utils.Scenes;
using Utils.Singleton;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private float moveVelocity;
        
        public UnityEngine.Camera MainCamera => mainCamera;
        
        private Vector3 _startPoint;
        private Vector3 _initialPosition;
        
        private void Start()
        {
            _initialPosition = transform.position;
            
            EventsController.Subscribe<EventModels.Game.EmptySpacesTapped>(this, OnEmptySpaceTapped);
            ScenesChanger.SceneLoadedEvent += OnSceneChanged;
        }

        private void OnDestroy()
        {
            EventsController.Unsubscribe<EventModels.Game.EmptySpacesTapped>(OnEmptySpaceTapped);
            ScenesChanger.SceneLoadedEvent -= OnSceneChanged;
        }

        private void Update()
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;
            
            UpdatePosition();
        }

        private void OnEmptySpaceTapped(EventModels.Game.EmptySpacesTapped e)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;
            
            _startPoint = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        
        private void UpdatePosition()
        {
            var currentPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var deltaPosition = -(currentPosition - _startPoint) * (moveVelocity * Time.deltaTime);

            MainCamera.transform.position += deltaPosition;
        }
        
        private void OnSceneChanged()
        {
            transform.position = _initialPosition;
        }
    }
}