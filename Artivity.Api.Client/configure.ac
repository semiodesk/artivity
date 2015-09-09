
AC_PREREQ([2.61])
AC_INIT([Artivity Client], [0.8], [hello@semiodesk.com])
AM_INIT_AUTOMAKE([subdir-objects])
LT_PREREQ([2.2])
LT_INIT([dlopen])

AC_CONFIG_SRCDIR([src])

AC_CONFIG_FILES([Makefile
                 src/Makefile])

AC_PROG_CXX
AC_PROG_INSTALL

AC_CHECK_LIB(curl, curl_easy_perform)

AC_CONFIG_FILES([libartivity.pc])


AC_OUTPUT

echo \
"-------------------------------------------------

 ${PACKAGE_NAME} Version ${PACKAGE_VERSION}

 Prefix: '${prefix}'.
 Compiler: '${CC} ${CFLAGS} ${CPPFLAGS}'
 Libraries: '${LIBS}'

 Package features:
   Async Execution: ${async_exec}

 Now type 'make @<:@<target>@:>@'
   where the optional <target> is:
     all                - build all binaries
     install            - install everything

--------------------------------------------------"
