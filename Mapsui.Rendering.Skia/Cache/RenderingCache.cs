﻿using System;
using Mapsui.Styles;

namespace Mapsui.Rendering.Skia.Cache
{
    public class RenderCache : IRenderCache
    {
        private readonly ISymbolCache _symbolCache = new SymbolCache();
        private readonly IVectorCache _vectorCache = new VectorCache();
        private readonly ILabelCache _labelCache = new LabelCache();
        
        public Size? GetSize(int bitmapId)
        {
            return _symbolCache.GetSize(bitmapId);
        }

        public IBitmapInfo GetOrCreate(int bitmapID)
        {
            return _symbolCache.GetOrCreate(bitmapID);
        }

        public T GetOrCreateTypeface<T>(Font font, Func<Font, T> createTypeFace) where T : class
        {
            return _labelCache.GetOrCreateTypeface(font, createTypeFace);
        }

        public T GetOrCreateLabel<T>(string? text, LabelStyle style, float opacity, Func<LabelStyle, string?, float, ILabelCache, T> createLabelAsBitmap) where T : IBitmapInfo
        {
            return _labelCache.GetOrCreateLabel(text, style, opacity, createLabelAsBitmap);
        }

        public T GetOrCreatePaint<T>(Pen? pen, float opacity, Func<Pen?, float, T> toPaint) where T : class
        {
            return _vectorCache.GetOrCreatePaint(pen, opacity, toPaint);
        }

        public T GetOrCreatePaint<T>(Brush? pen, float opacity, double rotation, Func<Brush?, float, double, ISymbolCache, T> toPaint) where T : class
        {
            return _vectorCache.GetOrCreatePaint(pen, opacity, rotation, toPaint);
        }

        public TPath GetOrCreatePath<TPath, TGeometry>(IReadOnlyViewport viewport, TGeometry geometry, float lineWidth, Func<TGeometry, IReadOnlyViewport, float, TPath> toPath) where TPath : class where TGeometry : class
        {
            return _vectorCache.GetOrCreatePath(viewport, geometry, lineWidth);
        }
    }
}