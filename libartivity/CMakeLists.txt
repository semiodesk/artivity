cmake_minimum_required (VERSION 2.6)
project (libartivity)

# Version Number
set (libartivity_MAJOR_VERSION 0)
set (libartivity_MINOR_VERSION 9)
set (libartivity_MICRO_VERSION 5)

set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11")

# Find Boost
set(Boost_USE_STATIC_LIBS OFF) 
set(Boost_USE_MULTITHREADED ON)  
set(Boost_USE_STATIC_RUNTIME OFF) 
find_package(Boost 1.60.0 COMPONENTS filesystem thread chrono) 
find_package(curl)

if(Boost_FOUND)
    include_directories(${Boost_INCLUDE_DIRS}) 
    include_directories(${CURL_INCLUDE_DIR})

    add_subdirectory (src)
    message( status " Found sources " ${SOURCE})

    add_library(artivity STATIC ${SOURCE} ${HEADER})

    target_link_libraries(artivity curl ${Boost_LIBRARIES})

endif()



