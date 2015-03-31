/*
 * Copyright (C) Johan Engelen 2007 <j.b.c.engelen@utwente.nl>
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include "ui/widget/registered-widget.h"
#include <glibmm/i18n.h>

#include "live_effects/parameter/parameter.h"
#include "live_effects/effect.h"
#include "svg/svg.h"
#include "xml/repr.h"

#include "svg/stringstream.h"

#include "verbs.h"

#define noLPEREALPARAM_DEBUG

namespace Inkscape {

namespace LivePathEffect {


Parameter::Parameter( const Glib::ustring& label, const Glib::ustring& tip,
                      const Glib::ustring& key, Inkscape::UI::Widget::Registry* wr,
                      Effect* effect )
    : param_key(key),
      param_wr(wr),
      param_label(label),
      oncanvas_editable(false),
      widget_is_visible(true),
      param_tooltip(tip),
      param_effect(effect)
{
}

void
Parameter::param_write_to_repr(const char * svgd)
{
    param_effect->getRepr()->setAttribute(param_key.c_str(), svgd);
}

void Parameter::write_to_SVG(void)
{
    gchar * str = param_getSVGValue();
    param_write_to_repr(str);
    g_free(str);
}

/*###########################################
 *   REAL PARAM
 */
ScalarParam::ScalarParam( const Glib::ustring& label, const Glib::ustring& tip,
                      const Glib::ustring& key, Inkscape::UI::Widget::Registry* wr,
                      Effect* effect, gdouble default_value)
    : Parameter(label, tip, key, wr, effect),
      value(default_value),
      min(-G_MAXDOUBLE),
      max(G_MAXDOUBLE),
      integer(false),
      defvalue(default_value),
      digits(2),
      inc_step(0.1),
      inc_page(1),
      add_slider(false)
{
}

ScalarParam::~ScalarParam()
{
}

bool
ScalarParam::param_readSVGValue(const gchar * strvalue)
{
    double newval;
    unsigned int success = sp_svg_number_read_d(strvalue, &newval);
    if (success == 1) {
        param_set_value(newval);
        return true;
    }
    return false;
}

gchar *
ScalarParam::param_getSVGValue() const
{
    Inkscape::SVGOStringStream os;
    os << value;
    gchar * str = g_strdup(os.str().c_str());
    return str;
}

void
ScalarParam::param_set_default()
{
    param_set_value(defvalue);
}

void
ScalarParam::param_set_value(gdouble val)
{
    value = val;
    if (integer)
        value = round(value);
    if (value > max)
        value = max;
    if (value < min)
        value = min;
}

void
ScalarParam::param_set_range(gdouble min, gdouble max)
{
    this->min = min;
    this->max = max;

    param_set_value(value); // reset value to see whether it is in ranges
}

void
ScalarParam::param_make_integer(bool yes)
{
    integer = yes;
    digits = 0;
    inc_step = 1;
    inc_page = 10;
}

Gtk::Widget *
ScalarParam::param_newWidget()
{
    Inkscape::UI::Widget::RegisteredScalar *rsu = Gtk::manage( new Inkscape::UI::Widget::RegisteredScalar(
        param_label, param_tooltip, param_key, *param_wr, param_effect->getRepr(), param_effect->getSPDoc() ) );

    rsu->setValue(value);
    rsu->setDigits(digits);
    rsu->setIncrements(inc_step, inc_page);
    rsu->setRange(min, max);
    rsu->setProgrammatically = false;
    if (add_slider) {
        rsu->addSlider();
    }

    rsu->set_undo_parameters(SP_VERB_DIALOG_LIVE_PATH_EFFECT, _("Change scalar parameter"));

    return dynamic_cast<Gtk::Widget *> (rsu);
}

void
ScalarParam::param_set_digits(unsigned digits)
{
    this->digits = digits;
}

void
ScalarParam::param_set_increments(double step, double page)
{
    inc_step = step;
    inc_page = page;
}




} /* namespace LivePathEffect */
} /* namespace Inkscape */

/*
  Local Variables:
  mode:c++
  c-file-style:"stroustrup"
  c-file-offsets:((innamespace . 0)(inline-open . 0)(case-label . +))
  indent-tabs-mode:nil
  fill-column:99
  End:
*/
// vim: filetype=cpp:expandtab:shiftwidth=4:tabstop=8:softtabstop=4 :
