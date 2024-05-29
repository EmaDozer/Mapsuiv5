﻿using Mapsui.Widgets;
using System;

namespace Mapsui.Styles;

/// <summary>
/// Type of CalloutStyle
/// </summary>
public enum CalloutType
{
    /// <summary>
    /// Only one line is shown
    /// </summary>
    Single,
    /// <summary>
    /// Header and detail is shown
    /// </summary>
    Detail,
    /// <summary>
    /// Content is custom, the bitmap given in Content is shown
    /// </summary>
    Image,
}

/// <summary>
/// Determines, where the pointer is
/// </summary>
public enum TailAlignment
{
    /// <summary>
    /// Callout tail is at bottom side of bubble
    /// </summary>
    Bottom,
    /// <summary>
    /// Callout tail is at left side of bubble
    /// </summary>
    Left,
    /// <summary>
    /// Callout tail is at top side of bubble
    /// </summary>
    Top,
    /// <summary>
    /// Callout tail is at right side of bubble
    /// </summary>
    Right,
}

/// <summary>
/// A CalloutStyle shows a callout or InfoWindow in Google Maps
/// </summary>
/// <remarks>
/// There are three different types of Callouts
/// 1. Type = CalloutType.Single
///    The text in Title will be shown
/// 2. Type = CalloutType.Detail
///    The text in Title and SubTitle will be shown
/// 3. Type = CalloutType.Custom
///    The bitmap with ID in Content will be shown
/// </remarks>
public class CalloutStyle : SymbolStyle
{
    private CalloutType _type = CalloutType.Single;
    private TailAlignment _arrowAlignment = TailAlignment.Bottom;
    private float _arrowWidth = 8f;
    private float _arrowHeight = 8f;
    private float _arrowPosition = 0.5f;
    private float _rectRadius = 4f;
    private float _shadowWidth = 2f;
    private MRect _padding = new(3f, 3f, 3f, 3f);
    private Color _color = Color.Black;
    private Color _backgroundColor = Color.White;
    private float _strokeWidth = 1f;
    private Offset _offset = new(0, 0);
    private double _rotation;
    private string? _title;
    private string? _subtitle;
    private Alignment _titleTextAlignment;
    private Alignment _subtitleTextAlignment;
    private double _spacing;
    private double _maxWidth;
    private Color? _titleFontColor;
    private Color? _subtitleFontColor;

    public string ImageIdOfCallout { get; private set; } = Guid.NewGuid().ToString();
    public string ImageIdOfCalloutContent { get; private set; } = Guid.NewGuid().ToString();

    public static new double DefaultWidth { get; set; } = 100;
    public static new double DefaultHeight { get; set; } = 30;

    /// <summary>
    /// Type of Callout
    /// </summary>
    /// <remarks>
    /// Could be Single, Detail or Image.
    /// </remarks>
    public CalloutType Type
    {
        get => _type;
        set
        {
            if (_type != value)
            {
                _type = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Offset position in pixels of Callout
    /// </summary>
    public Offset Offset
    {
        get => _offset;
        set
        {
            if (!_offset.Equals(value))
            {
                _offset = value;
                Invalidate();
                //SymbolOffset = new Offset(_offset.X, _offset.Y);
            }
        }
    }

    /// <summary>
    /// Gets or sets the rotation of the Callout in degrees (clockwise is positive)
    /// </summary>
    public double Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                SymbolRotation = _rotation;
            }
        }
    }

    /// <summary>
    /// Anchor position of Callout
    /// </summary>
    public TailAlignment ArrowAlignment
    {
        get => _arrowAlignment;
        set
        {
            if (value != _arrowAlignment)
            {
                _arrowAlignment = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Width of opening of anchor of Callout
    /// </summary>
    public float ArrowWidth
    {
        get => _arrowWidth;
        set
        {
            if (value != _arrowWidth)
            {
                _arrowWidth = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Height of anchor of Callout
    /// </summary>
    public float ArrowHeight
    {
        get => _arrowHeight;
        set
        {
            if (value != _arrowHeight)
            {
                _arrowHeight = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Relative position of anchor of Callout on the side given by AnchorType
    /// </summary>
    public float ArrowPosition
    {
        get => _arrowPosition;
        set
        {
            if (value != _arrowPosition)
            {
                _arrowPosition = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Color of stroke around Callout
    /// </summary>
    public Color Color
    {
        get => _color;
        set
        {
            if (value != _color)
            {
                _color = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// BackgroundColor of Callout
    /// </summary>
    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            if (value != _backgroundColor)
            {
                _backgroundColor = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Stroke width of frame around Callout
    /// </summary>
    public float StrokeWidth
    {
        get => _strokeWidth;
        set
        {
            if (value != _strokeWidth)
            {
                _strokeWidth = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Radius of rounded corners of Callout
    /// </summary>
    public float RectRadius
    {
        get => _rectRadius;
        set
        {
            if (value != _rectRadius)
            {
                _rectRadius = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Padding around content of Callout
    /// </summary>
    public MRect Padding
    {
        get => _padding;
        set
        {
            if (!value.Equals(_padding))
            {
                _padding = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Width of shadow around Callout
    /// </summary>
    public float ShadowWidth
    {
        get => _shadowWidth;
        set
        {
            if (value != _shadowWidth)
            {
                _shadowWidth = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Content of Callout title label
    /// </summary>
    public string? Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Font color to render title
    /// </summary>
    public Color? TitleFontColor
    {
        get => _titleFontColor;
        set
        {
            _titleFontColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Text alignment of title
    /// </summary>
    public Alignment TitleTextAlignment
    {
        get => _titleTextAlignment;
        set
        {
            if (_titleTextAlignment != value)
            {
                _titleTextAlignment = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Content of Callout subtitle label
    /// </summary>
    public string? Subtitle
    {
        get => _subtitle;
        set
        {
            if (_subtitle != value)
            {
                _subtitle = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Font color to render subtitle
    /// </summary>
    public Color? SubtitleFontColor
    {
        get => _subtitleFontColor;
        set
        {
            _subtitleFontColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Text alignment of subtitle
    /// </summary>
    public Alignment SubtitleTextAlignment
    {
        get => _subtitleTextAlignment;
        set
        {
            if (_subtitleTextAlignment != value)
            {
                _subtitleTextAlignment = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Space between Title and Subtitle of Callout
    /// </summary>
    public double Spacing
    {
        get => _spacing;
        set
        {
            if (_spacing != value)
            {
                _spacing = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// MaxWidth for Title and Subtitle of Callout
    /// </summary>
    public double MaxWidth
    {
        get => _maxWidth;
        set
        {
            if (_maxWidth != value)
            {
                _maxWidth = value;
                Invalidate();
            }
        }
    }

    private Font _titleFont = new();
    private Font _subtitleFont = new();

    public Font TitleFont
    {
        get => _titleFont;
        set
        {
            _titleFont = value;
            Invalidate();
        }
    }

    public Font SubtitleFont
    {
        get => _subtitleFont;
        set
        {
            _subtitleFont = value;
            Invalidate();
        }
    }

    public void Invalidate()
    {
        ImageIdOfCalloutContent = Guid.NewGuid().ToString();
        ImageIdOfCallout = Guid.NewGuid().ToString();
    }
}
