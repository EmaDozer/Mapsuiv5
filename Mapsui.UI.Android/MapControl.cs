using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Java.Lang;
using Mapsui.Fetcher;
using Mapsui.Utilities;
using SkiaSharp;
using SkiaSharp.Views.Android;
using Math = System.Math;

namespace Mapsui.UI.Android
{
    public class MapControl : SKCanvasView, IMapControl
    {
        private const int None = 0;
        private const int Dragging = 1;
        private const int Zoom = 2;
        private int _mode = None;
        private PointF _previousMap, _currentMap;
        private PointF _previousMid = new PointF();
        private readonly PointF _currentMid = new PointF();
        private float _oldDist = 1f;
        private bool _viewportInitialized;
        private Rendering.Skia.MapRenderer _renderer;
        private Map _map;
        private bool _layersInitialized;

        public event EventHandler ViewportInitialized;
        public event EventHandler<MouseInfoEventArgs> Info;

        public MapControl(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            //var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.start_with_openstreetmap_style);
            //var startWithOpenStreetMap = a.GetBoolean(Resource.Attribute.start_with_openstreetmap, false);
            
            Initialize();
        }

        public MapControl(Context context, IAttributeSet attrs, int defStyle):
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public void Initialize()
        {
            Map = new Map();
            if (StartWithOpenStreetMap) Map.Layers.Add(OpenStreetMap.CreateTileLayer());

            _renderer = new Rendering.Skia.MapRenderer();
            InitializeViewport();
            Touch += MapView_Touch;
        }

        private void InitializeViewport()
        {
            if (ViewportHelper.TryInitializeViewport(_map, Width, Height))
            {
                _viewportInitialized = true;
                Map.ViewChanged(true);
                OnViewportInitialized();
            }
        }

        private void OnViewportInitialized()
        {
            ViewportInitialized?.Invoke(this, EventArgs.Empty);
        }

        public void MapView_Touch(object sender, TouchEventArgs args)
        {
            var x = (int)args.Event.RawX;
            var y = (int)args.Event.RawY;
            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    _previousMap = null;
                    _mode = Dragging;
                    break;
                case MotionEventActions.Up:
                    _previousMap = null;
                    Invalidate();
                    _mode = None;
                    _map.ViewChanged(true);
                    HandleInfo(GetPosition(args.Event));
                    break;
                case MotionEventActions.Pointer2Down:
                    _previousMap = null;
                    _oldDist = Spacing(args.Event);
                    MidPoint(_currentMid, args.Event);
                    _previousMid = _currentMid;
                    _mode = Zoom;
                    break;
                case MotionEventActions.Pointer2Up:
                    _previousMap = null;
                    _previousMid = null;
                    _mode = Dragging;
                    break;
                case MotionEventActions.Move:
                    switch (_mode)
                    {
                        case Dragging:
                            _currentMap = new PointF(x, y);
                            if (_previousMap != null)
                            {
                                _map.Viewport.Transform(
                                    _currentMap.X,
                                    _currentMap.Y,
                                    _previousMap.X,
                                    _previousMap.Y);
                                Invalidate();
                            }
                            _previousMap = _currentMap;
                            break;
                        case Zoom:
                            {
                                if (args.Event.PointerCount < 2)
                                    return;

                                var newDist = Spacing(args.Event);
                                var scale = newDist / _oldDist;

                                _oldDist = Spacing(args.Event);
                                _previousMid = new PointF(_currentMid.X, _currentMid.Y);
                                MidPoint(_currentMid, args.Event);
                                _map.Viewport.Center = _map.Viewport.ScreenToWorld(
                                    _currentMid.X,
                                    _currentMid.Y);
                                _map.Viewport.Resolution = _map.Viewport.Resolution / scale;
                                _map.Viewport.Center = _map.Viewport.ScreenToWorld(
                                    (_map.Viewport.Width - _currentMid.X),
                                    (_map.Viewport.Height - _currentMid.Y));
                                _map.Viewport.Transform(
                                    _currentMid.X,
                                    _currentMid.Y,
                                    _previousMid.X,
                                    _previousMid.Y);
                                Invalidate();
                            }
                            break;
                    }
                    break;
            }
        }

        private void HandleInfo(PointF screenPosition)
        {
            if (Info == null) return;
            var args = InfoHelper.GetInfoEventArgs(Map, screenPosition.ToMapsui(), Map.InfoLayers);
            if (args != null) Info?.Invoke(this, args);
        }

        private static float Spacing(MotionEvent me)
        {
            if (me.PointerCount < 2)
                throw new ArgumentException();

            var x = me.GetX(0) - me.GetX(1);
            var y = me.GetY(0) - me.GetY(1);
            return (float)Math.Sqrt(x * x + y * y);
        }

        private static void MidPoint(PointF point, MotionEvent motionEvent)
        {
            var position = GetPosition2(motionEvent);
            point.Set(position.X / 2, position.Y / 2);
        }
        
        private static PointF GetPosition2(MotionEvent motionEvent)
        {
            return new PointF(motionEvent.GetX(0) + motionEvent.GetX(1), motionEvent.GetY(0) + motionEvent.GetY(1));
        }

        private static PointF GetPosition(MotionEvent motionEvent)
        {
            return new PointF(motionEvent.GetX(0) , motionEvent.GetY(0));
        }

        public Map Map
        {
            get
            {
                return _map;
            }
            set
            {
                if (_map != null)
                {
                    var temp = _map;
                    _map = null;
                    temp.DataChanged -= MapDataChanged;
                    temp.PropertyChanged -= MapPropertyChanged;
                    temp.RefreshGraphics -= MapRefreshGraphics;
                    temp.Dispose();
                }

                _map = value;

                if (_map != null)
                {
                    _map.DataChanged += MapDataChanged;
                    _map.PropertyChanged += MapPropertyChanged;
                    _map.RefreshGraphics += MapRefreshGraphics;
                    _map.ViewChanged(true);
                }

                RefreshGraphics();
            }
        }

        private void MapRefreshGraphics(object sender, EventArgs eventArgs)
        {
            ((Activity)Context).RunOnUiThread(new Runnable(RefreshGraphics));
        }

        private void MapPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Envelope") return;
            InitializeViewport();
            _map.ViewChanged(true);
        }

        public void MapDataChanged(object sender, DataChangedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                // todo: test code below:
                // ((Activity)Context).RunOnUiThread(new Runnable(Toast.MakeText(Context, GetErrorMessage(e), ToastLength.Short).Show));
            }
            else // no problems
            {
                ((Activity)Context).RunOnUiThread(new Runnable(RefreshGraphics));
            }
        }

        public void RefreshGraphics()
        {
            PostInvalidate();
        }

        protected override void OnDraw(SKSurface surface, SKImageInfo info)
        {
            if (!_layersInitialized && StartWithOpenStreetMap)
            {
                Map.Layers.Add(OpenStreetMap.CreateTileLayer());
                _layersInitialized = true;
            }

            base.OnDraw(surface, info);

            if (!_viewportInitialized)
                InitializeViewport();
            if (!_viewportInitialized)
                return;

            _renderer.Render(surface.Canvas, _map.Viewport, _map.Layers, _map.BackColor);
        }
        

        public bool StartWithOpenStreetMap { get; set; }
    }

}