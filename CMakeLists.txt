# Minimum CMake required
cmake_minimum_required(VERSION 3.11)

# Project
project(Splunk-ETW LANGUAGES CSharp VERSION 1.0.0)


SET(EXECUTABLE_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)
SET(LIBRARY_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)

# Config
set_property(GLOBAL PROPERTY USE_FOLDERS ON)

option(BUILD_TESTS "build test solution" OFF)

SET(EXECUTABLE_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)
SET(LIBRARY_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)

# Source directory
add_subdirectory(Splunk-ETW)


# Summary
message(STATUS "Configuration summary")
message(STATUS "Project name                 : ${PROJECT_NAME}")
message(STATUS "Project version              : ${PROJECT_VERSION}")

# Install config files
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/profile/cert.ini" DESTINATION profile)

set(CPACK_GENERATOR "TGZ")
set(CPACK_PACKAGE_FILE_NAME "${PROJECT_NAME}")
include(CPack)