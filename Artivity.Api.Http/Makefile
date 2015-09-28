all:

clean:
	rm -R dist
	rm artivity-service-1.0.0.tar.gz

dist:
	mkdir dist
	cp bin/Release/*.exe dist/
	cp bin/Release/*.dll dist/
	cp artivity.png artivity-service.desktop Makefile COPYING dist/
	chmod 644 dist/*
	chmod 755 dist/*.exe
	cd dist && tar cvzf ../artivity-service-1.0.0.tar.gz *
	rm -R dist

install:
	mkdir $(DESTDIR)/usr/bin/artivity/
	install -D -m 644 *.dll $(DESTDIR)/usr/bin/artivity/
	install -D -m 755 artivity-service.exe $(DESTDIR)/usr/bin/artivity/artivity-service.exe
	install -D -m 644 artivity-service.desktop $(DESTDIR)/usr/share/applications/artivity-service.desktop
	install -D -m 644 artivity.png $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity.png

uninstall:
	rm -Rf $(DESTDIR)/usr/bin/artivity
	rm $(DESTDIR)/usr/share/applications/artivity-service.desktop
	rm $(DESTDIR)/usr/share/icons/Artivity/32x32/artivity.png
