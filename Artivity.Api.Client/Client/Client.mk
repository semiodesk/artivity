##
## Auto Generated makefile by CodeLite IDE
## any manual changes will be erased      
##
## Debug
ProjectName            :=Client
ConfigurationName      :=Debug
WorkspacePath          := "/home/sebastian/Projects/Artivity/Artivity.Api.Client"
ProjectPath            := "/home/sebastian/Projects/Artivity/Artivity.Api.Client/Client"
IntermediateDirectory  :=./Debug
OutDir                 := $(IntermediateDirectory)
CurrentFileName        :=
CurrentFilePath        :=
CurrentFileFullPath    :=
User                   :=Sebastian
Date                   :=07/08/15
CodeLitePath           :="/home/sebastian/.codelite"
LinkerName             :=/usr/bin/g++
SharedObjectLinkerName :=/usr/bin/g++ -shared -fPIC
ObjectSuffix           :=.o
DependSuffix           :=.o.d
PreprocessSuffix       :=.i
DebugSwitch            :=-g 
IncludeSwitch          :=-I
LibrarySwitch          :=-l
OutputSwitch           :=-o 
LibraryPathSwitch      :=-L
PreprocessorSwitch     :=-D
SourceSwitch           :=-c 
OutputFile             :=$(IntermediateDirectory)/$(ProjectName)
Preprocessors          :=
ObjectSwitch           :=-o 
ArchiveOutputSwitch    := 
PreprocessOnlySwitch   :=-E
ObjectsFileList        :="Client.txt"
PCHCompileFlags        :=
MakeDirCommand         :=mkdir -p
LinkOptions            :=  
IncludePath            :=  $(IncludeSwitch). $(IncludeSwitch). 
IncludePCH             := 
RcIncludePath          := 
Libs                   := 
ArLibs                 :=  
LibPath                := $(LibraryPathSwitch). 

##
## Common variables
## AR, CXX, CC, AS, CXXFLAGS and CFLAGS can be overriden using an environment variables
##
AR       := /usr/bin/ar rcu
CXX      := /usr/bin/g++
CC       := /usr/bin/gcc
CXXFLAGS :=  -g -O0 -Wall $(Preprocessors)
CFLAGS   :=  -g -O0 -Wall $(Preprocessors)
ASFLAGS  := 
AS       := /usr/bin/as


##
## User defined environment variables
##
CodeLiteDir:=/usr/share/codelite
Objects0=$(IntermediateDirectory)/main.cpp$(ObjectSuffix) $(IntermediateDirectory)/Resource.cpp$(ObjectSuffix) $(IntermediateDirectory)/ActivityLog.cpp$(ObjectSuffix) $(IntermediateDirectory)/Activity.cpp$(ObjectSuffix) $(IntermediateDirectory)/Serializer.cpp$(ObjectSuffix) 



Objects=$(Objects0) 

##
## Main Build Targets 
##
.PHONY: all clean PreBuild PrePreBuild PostBuild MakeIntermediateDirs
all: $(OutputFile)

$(OutputFile): $(IntermediateDirectory)/.d $(Objects) 
	@$(MakeDirCommand) $(@D)
	@echo "" > $(IntermediateDirectory)/.d
	@echo $(Objects0)  > $(ObjectsFileList)
	$(LinkerName) $(OutputSwitch)$(OutputFile) @$(ObjectsFileList) $(LibPath) $(Libs) $(LinkOptions)

MakeIntermediateDirs:
	@test -d ./Debug || $(MakeDirCommand) ./Debug


$(IntermediateDirectory)/.d:
	@test -d ./Debug || $(MakeDirCommand) ./Debug

PreBuild:


##
## Objects
##
$(IntermediateDirectory)/main.cpp$(ObjectSuffix): main.cpp $(IntermediateDirectory)/main.cpp$(DependSuffix)
	$(CXX) $(IncludePCH) $(SourceSwitch) "/home/sebastian/Projects/Artivity/Artivity.Api.Client/Client/main.cpp" $(CXXFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/main.cpp$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/main.cpp$(DependSuffix): main.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/main.cpp$(ObjectSuffix) -MF$(IntermediateDirectory)/main.cpp$(DependSuffix) -MM "main.cpp"

$(IntermediateDirectory)/main.cpp$(PreprocessSuffix): main.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/main.cpp$(PreprocessSuffix) "main.cpp"

$(IntermediateDirectory)/Resource.cpp$(ObjectSuffix): Resource.cpp $(IntermediateDirectory)/Resource.cpp$(DependSuffix)
	$(CXX) $(IncludePCH) $(SourceSwitch) "/home/sebastian/Projects/Artivity/Artivity.Api.Client/Client/Resource.cpp" $(CXXFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/Resource.cpp$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/Resource.cpp$(DependSuffix): Resource.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/Resource.cpp$(ObjectSuffix) -MF$(IntermediateDirectory)/Resource.cpp$(DependSuffix) -MM "Resource.cpp"

$(IntermediateDirectory)/Resource.cpp$(PreprocessSuffix): Resource.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/Resource.cpp$(PreprocessSuffix) "Resource.cpp"

$(IntermediateDirectory)/ActivityLog.cpp$(ObjectSuffix): ActivityLog.cpp $(IntermediateDirectory)/ActivityLog.cpp$(DependSuffix)
	$(CXX) $(IncludePCH) $(SourceSwitch) "/home/sebastian/Projects/Artivity/Artivity.Api.Client/Client/ActivityLog.cpp" $(CXXFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/ActivityLog.cpp$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/ActivityLog.cpp$(DependSuffix): ActivityLog.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/ActivityLog.cpp$(ObjectSuffix) -MF$(IntermediateDirectory)/ActivityLog.cpp$(DependSuffix) -MM "ActivityLog.cpp"

$(IntermediateDirectory)/ActivityLog.cpp$(PreprocessSuffix): ActivityLog.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/ActivityLog.cpp$(PreprocessSuffix) "ActivityLog.cpp"

$(IntermediateDirectory)/Activity.cpp$(ObjectSuffix): Activity.cpp $(IntermediateDirectory)/Activity.cpp$(DependSuffix)
	$(CXX) $(IncludePCH) $(SourceSwitch) "/home/sebastian/Projects/Artivity/Artivity.Api.Client/Client/Activity.cpp" $(CXXFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/Activity.cpp$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/Activity.cpp$(DependSuffix): Activity.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/Activity.cpp$(ObjectSuffix) -MF$(IntermediateDirectory)/Activity.cpp$(DependSuffix) -MM "Activity.cpp"

$(IntermediateDirectory)/Activity.cpp$(PreprocessSuffix): Activity.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/Activity.cpp$(PreprocessSuffix) "Activity.cpp"

$(IntermediateDirectory)/Serializer.cpp$(ObjectSuffix): Serializer.cpp $(IntermediateDirectory)/Serializer.cpp$(DependSuffix)
	$(CXX) $(IncludePCH) $(SourceSwitch) "/home/sebastian/Projects/Artivity/Artivity.Api.Client/Client/Serializer.cpp" $(CXXFLAGS) $(ObjectSwitch)$(IntermediateDirectory)/Serializer.cpp$(ObjectSuffix) $(IncludePath)
$(IntermediateDirectory)/Serializer.cpp$(DependSuffix): Serializer.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) -MG -MP -MT$(IntermediateDirectory)/Serializer.cpp$(ObjectSuffix) -MF$(IntermediateDirectory)/Serializer.cpp$(DependSuffix) -MM "Serializer.cpp"

$(IntermediateDirectory)/Serializer.cpp$(PreprocessSuffix): Serializer.cpp
	@$(CXX) $(CXXFLAGS) $(IncludePCH) $(IncludePath) $(PreprocessOnlySwitch) $(OutputSwitch) $(IntermediateDirectory)/Serializer.cpp$(PreprocessSuffix) "Serializer.cpp"


-include $(IntermediateDirectory)/*$(DependSuffix)
##
## Clean
##
clean:
	$(RM) -r ./Debug/


