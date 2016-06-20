// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

#ifndef _ART_SOFTWAREASSOCIATION_H
#define _ART_SOFTWAREASSOCIATION_H

#include "../Ontologies/art.h"
#include "../Ontologies/rdf.h"
#include "../UriGenerator.h"

#include "Association.h"

namespace artivity
{
	class SoftwareAssociation;

	typedef boost::shared_ptr<SoftwareAssociation> SoftwareAssociationRef;

	class SoftwareAssociation : public Association
	{
	private:
		std::string _version;

	public:
		SoftwareAssociation() : Association(UriGenerator::getUri())
		{
			setType(art::SoftwareAssociation);
		}

		SoftwareAssociation(std::string uriref) : Association(uriref)
		{
			setType(art::SoftwareAssociation);
		}

		SoftwareAssociation(const char* uriref) : Association(uriref)
		{
			setType(art::SoftwareAssociation);
		}

		virtual ~SoftwareAssociation() {}

		std::string getVersion()
		{
			return _version;
		}

		void setVersion(std::string version)
		{
			_version = version;

			setValue(art::version, version.c_str());
		}
	};
}

#endif // _ART_SOFTWAREASSOCIATION_H
