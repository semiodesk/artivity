SUBDIRS = src

pkgconfigdir = $(libdir)/pkgconfig

libartivity.pc: libartivity.pc.in
	sed -e 's![@]prefix[@]!$(prefix)!g' \
		-e 's![@]exec_prefix[@]!$(exec_prefix)!g' \
		-e 's![@]includedir[@]!$(includedir)!g' \
		-e 's![@]libdir[@]!$(libdir)!g' \
		-e 's![@]PACKAGE_VERSION[@]!$(PACKAGE_VERSION)!g' \
		$(srcdir)libartivity.pc.in > $@

nodist_pkgconfig_DATA = libartivity.pc
