#ifndef SEEN_COLOR_PROFILE_H
#define SEEN_COLOR_PROFILE_H

#include <vector>
#include <glib.h>
#include <sp-object.h>
#include <glibmm/ustring.h>
#include "cms-color-types.h"

struct SPColor;

namespace Inkscape {

enum {
    RENDERING_INTENT_UNKNOWN = 0,
    RENDERING_INTENT_AUTO = 1,
    RENDERING_INTENT_PERCEPTUAL = 2,
    RENDERING_INTENT_RELATIVE_COLORIMETRIC = 3,
    RENDERING_INTENT_SATURATION = 4,
    RENDERING_INTENT_ABSOLUTE_COLORIMETRIC = 5
};

class ColorProfileImpl;


/**
 * Color Profile.
 */
class ColorProfile : public SPObject {
public:
	ColorProfile();
	virtual ~ColorProfile();

    friend cmsHPROFILE colorprofile_get_handle( SPDocument*, guint*, gchar const* );
    friend class CMSSystem;

    static std::vector<Glib::ustring> getBaseProfileDirs();
    static std::vector<Glib::ustring> getProfileFiles();
    static std::vector<std::pair<Glib::ustring, Glib::ustring> > getProfileFilesWithNames();
#if defined(HAVE_LIBLCMS1) || defined(HAVE_LIBLCMS2)
    //icColorSpaceSignature getColorSpace() const;
    ColorSpaceSig getColorSpace() const;
    //icProfileClassSignature getProfileClass() const;
    ColorProfileClassSig getProfileClass() const;
    cmsHTRANSFORM getTransfToSRGB8();
    cmsHTRANSFORM getTransfFromSRGB8();
    cmsHTRANSFORM getTransfGamutCheck();
    bool GamutCheck(SPColor color);

#endif // defined(HAVE_LIBLCMS1) || defined(HAVE_LIBLCMS2)

    gchar* href;
    gchar* local;
    gchar* name;
    gchar* intentStr;
    guint rendering_intent;

protected:
    ColorProfileImpl *impl;

	virtual void build(SPDocument* doc, Inkscape::XML::Node* repr);
	virtual void release();

	virtual void set(unsigned int key, const gchar* value);

	virtual Inkscape::XML::Node* write(Inkscape::XML::Document* doc, Inkscape::XML::Node* repr, guint flags);
};

} // namespace Inkscape

//#define COLORPROFILE_TYPE (Inkscape::colorprofile_get_type())
#define COLORPROFILE(obj) ((Inkscape::ColorProfile*)obj)
#define IS_COLORPROFILE(obj) (dynamic_cast<const Inkscape::ColorProfile*>((SPObject*)obj))

#endif // !SEEN_COLOR_PROFILE_H

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
