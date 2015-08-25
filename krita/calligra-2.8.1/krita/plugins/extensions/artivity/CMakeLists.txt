
set(kritabigbrother_PART_SRCS
        artivity.cc
   )

kde4_add_plugin(kritaartivity ${kritabigbrother_PART_SRCS})


target_link_libraries(kritaartivity kritaui artivity)

install(TARGETS kritaartivity DESTINATION ${PLUGIN_INSTALL_DIR})


########### install files ###############

install( FILES  artivity.rc  DESTINATION ${DATA_INSTALL_DIR}/kritaplugins)
install( FILES  kritaartivity.desktop  DESTINATION ${SERVICES_INSTALL_DIR})

