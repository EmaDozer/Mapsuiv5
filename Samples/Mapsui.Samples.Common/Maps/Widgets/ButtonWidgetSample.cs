﻿using Mapsui.Extensions;
using Mapsui.Logging;
using Mapsui.Manipulations;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.Utilities;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidgets;
using System;
using System.Threading.Tasks;

namespace Mapsui.Samples.Common.Maps.Widgets;

public class ButtonWidgetSample : ISample
{
    private int _tapCount;
    private int _doubleTapCount;
    private int _imageTapCount;

    public string Name => "Button";
    public string Category => "Widgets";

    public Task<Map> CreateMapAsync()
    {
        var map = new Map();

        map.Layers.Add(OpenStreetMap.CreateTileLayer());

        map.Widgets.Add(CreateButtonWidget("Tap me", VerticalAlignment.Top, HorizontalAlignment.Left, (s, e) =>
        {
            if (e.GestureType == GestureType.DoubleTap)
                return;
            var button = Caster.TryCastOrThrow<ButtonWidget>(s);
            button.Text = $"Tapped {++_tapCount} times";
            map.RefreshGraphics();
        }));
        map.Widgets.Add(CreateImageButtonWidget(VerticalAlignment.Top, HorizontalAlignment.Right, (s, e) =>
        {
            Logger.Log(LogLevel.Information, $"Image Tapped {++_imageTapCount} times");
            map.RefreshGraphics();
        }));
        map.Widgets.Add(CreateButtonWidget("Hello!", VerticalAlignment.Bottom, HorizontalAlignment.Right, (s, e) =>
        {
            var button = Caster.TryCastOrThrow<ButtonWidget>(s);
            button.Text = $"{button.Text}!";
            map.RefreshGraphics();
        }));
        map.Widgets.Add(CreateButtonWidget("Double Tap me", VerticalAlignment.Bottom, HorizontalAlignment.Left, (s, e) =>
        {
            if (e.GestureType == GestureType.SingleTap)
                return;
            var button = Caster.TryCastOrThrow<ButtonWidget>(s);
            button.Text = $"Double Tapped {++_doubleTapCount} times";
            map.RefreshGraphics();
        }));

        return Task.FromResult(map);
    }

    private static ButtonWidget CreateButtonWidget(string text, VerticalAlignment verticalAlignment,
        HorizontalAlignment horizontalAlignment, EventHandler<WidgetEventArgs> tapped) => new()
        {
            Text = text,
            VerticalAlignment = verticalAlignment,
            HorizontalAlignment = horizontalAlignment,
            Margin = new MRect(30),
            Padding = new MRect(10, 8),
            CornerRadius = 8,
            BackColor = new Color(0, 123, 255),
            TextColor = Color.White,
            WithTappedEvent = tapped
        };

    private static ImageButtonWidget CreateImageButtonWidget(VerticalAlignment verticalAlignment,
        HorizontalAlignment horizontalAlignment, EventHandler<WidgetEventArgs> tapped) => new()
        {
            Image = "embedded://Mapsui.Resources.Images.MyLocationStill.svg",
            VerticalAlignment = verticalAlignment,
            HorizontalAlignment = horizontalAlignment,
            Margin = new MRect(30),
            Padding = new MRect(10, 8),
            CornerRadius = 8,
            Envelope = new MRect(0, 0, 64, 64),
            WithTappedEvent = tapped
        };
}
