#
NAME ?= Artivity
VERSION ?= 1.5

SOURCE_DIR ?= ../build/Release
SOURCE_FILES ?= Artivity.app

TEMPLATE_DMG ?= template.dmg
TEMPLATE_SIZE ?= 150m

MASTER_DMG=$(NAME)-$(VERSION).dmg
WC_DMG=wc.dmg
WC_DIR=wc


.PHONY: all
all: $(MASTER_DMG)

$(TEMPLATE_DMG): $(TEMPLATE_DMG).zip
	unzip -qn $<

$(WC_DMG): $(TEMPLATE_DMG)
	cp $< $@

$(MASTER_DMG): $(WC_DMG) $(addprefix $(SOURCE_DIR)/,$(SOURCE_FILES))

	mkdir -p $(WC_DIR)
	hdiutil attach "$(WC_DMG)" -noautoopen -quiet -mountpoint "$(WC_DIR)"
	for i in $(SOURCE_FILES); do  \
		rm -rf "$(WC_DIR)/$$i"; \
		cp -R "$(SOURCE_DIR)/$$i" "$(WC_DIR)/$$i"; \
	done

	./buildDmg.sh $(WC_DIR) $@
	WCDEV=$(shell hdiutil info | grep -B1 "$(WC_DIR)$$" | awk 'NR==1{print $$1}';)

	rm -f "$(MASTER_DMG)"
	hdiutil convert "$(WC_DMG)" -quiet -format UDZO -imagekey zlib-level=9 -o "$@"
	rm -rf $(WC_DIR)
	cp $(MASTER_DMG) $(SOURCEDIR)/$(MASTER_DMG)
	@echo

.PHONY: clean
clean:
	-rm -rf $(TEMPLATE_DMG) $(MASTER_DMG) $(WC_DMG)
