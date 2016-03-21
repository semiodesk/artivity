AC_PREREQ([2.61])
AC_INIT([libartivity], [0.8.10])

# libtool versioning
LIBARTIVITY_MAJOR_VERSION=0
LIBARTIVITY_MINOR_VERSION=8
LIBARTIVITY_MICRO_VERSION=10
LIBARTIVITY_VERSION=$LIBARTIVITY_MAJOR_VERSION.$LIBARTIVITY_MINOR_VERSION.$LIBARTIVITY_MICRO_VERSION


dnl ******************************
dnl Check for C++11
dnl ******************************
AC_ARG_ENABLE([cxx11],
       AS_HELP_STRING([--enable-cxx11], [compile with C++11]))

if test "x$enable_cxx11" != "xno"; then
	AC_DEFINE(CXX11,1,[Build with C++11])
	CXXFLAGS="$CXXFLAGS -std=c++11"
fi

AC_SUBST(LIBARTIVITY_MAJOR_VERSION)
AC_SUBST(LIBARTIVITY_MINOR_VERSION)
AC_SUBST(LIBARTIVITY_MICRO_VERSION)
AC_SUBST(LIBARTIVITY_VERSION)

LT_VERSION=$LIBARTIVITY_MICRO_VERSION
LT_RELEASE=$LIBARTIVITY_MAJOR_VERSION.$LIBARTIVITY_MINOR_VERSION

AC_SUBST(LT_VERSION)
AC_SUBST(LT_RELEASE)

VERSION=$LIBARTIVITY_VERSION
PACKAGE=libartivity

AM_INIT_AUTOMAKE([subdir-objects])
LT_PREREQ([2.2])
LT_INIT([dlopen])

AC_CONFIG_SRCDIR([src])
AC_CONFIG_FILES([Makefile src/Makefile])
AC_CONFIG_FILES([libartivity.pc])

AC_PROG_CXX
AC_PROG_INSTALL

AC_CHECK_LIB(curl, curl_easy_perform)

AC_OUTPUT

echo \
"-------------------------------------------------

 ${PACKAGE_NAME} version ${PACKAGE_VERSION}

 Prefix: '${prefix}'.
 Compiler: '${CXX} ${CXXFLAGS}'
 Libraries: '${LIBS}'

 Package features:
   Async Execution: ${async_exec}

 Now type 'make @<:@<target>@:>@'
   where the optional <target> is:
     all                - build all binaries
     install            - install everything

--------------------------------------------------"
