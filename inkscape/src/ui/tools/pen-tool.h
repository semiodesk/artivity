#ifndef SEEN_PEN_CONTEXT_H
#define SEEN_PEN_CONTEXT_H

/** \file 
 * PenTool: a context for pen tool events.
 */

#include "ui/tools/freehand-base.h"
#include "live_effects/effect.h"

#define SP_PEN_CONTEXT(obj) (dynamic_cast<Inkscape::UI::Tools::PenTool*>((Inkscape::UI::Tools::ToolBase*)obj))
#define SP_IS_PEN_CONTEXT(obj) (dynamic_cast<const Inkscape::UI::Tools::PenTool*>((const Inkscape::UI::Tools::ToolBase*)obj) != NULL)

struct SPCtrlLine;

namespace Inkscape {
namespace UI {
namespace Tools {

/**
 * PenTool: a context for pen tool events.
 */
class PenTool : public FreehandBase {
public:
	PenTool();
	PenTool(gchar const *const *cursor_shape, gint hot_x, gint hot_y);
	virtual ~PenTool();

	enum Mode {
	    MODE_CLICK,
	    MODE_DRAG
	};

	enum State {
	    POINT,
	    CONTROL,
	    CLOSE,
	    STOP
	};

    Geom::Point p[5];

    /** \invar npoints in {0, 2, 5}. */
    // npoints somehow determines the type of the node (what does it mean, exactly? the number of Bezier handles?)
    gint npoints;

    Mode mode;
    State state;

    bool polylines_only;
    bool polylines_paraxial;
    int num_clicks;

    unsigned int expecting_clicks_for_LPE; // if positive, finish the path after this many clicks
    Inkscape::LivePathEffect::Effect *waiting_LPE; // if NULL, waiting_LPE_type in SPDrawContext is taken into account
    SPLPEItem *waiting_item;

    SPCanvasItem *c0;
    SPCanvasItem *c1;

    SPCtrlLine *cl0;
    SPCtrlLine *cl1;
    
    bool events_disabled;

	static const std::string prefsPath;

	virtual const std::string& getPrefsPath();

	int nextParaxialDirection(Geom::Point const &pt, Geom::Point const &origin, guint state) const;
	void setPolylineMode();
	bool hasWaitingLPE();
    void waitForLPEMouseClicks(Inkscape::LivePathEffect::EffectType effect_type, unsigned int num_clicks, bool use_polylines = true);

protected:
	virtual void setup();
	virtual void finish();
	virtual void set(const Inkscape::Preferences::Entry& val);
	virtual bool root_handler(GdkEvent* event);
	virtual bool item_handler(SPItem* item, GdkEvent* event);

private:
	bool _handleButtonPress(GdkEventButton const &bevent);
	bool _handleMotionNotify(GdkEventMotion const &mevent);
	bool _handleButtonRelease(GdkEventButton const &revent);
	bool _handle2ButtonPress(GdkEventButton const &bevent);
	bool _handleKeyPress(GdkEvent *event);

	void _setInitialPoint(Geom::Point const p);
	void _setSubsequentPoint(Geom::Point const p, bool statusbar, guint status = 0);
	void _setCtrl(Geom::Point const p, guint state);
	void _finishSegment(Geom::Point p, guint state);

	void _finish(gboolean closed);

	void _resetColors();

	void _disableEvents();
	void _enableEvents();

	void _setToNearestHorizVert(Geom::Point &pt, guint const state, bool snap) const;

	void _setAngleDistanceStatusMessage(Geom::Point const p, int pc_point_to_compare, gchar const *message);

	void _lastpointToLine();
	void _lastpointToCurve();
	void _lastpointMoveScreen(gdouble x, gdouble y);
	void _lastpointMove(gdouble x, gdouble y);
	void _redrawAll();

	void _endpointSnapHandle(Geom::Point &p, guint const state) const;
	void _endpointSnap(Geom::Point &p, guint const state) const;

	void _cancel();
};

}
}
}

#endif /* !SEEN_PEN_CONTEXT_H */

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
