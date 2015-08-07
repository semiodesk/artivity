/* Authors:
 *   Krzysztof Kosiński <tweenk.pl@gmail.com>
 *   Jon A. Cruz <jon@joncruz.org>
 *
 * Copyright (C) 2009 Authors
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include "ui/tool/curve-drag-point.h"
#include <glib/gi18n.h>
#include <2geom/bezier-curve.h>
#include "desktop.h"
#include "ui/tool/control-point-selection.h"
#include "ui/tool/event-utils.h"
#include "ui/tool/multi-path-manipulator.h"
#include "ui/tool/path-manipulator.h"
#include "ui/tool/node.h"

namespace Inkscape {
namespace UI {


bool CurveDragPoint::_drags_stroke = false;
bool CurveDragPoint::_segment_was_degenerate = false;

CurveDragPoint::CurveDragPoint(PathManipulator &pm) :
    ControlPoint(pm._multi_path_manipulator._path_data.node_data.desktop, Geom::Point(), SP_ANCHOR_CENTER,
                 CTRL_TYPE_INVISIPOINT,
                 invisible_cset, pm._multi_path_manipulator._path_data.dragpoint_group),
      _pm(pm)
{
    setVisible(false);
}

bool CurveDragPoint::_eventHandler(Inkscape::UI::Tools::ToolBase *event_context, GdkEvent *event)
{
    // do not process any events when the manipulator is empty
    if (_pm.empty()) {
        setVisible(false);
        return false;
    }
    return ControlPoint::_eventHandler(event_context, event);
}

bool CurveDragPoint::grabbed(GdkEventMotion */*event*/)
{
    _pm._selection.hideTransformHandles();
    NodeList::iterator second = first.next();

    // move the handles to 1/3 the length of the segment for line segments
    if (first->front()->isDegenerate() && second->back()->isDegenerate()) {
        _segment_was_degenerate = true;

        // delta is a vector equal 1/3 of distance from first to second
        Geom::Point delta = (second->position() - first->position()) / 3.0;
        first->front()->move(first->front()->position() + delta);
        second->back()->move(second->back()->position() - delta);

        _pm.update();
    } else {
        _segment_was_degenerate = false;
    }
    return false;
}

void CurveDragPoint::dragged(Geom::Point &new_pos, GdkEventMotion *event)
{
    NodeList::iterator second = first.next();

    // special cancel handling - retract handles when if the segment was degenerate
    if (_is_drag_cancelled(event) && _segment_was_degenerate) {
        first->front()->retract();
        second->back()->retract();
        _pm.update();
        return;
    }

    // Magic Bezier Drag Equations follow!
    // "weight" describes how the influence of the drag should be distributed
    // among the handles; 0 = front handle only, 1 = back handle only.
    double weight, t = _t;
    if (t <= 1.0 / 6.0) weight = 0;
    else if (t <= 0.5) weight = (pow((6 * t - 1) / 2.0, 3)) / 2;
    else if (t <= 5.0 / 6.0) weight = (1 - pow((6 * (1-t) - 1) / 2.0, 3)) / 2 + 0.5;
    else weight = 1;

    Geom::Point delta = new_pos - position();
    Geom::Point offset0 = ((1-weight)/(3*t*(1-t)*(1-t))) * delta;
    Geom::Point offset1 = (weight/(3*t*t*(1-t))) * delta;

    first->front()->move(first->front()->position() + offset0);
    second->back()->move(second->back()->position() + offset1);

    _pm.update();
}

void CurveDragPoint::ungrabbed(GdkEventButton *)
{
    _pm._updateDragPoint(_desktop->d2w(position()));
    _pm._commit(_("Drag curve"));
    _pm._selection.restoreTransformHandles();
}

bool CurveDragPoint::clicked(GdkEventButton *event)
{
    // This check is probably redundant
    if (!first || event->button != 1) return false;
    // the next iterator can be invalid if we click very near the end of path
    NodeList::iterator second = first.next();
    if (!second) return false;

    // insert nodes on Ctrl+Alt+click
    if (held_control(*event) && held_alt(*event)) {
        _insertNode(false);
        return true;
    }

    if (held_shift(*event)) {
        // if both nodes of the segment are selected, deselect;
        // otherwise add to selection
        if (first->selected() && second->selected())  {
            _pm._selection.erase(first.ptr());
            _pm._selection.erase(second.ptr());
        } else {
            _pm._selection.insert(first.ptr());
            _pm._selection.insert(second.ptr());
        }
    } else {
        // without Shift, take selection
        _pm._selection.clear();
        _pm._selection.insert(first.ptr());
        _pm._selection.insert(second.ptr());
    }
    return true;
}

bool CurveDragPoint::doubleclicked(GdkEventButton *event)
{
    if (event->button != 1 || !first || !first.next()) return false;
    _insertNode(true);
    return true;
}

void CurveDragPoint::_insertNode(bool take_selection)
{
    // The purpose of this call is to make way for the just created node.
    // Otherwise clicks on the new node would only work after the user moves the mouse a bit.
    // PathManipulator will restore visibility when necessary.
    setVisible(false);
    NodeList::iterator inserted = _pm.subdivideSegment(first, _t);
    if (take_selection) {
        _pm._selection.clear();
    }
    _pm._selection.insert(inserted.ptr());

    _pm.update(true);
    _pm._commit(_("Add node"));
}

Glib::ustring CurveDragPoint::_getTip(unsigned state) const
{
    if (_pm.empty()) return "";
    if (!first || !first.next()) return "";
    bool linear = first->front()->isDegenerate() && first.next()->back()->isDegenerate();
    if (state_held_shift(state)) {
        return C_("Path segment tip",
            "<b>Shift</b>: click to toggle segment selection");
    }
    if (state_held_control(state) && state_held_alt(state)) {
        return C_("Path segment tip",
            "<b>Ctrl+Alt</b>: click to insert a node");
    }
    if (linear) {
        return C_("Path segment tip",
            "<b>Linear segment</b>: drag to convert to a Bezier segment, "
            "doubleclick to insert node, click to select (more: Shift, Ctrl+Alt)");
    } else {
        return C_("Path segment tip",
            "<b>Bezier segment</b>: drag to shape the segment, doubleclick to insert node, "
            "click to select (more: Shift, Ctrl+Alt)");
    }
}

} // namespace UI
} // namespace Inkscape

/*
  Local Variables:
  mode:c++
  c-file-style:"stroustrup"
  c-file-offsets:((innamespace . 0)(inline-open . 0)(case-label . +))
  indent-tabs-mode:nil
  fill-column:99
  End:
*/
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4:fileencoding=utf-8:textwidth=99 :
