/*
 * Abstract base class for dynamic control widgets
 *
 * Authors:
 *   Lauris Kaplinski <lauris@kaplinski.com>
 *   bulia byak <buliabyak@users.sf.net>
 *   Jon A. Cruz <jon@joncruz.org>
 *
 * Copyright (C) 1999-2002 Lauris Kaplinski
 * Copyright (C) 2000-2001 Ximian, Inc.
 * Copyright (C) 2012 Authors
 *
 * Released under GNU GPL, read the file 'COPYING' for more information
 */

#include "macros.h"
#include "document.h"
#include "sp-widget.h"
#include "helper/sp-marshal.h"

using Inkscape::SPWidgetImpl;

enum {
    CONSTRUCT,
    MODIFY_SELECTION,
    CHANGE_SELECTION,
    SET_SELECTION,
    LAST_SIGNAL
};

namespace Inkscape {

class SPWidgetImpl
{
public:
    SPWidgetImpl(SPWidget &target);
    ~SPWidgetImpl();

    static void classInit(SPWidgetClass *klass);
    static void init(SPWidget *widget);
    static void dispose(GObject *object);
    static void show(GtkWidget *widget);
    static void hide(GtkWidget *widget);

#if GTK_CHECK_VERSION(3,0,0)
    static void getPreferredWidth(GtkWidget *widget,
                                  gint *minimal_width,
                                  gint *natural_width);

    static void getPreferredHeight(GtkWidget *widget,
                                   gint *minimal_height,
                                   gint *natural_height);
    static gboolean draw(GtkWidget *widget, cairo_t *cr);
#else
    static void sizeRequest(GtkWidget *widget, GtkRequisition *requisition);
    static gboolean expose(GtkWidget *widget, GdkEventExpose *event);
#endif

    static void sizeAllocate(GtkWidget *widget, GtkAllocation *allocation);
    static void modifySelectionCB(Application *inkscape, Selection *selection, guint flags, SPWidget *spw);
    static void changeSelectionCB(Application *inkscape, Selection *selection, SPWidget *spw);
    static void setSelectionCB(Application *inkscape, Selection *selection, SPWidget *spw);

    static GtkWidget *constructGlobal(SPWidget *spw, Inkscape::Application *inkscape);

    void modifySelection(Application *inkscape, Selection *selection, guint flags);
    void changeSelection(Application *inkscape, Selection *selection);
    void setSelection(Application *inkscape, Selection *selection);

private:
    static GtkBinClass *parentClass;
    static guint signals[LAST_SIGNAL];

    SPWidget &_target;
};

GtkBinClass *SPWidgetImpl::parentClass = 0;
guint SPWidgetImpl::signals[LAST_SIGNAL] = {0};

} // namespace Inkscape

GType SPWidget::getType()
{
    static GType type = 0;
    if (!type) {
        static GTypeInfo const info = {
            sizeof(SPWidgetClass),
            NULL, NULL,
            reinterpret_cast<GClassInitFunc>(SPWidgetImpl::classInit),
            NULL, NULL,
            sizeof(SPWidget),
            0,
            reinterpret_cast<GInstanceInitFunc>(SPWidgetImpl::init),
            NULL
        };
        type = g_type_register_static(GTK_TYPE_BIN,
                                      "SPWidget",
                                      &info,
                                      static_cast<GTypeFlags>(0));
    }
    return type;
}

namespace Inkscape {

SPWidgetImpl::SPWidgetImpl(SPWidget &target) :
    _target(target)
{
}

SPWidgetImpl::~SPWidgetImpl()
{
}

void SPWidgetImpl::classInit(SPWidgetClass *klass)
{
    GObjectClass *object_class = reinterpret_cast<GObjectClass *>(klass);
    GtkWidgetClass *widget_class = reinterpret_cast<GtkWidgetClass *>(klass);

    parentClass = reinterpret_cast<GtkBinClass *>(g_type_class_peek_parent(klass));

    object_class->dispose = SPWidgetImpl::dispose;

    signals[CONSTRUCT] =        g_signal_new ("construct",
                                              G_TYPE_FROM_CLASS(object_class),
                                              G_SIGNAL_RUN_FIRST,
                                              G_STRUCT_OFFSET(SPWidgetClass, construct),
                                              NULL, NULL,
                                              g_cclosure_marshal_VOID__VOID,
                                              G_TYPE_NONE, 0);

    signals[CHANGE_SELECTION] = g_signal_new ("change_selection",
                                              G_TYPE_FROM_CLASS(object_class),
                                              G_SIGNAL_RUN_FIRST,
                                              G_STRUCT_OFFSET(SPWidgetClass, change_selection),
                                              NULL, NULL,
                                              g_cclosure_marshal_VOID__POINTER,
                                              G_TYPE_NONE, 1,
                                              G_TYPE_POINTER);

    signals[MODIFY_SELECTION] = g_signal_new ("modify_selection",
                                              G_TYPE_FROM_CLASS(object_class),
                                              G_SIGNAL_RUN_FIRST,
                                              G_STRUCT_OFFSET(SPWidgetClass, modify_selection),
                                              NULL, NULL,
                                              sp_marshal_VOID__POINTER_UINT,
                                              G_TYPE_NONE, 2,
                                              G_TYPE_POINTER, G_TYPE_UINT);

    signals[SET_SELECTION] =    g_signal_new ("set_selection",
                                              G_TYPE_FROM_CLASS(object_class),
                                              G_SIGNAL_RUN_FIRST,
                                              G_STRUCT_OFFSET(SPWidgetClass, set_selection),
                                              NULL, NULL,
                                              g_cclosure_marshal_VOID__POINTER,
                                              G_TYPE_NONE, 1,
                                              G_TYPE_POINTER);

    widget_class->show = SPWidgetImpl::show;
    widget_class->hide = SPWidgetImpl::hide;
#if GTK_CHECK_VERSION(3,0,0)
    widget_class->get_preferred_width = SPWidgetImpl::getPreferredWidth;
    widget_class->get_preferred_height = SPWidgetImpl::getPreferredHeight;
    widget_class->draw = SPWidgetImpl::draw;
#else
    widget_class->size_request = SPWidgetImpl::sizeRequest;
    widget_class->expose_event = SPWidgetImpl::expose;
#endif
    widget_class->size_allocate = SPWidgetImpl::sizeAllocate;
}

void SPWidgetImpl::init(SPWidget *spw)
{
    spw->inkscape = NULL;

    spw->_impl = new SPWidgetImpl(*spw); // ctor invoked after all other init
}

void SPWidgetImpl::dispose(GObject *object)
{
    SPWidget *spw = reinterpret_cast<SPWidget *>(object);

    if (spw->inkscape) {
        // Disconnect signals

        // the checks are necessary because when destroy is caused by the program shutting down,
        // the inkscape object may already be (partly?) invalid --bb
        if (G_IS_OBJECT(spw->inkscape) && G_OBJECT_GET_CLASS(spw->inkscape)) {
            sp_signal_disconnect_by_data(spw->inkscape, spw);
        }
        spw->inkscape = NULL;
    }

    delete spw->_impl;
    spw->_impl = 0;

    if (reinterpret_cast<GObjectClass *>(parentClass)->dispose) {
        (*reinterpret_cast<GObjectClass *>(parentClass)->dispose)(object);
    }
}

void SPWidgetImpl::show(GtkWidget *widget)
{
    SPWidget *spw = SP_WIDGET(widget);

    if (spw->inkscape) {
        // Connect signals
        g_signal_connect(spw->inkscape, "modify_selection", G_CALLBACK(SPWidgetImpl::modifySelectionCB), spw);
        g_signal_connect(spw->inkscape, "change_selection", G_CALLBACK(SPWidgetImpl::changeSelectionCB), spw);
        g_signal_connect(spw->inkscape, "set_selection", G_CALLBACK(SPWidgetImpl::setSelectionCB), spw);
    }

    if (reinterpret_cast<GtkWidgetClass *>(parentClass)->show) {
        (*reinterpret_cast<GtkWidgetClass *>(parentClass)->show)(widget);
    }
}

void SPWidgetImpl::hide(GtkWidget *widget)
{
    SPWidget *spw = SP_WIDGET (widget);

    if (spw->inkscape) {
        // Disconnect signals
        sp_signal_disconnect_by_data(spw->inkscape, spw);
    }

    if (reinterpret_cast<GtkWidgetClass *>(parentClass)->hide) {
        (*reinterpret_cast<GtkWidgetClass *>(parentClass)->hide)(widget);
    }
}

#if GTK_CHECK_VERSION(3,0,0)
gboolean SPWidgetImpl::draw(GtkWidget *widget, cairo_t *cr)
#else
gboolean SPWidgetImpl::expose(GtkWidget *widget, GdkEventExpose *event)
#endif
{
    GtkBin    *bin = GTK_BIN(widget);
    GtkWidget *child = gtk_bin_get_child(bin);

    if (child) {
#if GTK_CHECK_VERSION(3,0,0)
        gtk_container_propagate_draw(GTK_CONTAINER(widget), child, cr);
#else
        gtk_container_propagate_expose(GTK_CONTAINER(widget), child, event);
#endif
    }

    return FALSE;
}

#if GTK_CHECK_VERSION(3,0,0)
void SPWidgetImpl::getPreferredWidth(GtkWidget *widget, gint *minimal_width, gint *natural_width)
{
    GtkBin    *bin   = GTK_BIN(widget);
    GtkWidget *child = gtk_bin_get_child(bin);

    if(child) {
        gtk_widget_get_preferred_width(child, minimal_width, natural_width);
    }
}

void SPWidgetImpl::getPreferredHeight(GtkWidget *widget, gint *minimal_height, gint *natural_height)
{
    GtkBin    *bin   = GTK_BIN(widget);
    GtkWidget *child = gtk_bin_get_child(bin);

    if(child) {
        gtk_widget_get_preferred_height(child, minimal_height, natural_height);
    }
}
#else
void SPWidgetImpl::sizeRequest(GtkWidget *widget, GtkRequisition *requisition)
{
    GtkBin    *bin   = GTK_BIN(widget);
    GtkWidget *child = gtk_bin_get_child(bin);

    if (child) {
        gtk_widget_size_request(child, requisition);
    }
}
#endif

void SPWidgetImpl::sizeAllocate(GtkWidget *widget, GtkAllocation *allocation)
{
    gtk_widget_set_allocation(widget, allocation);

    GtkBin        *bin   = GTK_BIN(widget);
    GtkWidget     *child = gtk_bin_get_child(bin);

    if (child) {
        gtk_widget_size_allocate(child, allocation);
    }
}

GtkWidget *SPWidgetImpl::constructGlobal(SPWidget *spw, Inkscape::Application *inkscape)
{
    g_return_val_if_fail(!spw->inkscape, NULL);

    spw->inkscape = inkscape;
    if (gtk_widget_get_visible(GTK_WIDGET(spw))) {
        g_signal_connect(inkscape, "modify_selection", G_CALLBACK(SPWidgetImpl::modifySelectionCB), spw);
        g_signal_connect(inkscape, "change_selection", G_CALLBACK(SPWidgetImpl::changeSelectionCB), spw);
        g_signal_connect(inkscape, "set_selection", G_CALLBACK(SPWidgetImpl::setSelectionCB), spw);
    }

    g_signal_emit(spw, signals[CONSTRUCT], 0);

    return GTK_WIDGET(spw);
}

void SPWidgetImpl::modifySelectionCB(Application *inkscape, Selection *selection, guint flags, SPWidget *spw)
{
    spw->_impl->modifySelection(inkscape, selection, flags);
}

void SPWidgetImpl::changeSelectionCB(Application *inkscape, Selection *selection, SPWidget *spw)
{
    spw->_impl->changeSelection(inkscape, selection);
}

void SPWidgetImpl::setSelectionCB(Application *inkscape, Selection *selection, SPWidget *spw)
{
    spw->_impl->setSelection(inkscape, selection);
}

void SPWidgetImpl::modifySelection(Application * /*inkscape*/, Selection *selection, guint flags)
{
    g_signal_emit(&_target, signals[MODIFY_SELECTION], 0, selection, flags);
}

void SPWidgetImpl::changeSelection(Application * /*inkscape*/, Selection *selection)
{
    g_signal_emit(&_target, signals[CHANGE_SELECTION], 0, selection);
}

void SPWidgetImpl::setSelection(Application * /*inkscape*/, Selection *selection)
{
    // Emit "set_selection" signal
    g_signal_emit(&_target, signals[SET_SELECTION], 0, selection);
    // Inkscape will force "change_selection" anyways
}

} // namespace Inkscape

// Methods

GtkWidget *sp_widget_new_global(Inkscape::Application *inkscape)
{
    SPWidget *spw = reinterpret_cast<SPWidget*>(g_object_new(SP_TYPE_WIDGET, NULL));

    if (!SPWidgetImpl::constructGlobal(spw, inkscape)) {
        g_object_unref(spw);
        spw = 0;
    }

    return reinterpret_cast<GtkWidget *>(spw);
}

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
