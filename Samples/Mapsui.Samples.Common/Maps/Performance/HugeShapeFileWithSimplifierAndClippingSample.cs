﻿using Mapsui.Layers;
using Mapsui.Nts.Providers;
using Mapsui.Samples.Common.Utilities;
using Mapsui.Styles;
using Mapsui.Styles.Thematics;
using Mapsui.Tiling;
using Mapsui.Tiling.Layers;
using System.IO;
using System.Threading.Tasks;

namespace Mapsui.Samples.Common.Maps.Performance;

public class HugeShapeFileWithSimplifierAndClippingSample : ISample
{
    private static TileLayer TileLayer = OpenStreetMap.CreateTileLayer();
    private static ILayer ShapeLayer1 = CreateShapeLayer("EZG_KB_LM.shp", "cache1");
    private static ILayer ShapeLayer2 = CreateShapeLayer("modell_ezgs_v02_ohneTalsperren_EPSG3857.shp", "cache2");

    public HugeShapeFileWithSimplifierAndClippingSample()
    {
        ShapeFilesDeployer.CopyEmbeddedResourceToFile("EZG_KB_LM.shp");
        ShapeFilesDeployer.CopyEmbeddedResourceToFile("modell_ezgs_v02_ohneTalsperren_EPSG3857.shp");
    }

    public string Name => "Huge Shape File With Simplifier and Clipping";
    public string Category => "Performance";

    public static Map CreateMap()
    {
        var map = new Map();

        map.Layers.Add(TileLayer);
        map.Layers.Add(ShapeLayer1);
        map.Layers.Add(ShapeLayer2);

        return map;
    }

    public Task<Map> CreateMapAsync() => Task.FromResult(CreateMap());

    private static ILayer CreateShapeLayer(string shapeName, string cacheName)
    {
        using var shapeFile = new Mapsui.Nts.Providers.Shapefile.ShapeFile(
           Path.Combine(ShapeFilesDeployer.ShapeFilesLocation, shapeName), false)
        { CRS = "EPSG:3857" };

        //option 1: with clipping
        var provider = new GeometrySimplifyAndClippingProvider(shapeFile);

        //option 2: without clipping
        //var provider = new GeometrySimplifyProvider(shapeFile);

        //var sqlitePersistentCache = new SqlitePersistentCache(cacheName);
        //sqlitePersistentCache.Clear();

        using var layer = new Layer
        {
            Name = shapeName,
            DataSource = provider,
            Style = CreateVectorThemeStyle(),
        };

        //return layer;
        return new RasterizingLayer(layer);
        //return new RasterizingTileLayer(layer) { Enabled = false }; //really slow
        //return new RasterizingTileLayer(layer, persistentCache: sqlitePersistentCache) { Enabled = false }; //really slow
    }

    private static IThemeStyle CreateVectorThemeStyle()
    {
        var style = new VectorStyle()
        {
            Fill = new Brush(Color.Transparent),
            Line = new Pen
            {
                Color = Color.Black,
                Width = 2
            },
            //Opacity = vectorTheme.Opacity,
            Outline = new Pen
            {
                Color = Color.Black,
                Width = 2
            }
        };

        return new ThemeStyle(f =>
        {
            return style;
        });
    }
}
