/** @file
 * @brief New From Template main dialog - implementation
 */
/* Authors:
 *   Jan Darowski <jan.darowski@gmail.com>, supervised by Krzysztof Kosiński    
 *
 * Copyright (C) 2013 Authors
 * Released under GNU GPL, read the file 'COPYING' for more information
 */


#include "new-from-template.h"
#include "file.h"

#include <gtkmm/alignment.h>
#include <glibmm/i18n.h>


namespace Inkscape {
namespace UI {


NewFromTemplate::NewFromTemplate()
    : _create_template_button(_("Create from template"))
{
    set_title(_("New From Template"));
    resize(400, 400);
    
    get_vbox()->pack_start(_main_widget);
   
    Gtk::Alignment *align;
    align = Gtk::manage(new Gtk::Alignment(Gtk::ALIGN_END, Gtk::ALIGN_CENTER, 0.0, 0.0));
    get_vbox()->pack_end(*align, Gtk::PACK_SHRINK);
    align->set_padding(0, 0, 0, 15);
    align->add(_create_template_button);
    
    _create_template_button.signal_clicked().connect(
    sigc::mem_fun(*this, &NewFromTemplate::_createFromTemplate));
   
    show_all();
}


void NewFromTemplate::_createFromTemplate()
{
    _main_widget.createTemplate();
    _onClose();
}

void NewFromTemplate::_onClose()
{
    response(0);
}

void NewFromTemplate::load_new_from_template()
{
    NewFromTemplate dl;
    dl.run();
}

}
}
