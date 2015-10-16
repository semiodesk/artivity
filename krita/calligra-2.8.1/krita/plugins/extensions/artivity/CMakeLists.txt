include_directories(${KOMAIN_INCLUDES} ${KOTEXT_INCLUDES})

set(krita-extension-artivity_PART_SRCS artivity.cc)

kde4_add_plugin(krita-extension-artivity ${krita-extension-artivity_PART_SRCS})

target_link_libraries(krita-extension-artivity komain kritaui artivity)

install(TARGETS krita-extension-artivity DESTINATION ${PLUGIN_INSTALL_DIR})

########### install files ###############

install(FILES artivity.rc DESTINATION ${DATA_INSTALL_DIR}/kritaplugins)
install(FILES krita-extension-artivity.desktop DESTINATION ${SERVICES_INSTALL_DIR})

include(CPack)
