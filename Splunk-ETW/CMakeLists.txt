set(SPLUNK_ETW_SRC
	src/SplunkETW.cs
	src/Manifest.cs
	src/Forwarder.cs
	src/ManifestParser.cs
	src/TraceLoggingParser.cs
	src/ETWWriter.cs
	src/TraceManager.cs
	src/ProviderGuid.cs
	src/NullParser.cs
	src/Parser.cs
)

add_executable(splunk-etw ${SPLUNK_ETW_SRC})
target_compile_options(splunk-etw PRIVATE "/langversion:6")

set_property(TARGET splunk-etw PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.5")
set_property(TARGET splunk-etw PROPERTY VS_DOTNET_REFERENCES
	"System"
	"System.Xml"
	"System.Runtime"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/Microsoft.Diagnostics.Tracing.TraceEvent.dll"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/Microsoft.Diagnostics.FastSerialization.dll"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Splunk.Client/Splunk.Client.dll"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Splunk.ModularInputs/Splunk.ModularInputs.dll"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Newtonsoft.Json/Newtonsoft.Json.dll"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.O365.Security.Native.ETW/Microsoft.O365.Security.Native.ETW.dll"
	"${CMAKE_CURRENT_SOURCE_DIR}/third_party/ini-parser/INIFileParser.dll"
)

install(TARGETS splunk-etw DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/Microsoft.Diagnostics.Tracing.TraceEvent.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/Microsoft.Diagnostics.FastSerialization.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/Dia2Lib.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/OSExtensions.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.Diagnostics.Tracing.TraceEvent/TraceReloggerLib.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Splunk.Client/Splunk.Client.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Splunk.ModularInputs/Splunk.ModularInputs.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Newtonsoft.Json/Newtonsoft.Json.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.O365.Security.Native.ETW/Microsoft.O365.Security.Native.ETW.dll" DESTINATION bin)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/third_party/ini-parser/INIFileParser.dll" DESTINATION bin)


# install config files
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/default/app.conf" DESTINATION default)
install(FILES "${CMAKE_CURRENT_SOURCE_DIR}/README/inputs.conf.spec" DESTINATION README)

if(BUILD_TESTS)
	set(SPLUNK_ETW_TST
		test/ProviderGuid.cs
		test/Forwarder.cs
		test/EventRecord.cs
		test/ManifestParser.cs
		test/TestWriter.cs
		test/NullParser.cs
		test/TraceLoggingParser.cs
		test/TraceManager.cs
	)

	add_library(splunk-etw-test SHARED ${SPLUNK_ETW_TST})
	target_compile_options(splunk-etw-test PRIVATE "/langversion:6")
	add_dependencies(splunk-etw-test splunk-etw)
	set_property(TARGET splunk-etw-test PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.5")
	set_property(TARGET splunk-etw-test PROPERTY VS_DOTNET_REFERENCES
		"System"
		"System.Xml"
		"System.Runtime"
		"Microsoft"
		"Microsoft.VisualStudio.QualityTools.UnitTestFramework"
		"${CMAKE_CURRENT_SOURCE_DIR}/third_party/Microsoft.O365.Security.Native.ETW/Microsoft.O365.Security.Native.ETW.dll"
		"${CMAKE_CURRENT_SOURCE_DIR}/third_party/ini-parser/INIFileParser.dll"
	)

	set_target_properties(splunk-etw-test
	PROPERTIES
	VS_GLOBAL_PROJECT_TYPES "{3AC096D0-A1C2-E12C-1390-A8335801FDAB};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"
	)
endif()