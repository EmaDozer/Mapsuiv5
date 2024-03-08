﻿using System;

namespace Mapsui.Manipulations;

public class TapGestureTracker
{
    private readonly double _maxTapDuration = 0.5;
    private DateTime _tapStartTime;
    private MPoint? _tapStartPosition;
    private MPoint? _tapEndPosition;

    public void IfTap(double maxTapDistance, MPoint tapEndPosition, Action<MPoint> onTap)
    {
        if (_tapStartPosition == null) return;
        if (tapEndPosition == null) return; // Note, this uses the tapEndPosition parameter.

        IfTap(maxTapDistance, _tapStartPosition, tapEndPosition, onTap);
    }

    /// <summary>
    /// Use this method in Blazor or other platforms where the mouse up position is unknown. Use this in combination 
    /// with SetLastMovePosition.
    /// </summary>
    /// <param name="maxTapDistance"></param>
    /// <param name="onTap"></param>
    public void IfTap(double maxTapDistance, Action<MPoint> onTap)
    {
        if (_tapStartPosition == null) return;
        if (_tapEndPosition == null) return; // Note, this uses the _tapEndPosition field.

        IfTap(maxTapDistance, _tapStartPosition, _tapEndPosition, onTap);
    }

    private void IfTap(double maxTapDistance, MPoint tapStartPosition, MPoint tapEndPosition, Action<MPoint> onTap)
    {
        if (tapStartPosition == null) return;
        if (tapEndPosition == null) return;

        var duration = (DateTime.Now - _tapStartTime).TotalSeconds;
        var distance = tapEndPosition.Distance(tapStartPosition);
        var isTap = duration < _maxTapDuration && distance < maxTapDistance;

        if (isTap) onTap(tapEndPosition);
    }

    /// <summary>
    /// Call this method during move if the platform does not provide the mouse up position.
    /// </summary>
    /// <param name="position"></param>
    public void SetLastMovePosition(MPoint position)
    {
        _tapEndPosition = position;
    }

    public void SetDownPosition(MPoint position)
    {
        _tapStartTime = DateTime.Now;
        _tapStartPosition = position;
    }
}
